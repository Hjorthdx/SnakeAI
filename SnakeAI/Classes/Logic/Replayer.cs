using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkNS;
using SnakeGameNS;
using GeneticAlgorithmNS;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SnakeAI {
  /// <summary>
  /// Shows GUI replays of fitness calculations. 
  /// Can be used to show replays of all agents using the settings specified at instantiation. 
  /// </summary>
  class Replayer {
    //Replay info udover det vedr. snakegame, skal vises i sekundært vindue!
    private SnakeSettings snakeSettings;
    private NetworkSettings networkSettings;

    public Replayer(SnakeSettings snakeSettings, NetworkSettings networkSettings) {
      this.snakeSettings = snakeSettings;
      this.networkSettings = networkSettings;
    }

    // Show replay when info saved in file // Skal implementeres. Indlæser csv fil.
    public void ShowFileReplay(string directoryPath) {
      throw new NotImplementedException();
    }

    // Show live replay of agent
    public void ShowLiveReplay(Agent agentToShow) {
      ConsoleKey choice;
      FitnessCalculator fitnessCalculator = new FitnessCalculator(networkSettings, snakeSettings);
      List<FitnessCalculatorRecorder> recorderList = new List<FitnessCalculatorRecorder>();
      string headLine = $"\n\nSHOWING LIVE REPLAY OF AGENT { agentToShow.UniqueID} | GEN { agentToShow.GenerationNumber} | ORIGINAL FITNESS { agentToShow.Fitness}\n\n";

      //Recording = AskForReplayToChoose();
  
      Console.WriteLine(headLine);
        Console.WriteLine("Press any key to record a live fitness calculation!");
        Console.ReadKey();
      do {
        Console.Clear();
        Console.WriteLine(headLine);
        recorderList.Add(fitnessCalculator.RecordCalculation(agentToShow));


        //using(FileStream filestreamWriter = new FileStream(ProgramSettings.FILE_NAME_SAVE_BEST_AGENT, FileMode.Create, FileAccess.Write)) {
        //  BinaryFormatter binaryFormatter = new BinaryFormatter();
        //  binaryFormatter.Serialize(filestreamWriter, recorderList[0]);
        //  filestreamWriter.Close();
        //}

        recorderList.Sort((a, b) => -a.newEndGameInfo.Fitness.CompareTo(b.newEndGameInfo.Fitness));
        for(int i = 0; i < recorderList.Count; i++) {
          Console.WriteLine($"{$"RESULTS [{i}]:",-13} {$" --> Fitness = {recorderList[i].newEndGameInfo.Fitness:N2}",-25} " +
                            $"{$"Avg. steps pr. food -> {recorderList[i].newEndGameInfo.averageMovesPerFood,8:N2}",-20}");
        }
        Console.WriteLine($"\n(1) Show replay of [yourChoice]");
        Console.Write($"\nPress ENTER to make new replay");
        choice = Console.ReadKey().Key;
      } while(choice != ConsoleKey.D1);

      Console.Write("\nEnter replay number: ");
      int choiche = Program.GetIntFromConsoleString(); // Skal ikke ligge i program

      ShowPlay(recorderList[choiche], agentToShow);
    }

    private void ShowPlay(FitnessCalculatorRecorder recorder, Agent agentToShow) {

      List<FitnessRoundInfo> fitnessRoundInfoList = recorder.FitnessRoundInfoList;

      // Get start grid. // Recorder.GetInitialGameInfo(); 
      Grid grid = fitnessRoundInfoList.First().grid;
      int score = fitnessRoundInfoList.First().score;
      bool isAlive = fitnessRoundInfoList.First().isAlive;

      SnakeGameGUI snakeGameGUI = new SnakeGameGUI(snakeSettings, grid, score, isAlive);
      int snakeSpeedMS = AskConsoleForSnakeSpeed();

      // START GUI
      snakeGameGUI.OpenGameWindow(); 

      // ShowStartScreen();
      Console.Clear();
      ShowConsolePlayInfo(agentToShow, networkSettings, fitnessRoundInfoList.First(), recorder.originalEndGameInfo);
      Console.WriteLine("All set, press enter");
      Console.ReadKey();
      Console.Clear();

      // Loop throug frames // ShowPlay();
      for(int i = 1; i < fitnessRoundInfoList.Count; i++) {
        snakeGameGUI.UpdateView(fitnessRoundInfoList[i].grid, fitnessRoundInfoList[i].score, fitnessRoundInfoList[i].isAlive, fitnessRoundInfoList[i].SnakeHeadPoint);
        Console.Clear();
        ShowConsolePlayInfo(agentToShow, networkSettings, fitnessRoundInfoList[i], recorder.originalEndGameInfo);
        Thread.Sleep(snakeSpeedMS);
      }
      ShowNewEndGameInfo(recorder.newEndGameInfo);

      Console.WriteLine("\nREPLAY DONE, press enter to go back to calculations");
      Console.ReadKey();
    }

    private void ShowNewEndGameInfo(EndGameInfo newEndGameInfo) {
      Console.WriteLine("\n\nEND GAME INFO: ");
      Console.WriteLine($"Fitness -> {newEndGameInfo.Fitness}");
      Console.WriteLine($"Food eaten -> {newEndGameInfo.foodEaten}");
      Console.WriteLine($"Avg. moves pr. food -> {newEndGameInfo.averageMovesPerFood}");
      Console.WriteLine($"Moves total -> {newEndGameInfo.MovesTotal}");
      Console.WriteLine($"Snake game score -> {newEndGameInfo.Score}");
      Console.WriteLine($"Cause of death -> {newEndGameInfo.SnakeCauseOfDeath.ToString()}");
    }

    // CONSOLE PRINT methods - skal erstattes af gui winform!!
    public int AskConsoleForSnakeSpeed() {
      int snakeSpeedMs;
      Console.Write("Enter snake speed (ms.): ");
      snakeSpeedMs = Program.GetIntFromConsoleString(); // I helper statiske helper methods?
      return snakeSpeedMs;
    }

    public static void ShowConsolePlayInfo(Agent agentToShow, NetworkSettings networkSettings, FitnessRoundInfo fitnessRoundInfo,
                                           EndGameInfo originalEndGameInfo) {

      Agent a = agentToShow;
      NetworkSettings n = networkSettings;
      FitnessRoundInfo f = fitnessRoundInfo;
      Console.WriteLine($"\n\nSHOWING REPLAY OF AGENT {a.UniqueID} | GEN {a.GenerationNumber} | FITNESS {a.Fitness} | WEIGHTS IN NN: {n.numberOfWeights}\n\n");
      Console.WriteLine("Moves since last point:" + f.movesSincePoint);
      Console.WriteLine("Total moves:" + f.totalMoves);
      Console.WriteLine("Determination reward: " + f.determinationReward);

      int counter = 0;
      // SHOW INPUT
      Console.WriteLine("\nINPUT VALUES");
      Dictionary<string, double> inputValues = GetInputValues(f.inputNeuralNetwork);
      foreach(KeyValuePair<string, double> item in inputValues) {
        Console.WriteLine($"{item.Key} --> {item.Value:N2}");
        counter++;
        if(counter % 4 == 0) {
          Console.WriteLine();
        }
      }

      // SHOW OUTPUT
      Console.WriteLine($"\nCurrent snake action ---> {f.currentDirection.ToString()}");
      Console.WriteLine("\nOUTPUT VALUES");
      Dictionary<int, double> outputvalues = f.outputNeuralNetwork;
      foreach(KeyValuePair<int, double> item in outputvalues) {
        Console.WriteLine($"{(Direction)item.Key} --> {item.Value:N2}");
      }
    }

    // Skal tilpasses så mere generel... ? 
    public static Dictionary<string, double> GetInputValues(double[] input) { // eller blot lav til list af strenge???
      Dictionary<string, double> inputValues = new Dictionary<string, double>();

      for(int i = 0, directionCounter = 1; i < input.Length; i++, directionCounter++) {
        if(i < 4) {
          inputValues.Add($"WALL IS {(Direction)directionCounter}", input[i]); // Direction enum er fra snake
        }
        else if(i < 8) {
          inputValues.Add($"BODY IS {(Direction)directionCounter}", input[i]);
        }
        else if(i < 12) {
          inputValues.Add($"FOOD IS {(Direction)directionCounter}", input[i]);
        }
        else if(i < 16) {
          inputValues.Add($"BODY IS IN DIRECTION {(Direction)directionCounter}", input[i]);
        }
        if(directionCounter % 4 == 0) {
          directionCounter = 0;
        }
      }
      return inputValues;
    }
  }
}
