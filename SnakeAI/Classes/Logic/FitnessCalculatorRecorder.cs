using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGameNS;
using GeneticAlgorithmNS;
using NeuralNetworkNS;

namespace SnakeAI {
  /// <summary>
  /// Stores information about a fitness calculation. 
  /// </summary>
  [Serializable]
  public class FitnessCalculatorRecorder {
    // ændres til camelcase
    public List<FitnessRoundInfo> FitnessRoundInfoList { get; set; }
    public readonly Agent Agent;
    public readonly EndGameInfo originalEndGameInfo;

    public readonly NetworkSettings NetworkSettings; 
    public readonly SnakeSettings SnakeSettings;

    // The end game info for this recording. 
    public EndGameInfo newEndGameInfo { get; private set; }

    public FitnessCalculatorRecorder(Agent agent, NetworkSettings networkSettings, SnakeSettings snakeSettings) {
      Agent = agent;
      SnakeSettings = snakeSettings;
      NetworkSettings = networkSettings;
      originalEndGameInfo  = agent.FitnessInfo as EndGameInfo;
      FitnessRoundInfoList = new List<FitnessRoundInfo>();
    }

    public void TakeSnapShot(FitnessRoundInfo fitnessRoundInfo) {
      FitnessRoundInfoList.Add(fitnessRoundInfo);
    }

    public void TakeSnapShot(EndGameInfo newEndGameInfo) {
      this.newEndGameInfo = newEndGameInfo;
    }
  }
}
