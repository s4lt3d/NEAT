using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NEATNeuralNetwork;

namespace NEATNeuralNetwork {
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        Genus pool = new Genus(2, 1);

        Random r = new Random();
        private void Form1_Load(object sender, EventArgs e)
        {

            Genus pool = new Genus(2, 1);
            




            //Genome p1 = new Genome();
            //p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 1);
            //p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 2);
            //p1.BuildNode(NodeGene.NodeType.INPUT_NODE, 3);
            //p1.BuildNode(NodeGene.NodeType.OUTPUT_NODE, 4);
            //p1.BuildNode(NodeGene.NodeType.HIDDEN_NODE, 5);
            //p1.BuildConnection(1, 4, 1, true);
            //p1.BuildConnection(1, 5, 1, true);
            //p1.BuildConnection(3, 4, 1, true);
            //p1.BuildConnection(2, 5, 1, true);
            //p1.BuildConnection(5, 4, 1, true);

            //Genome p2 = p1.Crossover(p1);

            //p2.Mutate();

            //Genome child = p1.Crossover(p2);

            //DrawGenome(forceGraphVisualizer1, p1);
            //DrawGenome(forceGraphVisualizer2, p2);
            //DrawGenome(forceGraphVisualizer3, child);

            //Species s = new Species();

            //s.AddGenome(p1);
            //s.AddGenome(p2);
            //s.AddGenome(child);


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

        private void button1_Click(object sender, EventArgs e) {

            for (int j = 0; j < 100; j++) {
                pool.NewGeneration();
                foreach (Genome g in pool.Genomes()) {
                    double averageScore = 0;
                    for (int i = 0; i < 2000; i++) {
                        int a = r.Next(0, 2);
                        int b = r.Next(0, 2);
                        float c = a ^ b;

                        double[] outputs = g.Evaluate(new double[] { a, b });

                        averageScore += Math.Abs(c - outputs[0]);

                    }
                    if (averageScore == 0) // perfect score
                        averageScore = double.MaxValue;
                    else
                        averageScore = 1 / averageScore;

                    g.Fitness = averageScore;
                }
            }

            Genome best = pool.BestGenome();
            textBoxFitness.Text = best.Fitness.ToString();
            DrawGenome(forceGraphVisualizer1, best);

        }

        private void textBoxFitness_TextChanged(object sender, EventArgs e) {

        }
    }
}
