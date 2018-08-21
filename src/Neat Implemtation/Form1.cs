using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neat_Implemtation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Genome parent1 = new Genome(8);
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 1));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 2));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 3));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.OUTPUT_NODE, 4));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.HIDDEN_NODE, 5));

            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 4, 1, false, 2));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(3, 4, 1, true, 3));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(2, 5, 1, true, 4));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(5, 4, 1, true, 5));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 5, 1, true, 8));
            parent1.Sort();
            textBox1.Text = parent1.ToString();

            Genome parent2 = new Genome(10);

            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 1));
            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 2));
            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE, 3));
            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.OUTPUT_NODE, 4));
            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.HIDDEN_NODE, 5));
            parent2.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.HIDDEN_NODE, 6));

            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 4, 2, true, 1));

            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(2, 4, 2, false, 2));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(3, 4, 2, true, 3));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(2, 5, 2, true, 4));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(5, 4, 2, false, 5));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(5, 6, 2, true, 6));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(6, 4, 2, true, 7));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(3, 5, 2, true, 9));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 6, 2, true, 10));
            parent2.BuildConnectionGenesFromCrossover(new ConnectionGene(5, 5, 2, true, 11));

            textBox2.Text = parent2.ToString();
            parent2.Sort();
            List<double> inputs = new List<double>(3);
            inputs.Add((double)numericUpDown1.Value);
            inputs.Add((double)numericUpDown2.Value);
            inputs.Add((double)numericUpDown3.Value);

            double[] o = parent2.Evaluate(inputs);
            textBoxOutput.Text = o[0].ToString();
            Genome offspring = parent2.crossover(parent1);
            

            textBox3.Text = offspring.ToString();

        }
    }
}
