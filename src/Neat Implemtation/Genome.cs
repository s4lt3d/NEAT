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

        List<int> visited; // used in TopologicalSort functions
        List<int> sorted;  // 

        public void TopologicalSortConnections() {
            visited = new List<int>(connections.Count);
            sorted = new List<int>(connections.Count);

            foreach (ConnectionGene g in connections) {
                if (visited.Contains(g.Innovation) == false)
                    TopologicalSort(g);
            }

            for (int i = 0; i < sorted.Count; i++) {
                GetConnectionByID(sorted[i]).SortingID = i;
            }

            connections.Sort();

            visited.Clear();
            sorted.Clear();
        }

        public void TopologicalSort(ConnectionGene g) {
            if (visited.Contains(g.Innovation))
                return;

            visited.Add(g.Innovation);
            ConnectionGene child = GetConnectionByInNode(g.OutNode);

            if (child == null)
                sorted.Add(g.Innovation);
            else {
                TopologicalSort(child);
                sorted.Add(g.Innovation);
            }
        }

        public ConnectionGene GetConnectionByID(int id) {
            foreach (ConnectionGene g in Connections) {
                if (g.Innovation == id)
                    return g;
            }
            return null;
        }

        public ConnectionGene GetConnectionByInNode(int id) {
            foreach (ConnectionGene g in Connections) {
                if (visited.Contains(id))
                    return null;
                if (g.InNode == id)
                    return g;
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
            if(Math.Abs(fitness - other.fitness) < 0.001)
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
