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
            Genome parent1 = new Genome();
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE,  1));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE,  2));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.INPUT_NODE,  3));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.OUTPUT_NODE, 4));
            parent1.BuildNodeGenesFromCrossover(new NodeGene(NodeGene.NodeType.HIDDEN_NODE, 5));

            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 4, 1, false, 2));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(3, 4, 1, true,  3));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(2, 5, 1, true,  4));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(5, 4, 1, true,  5));
            parent1.BuildConnectionGenesFromCrossover(new ConnectionGene(1, 5, 1, true,  8));

            textBox1.Text =  parent1.ToString();

            Genome parent2 = new Genome();

        }
    }
}
