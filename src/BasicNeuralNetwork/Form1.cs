﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace BasicNeuralNetwork {
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
            g.BuildConnection(2, 9, 1, true, 11);

            DrawGenome(forceGraphVisualizer1, g);

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
