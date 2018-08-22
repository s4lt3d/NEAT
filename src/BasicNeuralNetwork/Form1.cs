using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicNeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Genome g = new Genome();
            g.BuildNode(NodeGene.NodeType.INPUT_NODE, 1);
            g.BuildNode(NodeGene.NodeType.INPUT_NODE, 2);
            g.BuildNode(NodeGene.NodeType.OUTPUT_NODE, 3);
            g.BuildConnection(1, 3, 1, true, 4);
            g.BuildConnection(2, 3, 1, true, 5);
            g.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 6);
            g.BuildConnection(6, 3, 1, true, 7);
            g.BuildConnection(1, 6, 1, true, 8);
            g.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 9);
            g.BuildConnection(6, 9, 1, true, 10);
            g.BuildConnection(1, 9, 1, true, 11);


        }
    }
}
