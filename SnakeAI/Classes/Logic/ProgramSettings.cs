using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmNS;

namespace SnakeAI {
  // Constants used to instantiate the settings classes of the the 3 objets: NN, Snake, Genetic. 
  // This class should never be accessed inside the 3 objects!
  public static class ProgramSettings {

    // GENERAL SETTINGS //
    public const double PRINT_INTERVAL_SECONDS = 15; // Print frequency of avg. fitness in top 10 agents
    public const bool SAVE_WEIGHTS_EVERY_GENERATION = false; // Will save weights in file for every gen. Disable if not training
    //public const string DIRECTORY_PATH = @"SavedAgents\"; //To save/load agents. Placed in default folder NN->Main->bin->Debug
    public const string DIRECTORY_PATH = @"../../SavedAgents/"; //To save/load agents. Placed in default folder NN->Main->bin->Debug
    public static readonly string FILE_NAME_SAVE_BEST_AGENT;  // File path to save best agent. Set in constructor based on provided dir path

    static ProgramSettings() {
      // Get file name for this run
      int fileCount = Directory.GetFiles(DIRECTORY_PATH, "*.*", SearchOption.AllDirectories).Length;
      FILE_NAME_SAVE_BEST_AGENT = $"{DIRECTORY_PATH}BestAgent_RUN00{fileCount + 1}_STARTED_{DateTime.Now.ToString("HH-mm-ss")}.txt";
    }

    // GENETIC SETTINGS //
    public const int POPULATION_SIZE = 1000;                                                    // 1000
    public const int SELECTION_SIZE  = 50;                                                       // 50
    public const int CROSSOVER_PROBABILTY_AGENT = 100; // not impl.
    public const double MUTATION_PROBABALITY_AGENT = 0.9; // Percent                        // Best 0.9
    public const double MUTATION_PROBABILITY_GENE  = 0.03; // Percent                       // Best 0.03
    public const int TOP_K_AGENTS_COUNT = 10; // Choose how many top agents the algorithm should keep track of.

    public static readonly ICrossover CROSSOVER_METHOD = new CrossoverMethods().OnePointCombinePlusElitismCrossover;  


    public const CrossoverMethod SELECTION_METHOD = CrossoverMethod.Combine; // Not impl.
    public const MutationMethod MUTATION_METHOD = MutationMethod.Mutate;
    //public static readonly IMutator MUTATION_METHOD2 = new RandomResettingMutator;
    public const bool KEEP_PARENTS = false; // Will make room for and add parent agents in new population // NOT IMPL
    public const bool KEEP_TOP_TEN = false; // Will make room for and add all time top ten agents in new population // NOT IMPL
    public const bool KEEP_BEST = false; // Adds only the all time best agent in new population // NOT IMPL
    public const double CHILD_AGENTS_IN_POPULATION_PERCENT = 1; // NOT IMPL.
    public const double RANDOM_AGENTS_IN_POPULATION_PERCENT = 0; // NOT IMPL
    public const double PARENT_AGENTS_IN_POPULATION_PERCENT = 0; // NOT IMPL


    // CALC FITNESS METHOD //
    public const int MAX_STEPS      = 30; // How many steps before 
    public const int SNAKE_SPEED_MS = 0; // NOT IMPL
    public const int INPUT_METHOD   = 1; // NOT IMPL

    // NN SETTINGS
    public static readonly Random RandomNumber = new Random(); // Lav egen så unit test muligt // lav om klasse og skriv i doku
    public static readonly int NUMBER_OF_INPUT_NEURONS    = 16;
    public static readonly int[] HIDDEN_LAYER_STRUCTURE = { 10, 5 };
    public static readonly int[] OUTPUT_LABELS = { 1, 2, 3, 4 };

    // Number of output neurons = OutputLabels.Lenght?
    // Number of hidden neurons = HiddenLayerStructure.Lenght?

    // SNAKE GAME SETTINGS 
    public const int GRID_ROWS           = 20;
    public const int GRID_COLUMNS        = 30;
    public const int SIDE_LENGTH        = 20;
    public const int POINTS_PER_FOOD_EATEN = 1;

    /*

    // GENERAL CONSTANTS
    // Run regular snake game or run NN
    public const bool TEST_SNAKE = false;
    public const bool TEST_NN = false;
    // State of each snake field (input values)
    public const int STATE_SNAKE_HEAD = 100000;
    public const int STATE_SNAKE_PART = 3;
    public const int STATE_FOOD       = 2;
    public const int STATE_GRID_FIELD = 1;
    public const int STATE_WALL       = -2;

    public const int IS_WALL_UP = 0;
    public const int IS_WALL_RIGHT = 1;
    public const int IS_WALL_DOWN = 2;
    public const int IS_WALL_LEFT = 3;

    // NN CONSTANTS
    // Used for initializing random weights for NN testing purposes only. 
    public static readonly Random RandomNumber = new Random();

    //public static readonly int NUMBER_OF_INPUT_NEURONS = GRID_COLUMNS * GRID_ROWS;
    public static readonly int NUMBER_OF_INPUT_NEURONS = 8;

    public static readonly int[] NUMBER_OF_HIDDEN_NEURONS = { 6, 6 };
    public static readonly int[] NUMBER_OF_OUTPUT_NEURONS = { 1, 2, 3, 4 };

    // SNAKE GAME CONSTANTS
    public const int GRID_ROWS = 20;
    public const int GRID_COLUMNS = 30;
    public const bool SPAWN_RANDOM_SNAKE = false;
  }
  */
  }
}