using System;
using System.Collections.Generic;

namespace Neat_Implemtation
{
    class Genome : IEquatable<Genome>, IComparable<Genome>, ICloneable {
        private List<ConnectionGene> connections = new List<ConnectionGene>();
        private List<NodeGene> nodes = new List<NodeGene>();
        public double fitness = 0;
        public List<ConnectionGene> Connections { get => connections; }
        public List<NodeGene> Nodes { get => nodes; }

        Random r = new Random();

        public Genome(int seed) {
            r = new Random(seed);
        }

        public Genome() {

        }
        
        public override string ToString() {
            string s = "------\r\n";
            foreach (ConnectionGene connection in Connections) {
                s += connection.Innovation + "\r\n";
                s += connection.InNode + "->" + connection.OutNode + "\r\n";
                if (connection.Expressed == false)
                    s += "DISAB\r\n";
                s += "------\r\n";
            }
            return s;
        }


        /// <summary>
        /// Evaluates the network given inputs and produces outputs
        /// </summary>
        /// <param name="inputs">List of inputs</param>
        /// <returns>Array of outputs</returns>
        double[] Evaluate(List<double> inputs) {
            List<NodeGene> i = GetInputNodes();

            i.Sort((x, y) => x.Innovation.CompareTo(y.Innovation));

            for (int k = 0; k < i.Count; k++) {
                i[k].evaluatedValue = inputs[k];
            }

            foreach (ConnectionGene g in Connections) {
                NodeGene nodeIn = GetNodeByID(g.InNode);
                NodeGene nodeOut = GetNodeByID(g.OutNode);
                nodeOut.evaluatedValue += nodeIn.sigmoid(nodeIn.evaluatedValue) * g.Weight;
            }

            List<NodeGene> o = GetOutputNodes();
            o.Sort((x, y) => x.Innovation.CompareTo(y.Innovation));

            double[] outputs = new double[o.Count];

            for (int k = 0; k < o.Count; k++) {
                outputs[k] = o[k].evaluatedValue;
            }

            return outputs;
        }

        private List<NodeGene> visited; // used in TopologicalSort functions
        private List<NodeGene> sorted;  // 

        List<ConnectionGene> GetConnectionByInNode(int id) {
            List<ConnectionGene> connections = new List<ConnectionGene>();
            foreach (ConnectionGene g in Connections) {
                if (g.InNode == id)
                    connections.Add(g);
            }
            return connections;
        }

        List<ConnectionGene> GetConnectionByOutNode(int id) {
            List<ConnectionGene> connections = new List<ConnectionGene>();
            foreach (ConnectionGene g in Connections) {
                if (g.OutNode == id)
                    connections.Add(g);
            }
            return connections;
        }

        List<NodeGene> GetChildNodes(int id) {
            List<NodeGene> children = new List<NodeGene>();
            foreach (ConnectionGene g in GetConnectionByInNode(id)) {
                children.Add(GetNodeByID(g.OutNode));
            }

            return children;
        }

        /// <summary>
        /// Topological Depth-first search.
        /// </summary>
        public void Sort() {
            sorted = new List<NodeGene>(Connections.Count);
            visited = new List<NodeGene>(Connections.Count);

            foreach (NodeGene g in Nodes)
                g.SortingID = -1;

            foreach (NodeGene g in GetInputNodes()) {
                SortNode(g);
            }

            for(int i = 0; i < sorted.Count; i++) {
                sorted[i].SortingID = i;
            }

            nodes.Sort();
            int k = 0;
            foreach (NodeGene n in Nodes) {
                foreach (ConnectionGene g in GetConnectionByInNode(n.Innovation)) {
                    g.SortingID = k++;
                }
            }

            connections.Sort();

            visited.Clear();
            sorted.Clear();
        }

        /// <summary>
        /// Recursive function to add nodes to sorted list
        /// </summary>
        /// <param name="g">Node to sort</param>
        private void SortNode(NodeGene g) {
            if (visited.Contains(g))
                return;
            visited.Add(g);
            foreach (NodeGene c in GetChildNodes(g.Innovation)) {
                SortNode(c);
            }
            sorted.Add(g);
        }

        public List<ConnectionGene> GetInputConnections() {
            List<ConnectionGene> inputs = new List<ConnectionGene>();
            foreach (ConnectionGene g in Connections) {
                NodeGene n = GetNodeByID(g.InNode);
                if (n == null)
                    continue;
                if (n.Type == NodeGene.NodeType.INPUT_NODE) {
                    inputs.Add(g);
                }
            }

            return inputs;
        }

        public List<NodeGene> GetInputNodes() {
            List<NodeGene> inputs = new List<NodeGene>();
            foreach (NodeGene n in Nodes) {
                if (n.Type == NodeGene.NodeType.INPUT_NODE) {
                    inputs.Add(n);
                }
            }

            return inputs;
        }

        public List<NodeGene> GetOutputNodes() {
            List<NodeGene> outputs = new List<NodeGene>();
            foreach (NodeGene n in Nodes) {
                if (n.Type == NodeGene.NodeType.INPUT_NODE) {
                    outputs.Add(n);
                }
            }

            return outputs;
        }

        public NodeGene GetNodeByID(int id) {
            foreach (NodeGene n in Nodes) {
                if (n.Innovation == id)
                    return n;
            }
            return null;
        }

        /// <summary>
        /// The minimal structure a genome can have. For brand new genomes, not for crossover
        /// </summary>
        public void initGenome() {
            NodeGene bias = new NodeGene(NodeGene.NodeType.BIAS_NODE, GlobalInnovation.Next);
            NodeGene n2 = new NodeGene(NodeGene.NodeType.OUTPUT_NODE, GlobalInnovation.Next);
            NodeGene n3 = new NodeGene(NodeGene.NodeType.INPUT_NODE, GlobalInnovation.Next);
            NodeGene n4 = new NodeGene(NodeGene.NodeType.INPUT_NODE, GlobalInnovation.Next);

            nodes.Add(bias);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);

            ConnectionGene c1 = new ConnectionGene(0, 1, -1, true, GlobalInnovation.Next);
            ConnectionGene c2 = new ConnectionGene(2, 1, 2, true, GlobalInnovation.Next);
            ConnectionGene c3 = new ConnectionGene(3, 1, 3, true, GlobalInnovation.Next);

            connections.Add(c1);
            connections.Add(c2);
            connections.Add(c3);
            connections.Sort();
        }
        
        /// <summary>
        /// Connection weights mutate as in any NE system, with each connection either perturbed or not at each generation.
        /// </summary>
        /// <param name="r"></param>
        public void mutateConnectionWeights() {
            for (int i = 0; i < connections.Count; i++) {
                connections[i].mutateWeight(r);
            }
        }


        /// <summary>
        /// Does the connection list contain a link between two nodes?
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public ConnectionGene containsConnection(int node1, int node2) {
            foreach (ConnectionGene c in connections) {
                // else may be trimmed, but leaving for debugging issues
                if (c.InNode == node1 && c.OutNode == node2)
                    return c;
                else if (c.OutNode == node1 && c.InNode == node2)
                    return c;
            }

            return null;
        }

        /// <summary>
        /// Does the connection list contain a link between two nodes?
        /// </summary>
        /// <param name="gene"></param>
        /// <returns></returns>
        public ConnectionGene containsConnection(ConnectionGene gene) {
            return containsConnection(gene.InNode, gene.OutNode);
        }

        /// <summary>
        /// In the add connection mutation, a single new connection gene with a 
        /// random weight is added connecting two previously unconnected nodes.
        /// </summary>
        /// <param name="r"></param>
        public void addConnectionMutation() {
            int n1 = r.Next(0, nodes.Count); // (from 0 and count -1)
            int n2 = r.Next(0, nodes.Count);

            // no self connections
            int k = 0; // prevents runaway
            while (n1 == n2 && k++ < 10 && containsConnection(n1, n2) != null) { 
                n2 = r.Next(0, nodes.Count);
            }


            // connections flow from n1 to n2. Swap if needed
            bool reverse = false;

            if (nodes[n2].Type == NodeGene.NodeType.INPUT_NODE)
                reverse = true;
            else if (nodes[n1].Type == NodeGene.NodeType.OUTPUT_NODE)
                reverse = true;
            else if (nodes[n2].Type == NodeGene.NodeType.BIAS_NODE)
                reverse = true;
            if (reverse == true) {
                int swap = n1;
                n1 = n2;
                n2 = swap;
            }
            
            // cases which are not allowed.
            // can't have two connections
            // can't change bias
            // can't change other inputs directly
            // self connections are fine
            if (containsConnection(n1, n2) != null)
                return;
            else if (nodes[n2].Type == NodeGene.NodeType.BIAS_NODE)
                return;
            
            float w = (float)(r.NextDouble() * 2 - 1);
            ConnectionGene connection = new ConnectionGene(n1, n2, w, true, n1);
            connections.Add(connection);
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
        public void addNode() {
            int c = r.Next(0, connections.Count); // (from 0 and count -1)
            NodeGene newNode = new NodeGene(NodeGene.NodeType.HIDDEN_NODE, GlobalInnovation.Next);
            int newNodeNumber = nodes.Count;
            nodes.Add(newNode);

            connections[c].Expressed = false;

            ConnectionGene c1 = new ConnectionGene(connections[c].InNode, newNodeNumber, 1, true, connections[c].InNode);
            ConnectionGene c2 = new ConnectionGene(newNodeNumber, connections[c].OutNode, connections[c].Weight, true, connections[c].InNode);

            connections.Add(c1);
            connections.Add(c2);
        }

        

        public void BuildNodeGenesFromCrossover(NodeGene gene) {
            foreach (NodeGene node in nodes)
                if (node.Innovation == gene.Innovation)
                    return;
            // otherwise add the gene
            nodes.Add(gene);
        }

        public void BuildConnectionGenesFromCrossover(ConnectionGene gene) {
            foreach (ConnectionGene connection in connections) {
                if (connection.Innovation == gene.Innovation)
                    return;
            }
            // otherwise add the gene
            connections.Add(gene);
        }


        /// <summary>
        /// A comparison of compatibiltiy before mating
        /// </summary>
        /// <param name="mate">The other genome to mate with</param>
        /// <returns></returns>
        public double Compatibility(Genome mate) {
            int disjoint = 0;
            double weightDifference = 0;
            double weightDifferenceAvg = 0;
            int matches = 0;

            int disjointGenes = 0;
            foreach (ConnectionGene gene in connections) {
                bool found = false;
                foreach (ConnectionGene other in mate.Connections) {
                    if (other.Innovation == gene.Innovation) {
                        found = true;
                        matches++;
                        weightDifference += Math.Abs(other.Weight - gene.Weight);
                        break;
                    }
                }
                if (found == false) {
                    disjointGenes++;
                }
            }

            weightDifferenceAvg = weightDifference / matches;

            double delta = (double)disjoint / Math.Max(mate.Connections.Count, connections.Count) + weightDifferenceAvg;

            return delta;
        }
        
        /// <summary>
        /// Breeding! When crossing over, the  genes in both genomes with the same innovation numbers are lined up.
        /// These genes are called matching genes.Genes that do not match are either disjoint or excess, depending
        /// on whether they occur within or outside the range of the other parent’s innovation
        /// numbers.They represent structure that is not present in the other genome.
        /// </summary>
        /// <param name="mate">Should be the most fit</param>
        /// <returns></returns>
        public Genome crossover(Genome mate) {
            Genome self = this; // we are the female? 
            Genome offspring = new Genome(); 

            foreach (NodeGene node in mate.Nodes) {
                offspring.BuildNodeGenesFromCrossover(node);
            }
            foreach (NodeGene node in self.Nodes) {
                offspring.BuildNodeGenesFromCrossover(node);
            }

            foreach (ConnectionGene cp1 in mate.Connections) {
                ConnectionGene cp2 = self.containsConnection(cp1);
                if (cp2 != null) {
                    if (r.NextDouble() > 0.5) {
                        offspring.BuildConnectionGenesFromCrossover((ConnectionGene)cp1.Clone());
                    }
                    else {
                        offspring.BuildConnectionGenesFromCrossover((ConnectionGene)cp2.Clone());
                    }
                }
                else {
                    offspring.BuildConnectionGenesFromCrossover((ConnectionGene)cp1.Clone());
                }
            }

            foreach (ConnectionGene cp2 in self.Connections) {
                offspring.BuildConnectionGenesFromCrossover((ConnectionGene)cp2.Clone());
            }

            return offspring;
        }

        public bool EvaluationNetwork(List<float> inputs) {


            return true;
        }


        // interface functions ////////////////

        public object Clone() {
            Genome g = new Genome();
            foreach (NodeGene node in Nodes)
                g.BuildNodeGenesFromCrossover((NodeGene)node.Clone());

            foreach (ConnectionGene connection in Connections)
                g.BuildConnectionGenesFromCrossover((ConnectionGene)connection.Clone());

            g.fitness = fitness;

            return g;
        }

        public bool Equals(Genome other) {
            if(Compatibility(other) < Species.CompatibilityDistance)
                return true;

            return false;
        }

        public int CompareTo(Genome other) {
            if (other == null) // sort nulls to end
                return -1;

            if (other.fitness > fitness) {
                return 1;
            }
            else if (other.fitness == fitness) {
                if (other.Nodes.Count > Nodes.Count)
                    return 1;
                else if (other.Nodes.Count == Nodes.Count)
                    return 0;

                return -1;
            }

            return -1;
        }
    }
}
