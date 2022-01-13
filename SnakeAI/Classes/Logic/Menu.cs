using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmNS;
using System.Windows.Forms;
using SnakeGameNS;
using System.Diagnostics;
using NeuralNetworkNS;
using System.Runtime.Serialization.Formatters.Binary;

namespace SnakeAI {

  /// <summary>
  /// The program menu, where all the magic happens.
  /// </summary>
  public class Menu {
    // Skal i GUI!!!
    private GeneticAlgorithm geneticAlgorithm;
    private GeneticSettings geneticSettings;
    private SnakeSettings snakeSettings;
    private NetworkSettings networkSettings;
    private Replayer replayer;

    public Menu(GeneticAlgorithm geneticAlgo, GeneticSettings geneticSettings,
                SnakeSettings snakeSettings, NetworkSettings networkSettings) {
      this.geneticAlgorithm = geneticAlgo;
      this.geneticSettings = geneticSettings;
      this.snakeSettings = snakeSettings;
      this.networkSettings = networkSettings;
      replayer = new Replayer(snakeSettings, networkSettings);
    }

    public void WelcomeScreen() {
      int input;
      Console.WriteLine("----SNAKE AI----");
      do {
        Console.WriteLine("(1) Show menu");
        Console.WriteLine("(2) Run genetic algo!");
        Console.WriteLine("\nNB: press enter any time during calculations to show menu");
        input = Program.GetIntFromConsoleChar();
        if(input == 1) {
          Show();
        }
      } while(input != 2);
      Console.WriteLine("\nHERE WE GO!\n");
    }

    public void Show() {
      Console.Clear();
      Console.WriteLine("\n\nKEY PRESSED! HERE ARE YOUR CHOICES:");
      Console.WriteLine("(1) GUI: Show play of best agent in current population");
      Console.WriteLine("(2) GUI: Load agent from file and show play");
      Console.WriteLine("(3) ASCII: Play snake (for testing game)");
      Console.WriteLine("(4) Save weights of best agent in current population");
      Console.WriteLine("(5) Change settings (not implemented)");
      Console.WriteLine("(6) Calculate convergence");
      Console.WriteLine("(7) Save population");
      Console.WriteLine("\n(8) EXIT MENU (and go back to calculations)");
      Console.WriteLine("\n(9) EXIT PROGRAM");
      int choice = Program.GetIntFromConsoleChar();
      Console.Clear();

      switch(choice) {
        case 1:
          replayer.ShowLiveReplay(geneticAlgorithm.TopKAgents.Best.Agent);
          break;
        case 2:
          LoadAgentGUI(ProgramSettings.DIRECTORY_PATH, geneticSettings.GeneCount);
          break;
        case 3:
          PlaySnake();
          break;
        case 4:
          SaveWeightsOfBestAgent(geneticAlgorithm.TopKAgents.Best.Agent, ProgramSettings.FILE_NAME_SAVE_BEST_AGENT);
          Console.WriteLine("Weights saved succesfully!"); // Tjek det lige
          Console.WriteLine("\nPress any key to return to calculations...");
          Console.ReadKey();
          break;
        case 5:
          ChangeSettings();
          break;
        case 6:
          ConvergenceCalculator c = new ConvergenceCalculator(geneticAlgorithm.CurrentPopulation.Agents);
          c.CalculateConvergence();
          break;
        case 7:
          SavePopulation(geneticAlgorithm.CurrentPopulation, ProgramSettings.FILE_NAME_SAVE_BEST_AGENT);
          break;
        case 8:
          break;
        case 9:
          Environment.Exit(1);
          break;
        default:
          break;
      }
    }

    // En snake player??
    public void PlaySnake() {
      int lastKeyPressed;

      SnakeSettings snakeSettings = new SnakeSettings(ProgramSettings.GRID_ROWS, ProgramSettings.GRID_COLUMNS, ProgramSettings.SIDE_LENGTH, ProgramSettings.POINTS_PER_FOOD_EATEN,
                                    new Random());

      do {
        SnakeGame snakeGame = new SnakeGame(snakeSettings);
        // Game loop
        do {
          Console.Clear();
          snakeGame.Display();
          lastKeyPressed = GetIntFromConsoleArrow();
          snakeGame.UpdateDirection(lastKeyPressed);
        } while(snakeGame.Snake.IsAlive);
        Console.Clear();
        snakeGame.DisplayEndScreen();

        Console.Write("\nRetry (any key) or quit (enter)?");
      } while(Console.ReadKey().Key != ConsoleKey.Enter);
    }

    private int GetIntFromConsoleArrow() {
      int intArrow = 0;
      bool isArrowKey = true;
      ConsoleKeyInfo userInput = default(ConsoleKeyInfo);

      do {
        userInput = Console.ReadKey();
        // Check key pressed
        switch(userInput.Key) {
          case ConsoleKey.UpArrow:
            intArrow = 1;
            break;
          case ConsoleKey.DownArrow:
            intArrow = 2;
            break;
          case ConsoleKey.LeftArrow:
            intArrow = 3;
            break;
          case ConsoleKey.RightArrow:
            intArrow = 4;
            break;
          default:
            isArrowKey = false; // Key not found
            break;
        }
      } while(!isArrowKey);

      return intArrow;
    }

    // Creates or overwrites text file at provided path with weight of best agent
    public void SaveWeightsOfBestAgent(Agent agent, string filePath) {
      File.WriteAllText(filePath, string.Join("\n", agent.Chromosome.ToArray()));
    }

    // Del af GA library??
    public void SavePopulation(Population population, string filePath) {

      using(FileStream filestreamWriter = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(filestreamWriter, population);
        filestreamWriter.Close();
      }
    }



    /*
    public void LoadPopulation(string filePath) {

      using(FileStream filestreamWriter = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(filestreamWriter, population);
        filestreamWriter.Close();
      }
    }
    */

    private void ChangeSettings() {
      // Prompt for settings. Lav ny genetic med settings og nuværende pop således ej randomizer
      throw new NotImplementedException();
    }

    private void LoadAgentGUI(string directoryPath, int geneCount) {
      bool fileFound = false;
      string input;
      string[] chromosomeString = new string[0];

      // Get names of files in default directory
      string[] filesInDirectory = Directory.GetFiles(directoryPath);
      // Prine file names
      Console.WriteLine($"Files in directory {directoryPath}:\n\n");
      foreach(var fileName in filesInDirectory) {
        Console.WriteLine(fileName);
      }

      // Get user file name input and check if file exist
      do {
        Console.WriteLine("\nEnter file name:");
        Console.Write($"{directoryPath}");
        input = Console.ReadLine();
        // Check if in directory
        if(File.Exists(directoryPath + input)) {
          // File Found. Read genes from file
          chromosomeString = File.ReadAllLines(directoryPath + input);
          // Check if genes equalt weights in NN
          if(chromosomeString.Length == geneCount) {
            fileFound = true;
          }
          else {
            Console.WriteLine("\n!!!No. of genes not same as in current NN! Choose another agent-file!!!");
          }
        }
        else {
          Console.WriteLine("\nFILE NOT FOUND - TRY AGAIN!");
        }
      } while(!fileFound);

      // Make new gene
      Gene[] genes = new Gene[chromosomeString.Length];
      // Fill gene with doubles from converted strings
      for(int i = 0; i < chromosomeString.Length; i++) {
        genes[i] = new Gene(Convert.ToDouble(chromosomeString[i]));
      }
      Chromosome chromosomeCopy = new Chromosome(genes);
      Agent loadedAgent = new Agent(chromosomeCopy);

      Console.WriteLine("\n\nAGENT LOADED SUCCESFULLY!");
      Console.WriteLine("Press any key to show play of loaded agent...");
      Console.ReadKey();

      replayer.ShowLiveReplay(loadedAgent);
    }
  }
}
