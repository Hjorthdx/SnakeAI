using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace GeneticAlgorithmNS {
  public class GeneticSettingsListGUI : ListView {

    private readonly GeneticSettings geneticSettings;
    private Font boldFont;

    public GeneticSettingsListGUI(GeneticSettings geneticSettings) {
      this.geneticSettings = geneticSettings;

      ColumnHeader setting = new ColumnHeader();
      ColumnHeader value = new ColumnHeader();

      setting.Width = 220;
      value.Width = 200;

      BorderStyle = BorderStyle.None;
      HeaderStyle = ColumnHeaderStyle.None;
      Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
      boldFont = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

      Columns.AddRange(new ColumnHeader[] {
            setting,
            value});

      Location = new Point(22, 14);
      Size = new Size(306, 249);
      Name = "SettingsList";

      Activation = ItemActivation.OneClick;
      Scrollable = false;
      ShowItemToolTips = true;

      View = View.Details;

      CreateItems();
    }

    private void CreateItems() {
      // FLYT TOOLTIPS STRINGS IND I GENETIC SETTINGS SÅ DE KAN GENBRUGES

      ListViewItem populationSize = new ListViewItem("Population size");
      populationSize.SubItems.Add(": " + geneticSettings.PopulationSize.ToString());
      populationSize.ToolTipText = "The size of the population in each generation.";

      ListViewItem selectionSize = new ListViewItem("Selection size");
      selectionSize.SubItems.Add(": " + geneticSettings.SelectionSize.ToString());
      selectionSize.ToolTipText = "The number of agents chosen to be parents.";

      ListViewItem geneMutation = new ListViewItem("Gene mutation chance");
      geneMutation.SubItems.Add(": " + (geneticSettings.MutationProbabilityGenes * 100).ToString() + "%");
      geneMutation.ToolTipText = "The chance for any single gene to be mutated.";

      ListViewItem agentMutation = new ListViewItem("Agent mutation chance");
      agentMutation.SubItems.Add(": " + (geneticSettings.MutationProbabiltyAgents * 100).ToString() + "%");
      agentMutation.ToolTipText = "The chance for any single agent to be mutated.";

      ListViewItem chromosomeSize = new ListViewItem("Chromesome size");
      chromosomeSize.SubItems.Add(": " + geneticSettings.GeneCount.ToString());
      chromosomeSize.ToolTipText = "The number of genes in an agent's chromosome.";

      ListViewItem selectionMethod = new ListViewItem("Selection method");
      selectionMethod.SubItems.Add(": " + geneticSettings.Selector.Title);
      selectionMethod.ToolTipText = "The chosen method for selection."; // BESKRIV SPECIFIK METHOD

      ListViewItem crossOverMethod = new ListViewItem("Crossover method");
      crossOverMethod.SubItems.Add(": " + geneticSettings.Crossover.Title);
      crossOverMethod.ToolTipText = "The chosen method for crossover."; // BESKRIV SPECIFIK METHOD

      Items.Add(populationSize);
      Items.Add(selectionSize);
      Items.Add(geneMutation);
      Items.Add(agentMutation);
      Items.Add(chromosomeSize);
      Items.Add(selectionMethod);
      Items.Add(crossOverMethod);
    }
  }
}
