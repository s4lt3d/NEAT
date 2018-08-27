using System;
using System.Collections.Generic;
using System.Linq;

namespace NEATNeuralNetwork {
    class Genome {
        Dictionary<int, NodeGene> nodes = new Dictionary<int, NodeGene>();
        Dictionary<int, ConnectionGene> connections = new Dictionary<int, ConnectionGene>();
        List<int> inputNodes = new List<int>();
        List<int> outputNodes = new List<int>();
        List<int> sortedNodes;
        List<int> visitedNodes;
        double fitness;

        Random r = new Random();

        public Dictionary<int, NodeGene> Nodes { get => nodes; set => nodes = value; }
        public Dictionary<int, ConnectionGene> Connections { get => connections; set => connections = value; }
        public double Fitness { get => fitness; set => fitness = value; }

        public int BuildNode(NodeGene.NodeType type, int id = -1) {
            if (Nodes.ContainsKey(id)) // we don't add nodes we already have
                return -1;
            else if (id < 0) {
                id = InnovationGenerator.NextInnovationNumber;
            }

            NodeGene n = new NodeGene(type, id);
            Nodes.Add(id, n);

            // these lists speed things up during evaluation
            if (type == NodeGene.NodeType.INPUT_NODE) {
                inputNodes.Add(id);
            }
            else if (type == NodeGene.NodeType.OUTPUT_NODE) {
                outputNodes.Add(id);
            }

            inputNodes.Sort((o1, o2) => Nodes[o1].Innovation.CompareTo(Nodes[o2].Innovation));
            outputNodes.Sort((o1, o2) => Nodes[o1].Innovation.CompareTo(Nodes[o2].Innovation));
            return id;
        }

        public int BuildConnection(int inNode, int outNode, double weight, bool expressed, int id = -1) {
            if (Connections.ContainsKey(id))
                return -1;
            else if (id < 0) {
                id = InnovationGenerator.NextInnovationNumber;
            }

            ConnectionGene g = new ConnectionGene(inNode, outNode, weight, expressed, id);
            Connections.Add(id, g);
            Sort();
            return id;
        }

        public void Sort() {
            sortedNodes = new List<int>(Connections.Count);
            visitedNodes = new List<int>(Connections.Count);

            foreach (KeyValuePair<int, NodeGene> g in Nodes) {
                SortNode(g.Key);
            }
            sortedNodes.Reverse();
            visitedNodes.Clear();
        }

        private void SortNode(int nodeId) {
            if (visitedNodes.Contains(nodeId))
                return;
            visitedNodes.Add(nodeId);
            foreach (int childNode in GetChildNodes(nodeId)) {
                SortNode(childNode);
            }
            sortedNodes.Add(nodeId);
        }

        private List<int> GetChildNodes(int nodeId) {
            List<int> childNodes = new List<int>();
            foreach (KeyValuePair<int, ConnectionGene> g in Connections) {
                if (g.Value.InNode == nodeId)
                    childNodes.Add(g.Value.OutNode);
            }
            return childNodes;
        }

        private List<int> GetParentNodes(int nodeId) {
            List<int> parentNodes = new List<int>();
            foreach (KeyValuePair<int, ConnectionGene> g in Connections) {
                if (g.Value.OutNode == nodeId)
                    parentNodes.Add(g.Value.OutNode);
            }
            return parentNodes;
        }

        public double GetWeightedInputSum(int nodeId) {
            double val = 0;

            // inputs and bias don't have parents
            if (Nodes[nodeId].Type == NodeGene.NodeType.INPUT_NODE || Nodes[nodeId].Type == NodeGene.NodeType.BIAS_NODE)
                return Nodes[nodeId].Value;

            foreach (KeyValuePair<int, ConnectionGene> g in Connections) {
                if (g.Value.Expressed)
                    if (g.Value.OutNode == nodeId)
                        val += g.Value.Weight * Nodes[g.Value.InNode].Value;
            }

            return NodeGene.EvaluationFunction(val);
        }

        public double[] Evaluate(double[] inputs) {
            double[] output = new double[outputNodes.Count];

            // set input values
            for (int i = 0; i < inputs.Length; i++) {
                Nodes[inputNodes[i]].Value = inputs[i];
            }

            // do feed forward evaluation
            foreach (int nodeToEvaluate in sortedNodes) {
                Nodes[nodeToEvaluate].Value = GetWeightedInputSum(nodeToEvaluate);
            }

            // get output values
            for (int i = 0; i < outputNodes.Count; i++) {
                output[i] = Nodes[outputNodes[i]].Value;
            }

            return output;
        }

        private int ContainsConnection(int n1, int n2) {
            foreach (KeyValuePair<int, ConnectionGene> cg in Connections) {
                if (cg.Value.InNode == n1) {
                    if (cg.Value.OutNode == n2) {
                        return cg.Key;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Connection weights mutate as in any NE system, with each connection either perturbed or not at each generation.
        /// </summary>
        public void MutateConnectionWeights() {
            foreach (KeyValuePair<int, ConnectionGene> g in Connections) {
                if (r.NextDouble() < NEATSettings.WeightMutation) {
                    g.Value.Weight += (float)(2 * r.NextDouble() - 1);
                }
            }
        }
    
        /// <summary>
        /// In the add connection mutation, a single new connection gene with a 
        /// random weight is added connecting two previously unconnected nodes.
        /// </summary>
        public void AddConnectionMutation()
        {
            if (r.NextDouble() > NEATSettings.ConnectionMutation)
                return;

            int n1 = r.Next(0, Nodes.Count); // (from 0 and count -1)
            int n2 = r.Next(0, Nodes.Count);

            int k = 0; // prevents runaway
            while (k++ < 64)
            {
                n1 = r.Next(0, Nodes.Count);
                n2 = r.Next(0, Nodes.Count);

                // cases which are not allowed.
                // can't have existing connection
                // can't change bias
                // can't connect from one input to another input
                if (ContainsConnection(n1, n2) == -1)
                {
                    if (Nodes[n2].Type != NodeGene.NodeType.INPUT_NODE)
                        if (Nodes[n2].Type != NodeGene.NodeType.BIAS_NODE)
                            break;
                }
            }

            if (k >= 64) {
                return; // failed to find a valid new connection
            }
            
            float w = (float)(r.NextDouble() * 2 - 1);
            BuildConnection(n1, n2, w, true);
        }

        /// <summary>
        ///  In the add node mutation, an existing connection is split and the 
        ///  new node placed where the old connection used to be. 
        ///  The old connection is disabled and two new connections are added 
        ///  to the genome. The new connection leading into the new node 
        ///  receives a weight of 1, and the new connection leading out 
        ///  receives the same weight as the old connection.
        /// </summary>
        /// <param name="r"></param>
        public void AddNodeMutation() {
            if (r.NextDouble() > NEATSettings.NodeMutation)
                return;
            int c = r.Next(0, Connections.Count); // (from 0 and count -1)
            int newNodeId = BuildNode(NodeGene.NodeType.HIDDEN_NODE);
            
            Connections[c].Expressed = false;
            int oldInNode = Connections[c].InNode;
            int oldOutNode = Connections[c].OutNode;
            double oldWeight = Connections[c].Weight;

            BuildConnection(oldInNode, newNodeId, 1, true);
            BuildConnection(newNodeId, oldOutNode, oldWeight, true);
        }

        /// <summary>
        /// A comparison of compatibiltiy before mating
        /// </summary>
        /// <param name="mate">The other genome to mate with</param>
        /// <returns></returns>
        public bool Compatibility(Genome mate)
        {
            int disjoint = 0;
            double weightDifference = 0;
            double weightDifferenceAvg = 0;
            int matches = 0;

            int disjointGenes = 0;
            for(int i = 0; i < Connections.Count; i++) {
                bool found = false;
                ConnectionGene myGene = Connections[i];
                for(int j = 0; j < mate.Connections.Count; j++) {
                    ConnectionGene mateGene = mate.Connections[j];
                    if (mateGene.Innovation == myGene.Innovation)
                    {
                        found = true;
                        matches++;
                        weightDifference += Math.Abs(mateGene.Weight - myGene.Weight);
                        break;
                    }
                }
                if (found == false)
                {
                    disjointGenes++;
                }
            }

            weightDifferenceAvg = weightDifference / matches;

            double delta = (double)disjoint / Math.Max(mate.Connections.Count, Connections.Count) + weightDifferenceAvg;

            return delta < NEATSettings.CompatibilityThreshold;
        }

        /// <summary>
        /// Breeding! When crossing over, the  genes in both genomes with the same innovation numbers are lined up.
        /// These genes are called matching genes.Genes that do not match are either disjoint or excess, depending
        /// on whether they occur within or outside the range of the other parent’s innovation
        /// numbers.They represent structure that is not present in the other genome.
        /// </summary>
        /// <param name="mate">Should be the most fit</param>
        /// <returns></returns>
        public Genome Crossover(Genome mate) {
            Genome self = this; // is this the female of the two? 
            Genome offspring = new Genome();

            foreach (KeyValuePair<int, NodeGene> node in mate.Nodes) {
                offspring.BuildNode(node.Value.Type, node.Value.Innovation);
            }
            foreach (KeyValuePair<int, NodeGene> node in Nodes) {
                offspring.BuildNode(node.Value.Type, node.Value.Innovation);
            }

            foreach (KeyValuePair<int, ConnectionGene> cp1 in mate.Connections) {
                if (Connections.ContainsKey(cp1.Key)) {
                    ConnectionGene cp2 = Connections[cp1.Key];
                    if (r.NextDouble() > 0.5) {
                        offspring.BuildConnection(cp1.Value.InNode, cp1.Value.OutNode, cp1.Value.Weight, cp1.Value.Expressed, cp1.Value.Innovation);
                    }
                    else {
                        offspring.BuildConnection(cp2.InNode, cp2.OutNode, cp2.Weight, cp2.Expressed, cp2.Innovation);
                    }
                }
                else {
                    offspring.BuildConnection(cp1.Value.InNode, cp1.Value.OutNode, cp1.Value.Weight, cp1.Value.Expressed, cp1.Value.Innovation);
                }
            }

            foreach (KeyValuePair<int, ConnectionGene> cp2 in Connections) {
                offspring.BuildConnection(cp2.Value.InNode, cp2.Value.OutNode, cp2.Value.Weight, cp2.Value.Expressed, cp2.Value.Innovation);
            }

            return offspring;
        }

        /// <summary>
        /// Creates genome with only inputs and outputs connected. 
        /// </summary>
        /// <param name="numberOfInputs"></param>
        /// <param name="numberOfOutputs"></param>
        /// <returns></returns>
        public static Genome CreateGenome(int numberOfInputs, int numberOfOutputs)
        {
            Genome g = new Genome();
            List<int> inputs = new List<int>();
            List<int> outputs = new List<int>();
            inputs.Add(g.BuildNode(NodeGene.NodeType.BIAS_NODE));

            for (int i = 0; i < numberOfInputs; i++)
                inputs.Add(g.BuildNode(NodeGene.NodeType.INPUT_NODE));

            for (int i = 0; i < numberOfOutputs; i++)
                outputs.Add(g.BuildNode(NodeGene.NodeType.OUTPUT_NODE));

            foreach (int i in inputs)
            {
                foreach (int o in outputs)
                {
                    g.BuildConnection(i, o, 0, true);
                }
            }

            return g;
        }
    }
}
