using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Windows.Forms.Timer;

namespace GeneticAlgorithmNS {
  public class GeneticGUI : Form {

    private readonly GeneticAlgorithm geneticAlgorithm;
    private readonly GeneticSettings geneticSettings;

    private Timer timer;

    private Label generationCounter;
    private Label currentBestFitness;
    private Label elapsedTime;
    private Label avgTopTenFitness;

    private FitnessGraph fitnessGraph;
    private double currentBest;
    private int lastGen;

    private Button pauseButton;
    private Button playBestButton;
    private Button saveBestAgentButton;

    private Font fontLarge;
    private Font fontSmall;
    private Font fontButton;

    public GeneticGUI(GeneticAlgorithm geneticAlgorithm, GeneticSettings geneticSettings) {
      this.geneticAlgorithm = geneticAlgorithm;
      this.geneticSettings = geneticSettings;

      fontLarge = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
      fontSmall = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
      fontButton = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
      
      InitializeComponent();

      timer = new Timer();
      timer.Start();
      timer.Tick += UpdateGraph;
    }

    private void UpdateGraph(Object sender, EventArgs e) {
      elapsedTime.Text = $"Time elapsed: {geneticAlgorithm.RunTime.Elapsed.ToString(@"hh\:mm\:ss")}--";

      if (geneticAlgorithm.TopKAgents.Best != null) {
        currentBest = geneticAlgorithm.TopKAgents.Best.Agent.Fitness;
      }

      if (geneticAlgorithm.GenerationCount != lastGen) {

        generationCounter.Text = $"Current generation: {geneticAlgorithm.GenerationCount}";
        currentBestFitness.Text = $"Current best fitness: {currentBest}";
        avgTopTenFitness.Text = $"Average top 10 fitness: {geneticAlgorithm.TopKAgents.AverageFitness}";
        
        fitnessGraph.UpdateGraph(0, geneticAlgorithm.GenerationCount, geneticAlgorithm.TopKAgents.AverageFitness);
        fitnessGraph.UpdateGraph(1, geneticAlgorithm.GenerationCount, currentBest);
      }
      lastGen = geneticAlgorithm.GenerationCount;
    }   

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneticGUI));
      Icon = GeneticAlgorithmNS.Properties.Ressources.genetic_icon;

      fitnessGraph = new FitnessGraph(new Point(12, 319), new Size(795, 255));
      ((System.ComponentModel.ISupportInitialize)(this.fitnessGraph)).BeginInit(); // Dunno what this does
      
      Panel settingsPanel = CreateSettingsPanel();
      Panel generationPanel = CreateGenerationsPanel();
      Panel agentPanel = CreateAgentsPanel();

      Controls.Add(generationPanel);
      Controls.Add(agentPanel);
      Controls.Add(settingsPanel);
      Controls.Add(fitnessGraph);

      MenuStrip menu = CreateMenu();
      Controls.Add(menu);

      ClientSize = new Size(819, 586);

      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;

      Text = "Genetic Algorithm";      
     
      PerformLayout(); // Not sure why

    }

    private GeneticPanelGUI CreateGenerationsPanel() {
      GeneticPanelGUI generationPanel = new GeneticPanelGUI(new Point(12, 35), new Size(380, 136));

      generationCounter = new GeneticLabelGUI(new Point(3, 14), new Size(296, 31), fontLarge);
      generationPanel.Controls.Add(generationCounter);

      elapsedTime = new GeneticLabelGUI(new Point(5, 54), new Size(200, 24), fontSmall);
      generationPanel.Controls.Add(elapsedTime);

      EventHandler pauseHandler = new EventHandler(PauseAlgorithmClick);
      pauseButton = new GeneticButtonGUI(new Point(9, 90), new Size(352, 31), fontButton, pauseHandler);
      pauseButton.Text = "Pause algorithm";
      generationPanel.Controls.Add(pauseButton);

      return generationPanel;
    }

    private GeneticPanelGUI CreateAgentsPanel() {
      GeneticPanelGUI agentPanel = new GeneticPanelGUI(new Point(12, 177), new Size(380, 136));

      currentBestFitness = new GeneticLabelGUI(new Point(3, 14), new Size(266, 31), fontLarge);
      agentPanel.Controls.Add(currentBestFitness);

      avgTopTenFitness = new GeneticLabelGUI(new Point(5, 54), new Size(204, 24), fontSmall);
      agentPanel.Controls.Add(avgTopTenFitness);

      EventHandler playHandler = new EventHandler(PlayBestClick);
      playBestButton = new GeneticButtonGUI(new Point(9, 90), new Size(148, 31), fontButton, playHandler);
      playBestButton.Text = "Play best agent";
      agentPanel.Controls.Add(playBestButton);

      EventHandler saveHandler = new EventHandler(SaveBestClick);
      saveBestAgentButton = new GeneticButtonGUI(new Point(217, 90), new Size(148, 31), fontButton, saveHandler);
      saveBestAgentButton.Text = "Save best agent";
      agentPanel.Controls.Add(saveBestAgentButton);

      return agentPanel;
    }

    private GeneticPanelGUI CreateSettingsPanel() {
      GeneticPanelGUI panel = new GeneticPanelGUI(new Point(456, 35), new Size(351, 278));

      GeneticSettingsListGUI settingsList = new GeneticSettingsListGUI(geneticSettings);
           
      panel.Controls.Add(settingsList);

      return panel;
    }

    private MenuStrip CreateMenu() {
      MenuStrip menu = new MenuStrip();

      ToolStripMenuItem exitButton = new ToolStripMenuItem();
      exitButton.Size = new Size(137, 20);
      exitButton.Text = "Exit Genetic Algorithm";
      exitButton.Click += new EventHandler(menuItemExit_Click);
      
      menu.Items.AddRange(new ToolStripItem[] {
            exitButton});

      menu.Location = new Point(0, 0);
      menu.Size = new Size(819, 24);

      return menu;
    }

    // SKAL KUNNE STOPPE ALGO HER
    private void menuItemExit_Click(object sender, EventArgs e) {
      DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit? Unsaved agents will be lost.", "WARNING", MessageBoxButtons.YesNo);
      if (dialogResult == DialogResult.Yes) {
        Close();
      }
    }

    private void PauseAlgorithmClick(object sender, EventArgs e) {
      MessageBox.Show("NOT YET IMPLEMENTED", "Error");
    }

    private void PlayBestClick(object sender, EventArgs e) {
      MessageBox.Show("NOT YET IMPLEMENTED", "Error");
    }

    private void SaveBestClick(object sender, EventArgs e) {
      MessageBox.Show("NOT YET IMPLEMENTED", "Error");
    }
  }
}
