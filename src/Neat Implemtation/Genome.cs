using System;
using System.Collections.Generic;

namespace Neat_Implemtation
{
    class Genome
    {
        List<ConnectionGene> connections = new List<ConnectionGene>();
        List<NodeGene> nodes = new List<NodeGene>();
        int globalInnovationNumber = 0;

        public Genome() {
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
        public void mutateConnectionWeights(Random r) {
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
        public bool containsConnection(int node1, int node2) {
            foreach (ConnectionGene c in connections) {
                // else may be trimmed, but leaving for debugging issues
                if (c.InNode == node1 && c.OutNode == node2)
                    return true;
                else if (c.OutNode == node1 && c.InNode == node2)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// In the add connection mutation, a single new connection gene with a 
        /// random weight is added connecting two previously unconnected nodes.
        /// </summary>
        /// <param name="r"></param>
        public void addConnectionMutation(Random r) {
            int n1 = r.Next(0, nodes.Count); // (from 0 and count -1)
            int n2 = r.Next(0, nodes.Count);

            // no self connections
            int k = 0; // prevents runaway
            while (n1 == n2 && k++ < 10 && containsConnection(n1, n2) == true) { 
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
            if (containsConnection(n1, n2))
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
        public void addNode(Random r) {
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
    }
}
