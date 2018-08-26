using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace NEATNeuralNetwork {
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void MakeWebBrowserUseIE9() {
            string appName = "";
            try {
                appName = Path.GetFileName(Assembly.GetEntryAssembly().Location);

                const string IE_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

                using (var fbeKey = Registry.CurrentUser.OpenSubKey(IE_EMULATION, true)) {

                    fbeKey.SetValue(appName, 9000, RegistryValueKind.DWord);

                }
            }

            catch (Exception ex) {
                MessageBox.Show(appName + "\n" + ex.ToString(), "Unexpected error setting browser mode!");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MakeWebBrowserUseIE9();
            Genome p1 = new Genome();
            p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 1);
            p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 2);
            p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 3);
            p1.BuildNode(NodeGene.NodeType.OUTPUT_NODE, 4);
            p1.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 5);
            p1.BuildConnection(1, 4, 1, true);
            p1.BuildConnection(1, 5, 1, true);
            p1.BuildConnection(3, 4, 1, true);
            p1.BuildConnection(2, 5, 1, true);
            p1.BuildConnection(5, 4, 1, true);

            Genome p2 = new Genome();
            p2.BuildNode(NodeGene.NodeType.INPUT_NODE, 1);
            p2.BuildNode(NodeGene.NodeType.INPUT_NODE, 2);
            p2.BuildNode(NodeGene.NodeType.INPUT_NODE, 3);
            p2.BuildNode(NodeGene.NodeType.OUTPUT_NODE, 4);
            p2.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 5);
            p2.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 6);

            p2.BuildConnection(1, 4, 1, true);
            p2.BuildConnection(1, 6, 1, true);
            p2.BuildConnection(6, 4, 1, true);
            p2.BuildConnection(2, 5, 1, true);
            p2.BuildConnection(3, 5, 1, true);
            p2.BuildConnection(5, 6, 1, true);
            p2.BuildConnection(3, 4, 1, true);

            Genome child = p1.Crossover(p2);



            DrawGenome(forceGraphVisualizer1, p1);
            DrawGenome(forceGraphVisualizer2, p2);
            DrawGenome(forceGraphVisualizer3, child);

        }

        private void DrawGenome(ForceDirected.ForceGraphVisualizer graph, Genome genome) {
            graph.ClearGraph();
            foreach(KeyValuePair<int, NodeGene> node in genome.Nodes) {
                graph.AddNode(node.Value.Innovation, Color.White);
            }

            foreach (KeyValuePair<int, ConnectionGene> connection in genome.Connections) {
                graph.AddEdge(connection.Value.InNode, connection.Value.OutNode);
            }
        }
    }
}
