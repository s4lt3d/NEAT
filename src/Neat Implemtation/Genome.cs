using System;
using System.Collections.Generic;

namespace Neat_Implemtation
{
    class Genome
    {
        private List<ConnectionGene> connections = new List<ConnectionGene>();
        private List<NodeGene> nodes = new List<NodeGene>();
        private int globalInnovationNumber = 0;
        public double fitness = 0;
        public List<ConnectionGene> Connections { get => connections; }
        public List<NodeGene> Nodes { get => nodes; }

        Random r = new Random();

        public Genome(int innovationNumber, int seed) {
            r = new Random(seed);
            this.globalInnovationNumber = innovationNumber;
        }

        public Genome(int innovationNumber) {
            this.globalInnovationNumber = innovationNumber;
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
        /// The minimal structure a genome can have. For brand new genomes, not for crossover
        /// </summary>
        public void initGenome() {
            NodeGene bias = new NodeGene(NodeGene.NodeType.BIAS_NODE, globalInnovationNumber++);
            NodeGene n2 = new NodeGene(NodeGene.NodeType.OUTPUT_NODE, globalInnovationNumber++);
            NodeGene n3 = new NodeGene(NodeGene.NodeType.INPUT_NODE, globalInnovationNumber++);
            NodeGene n4 = new NodeGene(NodeGene.NodeType.INPUT_NODE, globalInnovationNumber++);

            nodes.Add(bias);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);

            ConnectionGene c1 = new ConnectionGene(0, 1, -1, true, globalInnovationNumber++);
            ConnectionGene c2 = new ConnectionGene(2, 1, 2, true, globalInnovationNumber++);
            ConnectionGene c3 = new ConnectionGene(3, 1, 3, true, globalInnovationNumber++);

            connections.Add(c1);
            connections.Add(c2);
            connections.Add(c3);
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
            if (n1 == n2)
                return;
            if (containsConnection(n1, n2) != null)
                return;
            else if (nodes[n1].Type == NodeGene.NodeType.INPUT_NODE && nodes[n2].Type == NodeGene.NodeType.INPUT_NODE)
                return;
            else if (nodes[n1].Type == NodeGene.NodeType.OUTPUT_NODE && nodes[n2].Type == NodeGene.NodeType.OUTPUT_NODE)
                return;
            else if (nodes[n1].Type == NodeGene.NodeType.BIAS_NODE && nodes[n2].Type == NodeGene.NodeType.INPUT_NODE)
                return;
            else if (nodes[n1].Type == NodeGene.NodeType.INPUT_NODE && nodes[n2].Type == NodeGene.NodeType.BIAS_NODE)
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
            NodeGene newNode = new NodeGene(NodeGene.NodeType.HIDDEN_NODE, globalInnovationNumber++);
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
                if (node.Id == gene.Id)
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
        /// Breeding! When crossing over, the  genes in both genomes with the same innovation numbers are lined up.
        /// These genes are called matching genes.Genes that do not match are either disjoint or excess, depending
        /// on whether they occur within or outside the range of the other parent’s innovation
        /// numbers.They represent structure that is not present in the other genome.
        /// </summary>
        /// <param name="parent1">Should be the most fit</param>
        /// <param name="parent2">Should be the least fit</param>
        /// <returns></returns>
        public Genome crossover(Genome parent1) {
            Genome parent2 = this; // we are the female? 
            Genome offspring = new Genome(Math.Max(parent1.globalInnovationNumber, parent2.globalInnovationNumber)); // continue innovataion number

            foreach (NodeGene node in parent1.Nodes) {
                offspring.BuildNodeGenesFromCrossover(node);
            }
            foreach (NodeGene node in parent2.Nodes) {
                offspring.BuildNodeGenesFromCrossover(node);
            }

            foreach (ConnectionGene cp1 in parent1.Connections) {
                ConnectionGene cp2 = parent2.containsConnection(cp1);
                if (cp2 != null) {
                    if (r.NextDouble() > 0.5) {
                        offspring.BuildConnectionGenesFromCrossover(cp1.replicate());
                    }
                    else {
                        offspring.BuildConnectionGenesFromCrossover(cp2.replicate());
                    }
                }
                else {
                    offspring.BuildConnectionGenesFromCrossover(cp1.replicate());
                }
            }

            foreach (ConnectionGene cp2 in parent2.Connections) {
                offspring.BuildConnectionGenesFromCrossover(cp2.replicate());
            }

            return offspring;
        }
    }
}
