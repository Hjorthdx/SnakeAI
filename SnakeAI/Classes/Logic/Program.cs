using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithmNS;
using NeuralNetworkNS;
using SnakeGameNS;

namespace SnakeAI {
  static class Program {

    // Lock to prevent threads accessing console at same time
    public static readonly object ConsoleWriteLineLock = new object(); 

    [STAThread]
    static void Main(string[] args) {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);


      // Set up settings
      NetworkSettings networkSettings = MakeNetworkSettings();
      SnakeSettings snakeSettings = MakeSnakeSettings();
      GeneticSettings geneticSettings = MakeGeneticSettings(networkSettings, snakeSettings);

      SettingsGUI settingsGUI = new SettingsGUI(geneticSettings, networkSettings, snakeSettings);
      Application.Run(settingsGUI);

      // Set up genetic algorithm
      GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(geneticSettings); 

      Menu menu = new Menu(geneticAlgorithm, geneticSettings, snakeSettings, networkSettings);

      // Print start screen console // Skal slettes når vi får lavet en winform menu
      Print.GeneticSettings(geneticSettings);
      menu.WelcomeScreen();
      
      // Set up and start threads
      KeyPressListener keyPressListener = new KeyPressListener();
      keyPressListener.StartThread(); 
      AgentCounter agentCounter = new AgentCounter();
      agentCounter.StartThread();

      // Start genetic GUI
      geneticAlgorithm.ShowGUI();

      // Start genetic algorithm
      do {
        geneticAlgorithm.RunTime.Start();
        geneticAlgorithm.CalculateFitness(); 
        // Show menu if key pressed.
        if(keyPressListener.keyPressed) {
          geneticAlgorithm.RunTime.Stop();
          agentCounter.Pause();
          menu.Show();
          // Resume threads
          keyPressListener.Resume();
          agentCounter.Resume();
          geneticAlgorithm.RunTime.Start();
        }
        if(ProgramSettings.SAVE_WEIGHTS_EVERY_GENERATION) {
          menu.SaveWeightsOfBestAgent(geneticAlgorithm.TopKAgents.Best.Agent, ProgramSettings.FILE_NAME_SAVE_BEST_AGENT);
        }
        geneticAlgorithm.MakeSelection();
        // Update convergence
        if(geneticAlgorithm.GenerationCount % 10 == 0) {
          geneticAlgorithm.UpdateConvergencePercent();
        }
        agentCounter.Pause();
        lock(ConsoleWriteLineLock) {
          Print.Status(geneticAlgorithm);
        }
        agentCounter.Resume(); 
        geneticAlgorithm.MakeCrossovers();
        geneticAlgorithm.MakeMutations();
      } while(true); // Lav converge tester her. 

      // END OF MAIN
    }

    private static GeneticSettings MakeGeneticSettings(NetworkSettings networkSettings, SnakeSettings snakeSettings) {

      GeneticSettings geneticSettings = new GeneticSettings(ProgramSettings.POPULATION_SIZE,
                                                            networkSettings.numberOfWeights,
                                                            ProgramSettings.SELECTION_SIZE,
                                                            ProgramSettings.CHILD_AGENTS_IN_POPULATION_PERCENT,
                                                            ProgramSettings.CROSSOVER_PROBABILTY_AGENT,
                                                            ProgramSettings.MUTATION_PROBABALITY_AGENT,
                                                            ProgramSettings.MUTATION_PROBABILITY_GENE,
                                                            new RandomNumberGenerator(),
                                                            new FitnessCalculator(networkSettings, snakeSettings),
                                                            new RandomResettingMutator(),
                                                            new TopPerformersSelector(),
                                                            ProgramSettings.CROSSOVER_METHOD,
                                                            ProgramSettings.TOP_K_AGENTS_COUNT);
      return geneticSettings;
    }

    private static NetworkSettings MakeNetworkSettings() {
      NetworkSettings networkSettings = new NetworkSettings(ProgramSettings.NUMBER_OF_INPUT_NEURONS,
                                                            ProgramSettings.HIDDEN_LAYER_STRUCTURE,
                                                            ProgramSettings.OUTPUT_LABELS);
      return networkSettings;
    }

    private static SnakeSettings MakeSnakeSettings() {
      SnakeSettings snakeSettings = new SnakeSettings(ProgramSettings.GRID_ROWS, ProgramSettings.GRID_COLUMNS, ProgramSettings.SIDE_LENGTH,
                                                      ProgramSettings.POINTS_PER_FOOD_EATEN, new Random());
      return snakeSettings;
    }

    public static int GetIntFromConsoleChar() {
      int numberInt = -1;
      bool isNumber = false;
      string userInput;

      while(!isNumber) {
        userInput = Console.ReadKey().KeyChar.ToString();
        isNumber = Int32.TryParse(userInput, out numberInt);
      }
      return numberInt;
    }

    public static int GetIntFromConsoleString() {
      int numberInt = -1;
      bool succes = false;
      string userInput;

      while(!succes) {
        userInput = Console.ReadLine();
        succes = Int32.TryParse(userInput, out numberInt);
      }
      return numberInt;
    }



  }
}
