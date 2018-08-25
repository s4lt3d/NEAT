using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicNeuralNetwork
{
    class Genome
    {
        Dictionary<int, NodeGene> Nodes = new Dictionary<int, NodeGene>();
        Dictionary<int, ConnectionGene> ForwardConnections = new Dictionary<int, ConnectionGene>();
        List<int> inputNodes = new List<int>();
        List<int> outputNodes = new List<int>();
        List<int> sortedNodes;
        List<int> visitedNodes;
        Random r = new Random();

        public int BuildNode(NodeGene.NodeType type, int id = -1) { 
            if(id < 0) {
                id = InnovationGenerator.NextInnovationNumber;
            }
            NodeGene n = new NodeGene(type, id);
            Nodes.Add(id, n);

            // these lists speed things up during evaluation
            if (type == NodeGene.NodeType.INPUT_NODE) {
                inputNodes.Add(id);
            }
            else if(type == NodeGene.NodeType.OUTPUT_NODE){
                outputNodes.Add(id);
            }

            inputNodes.Sort((o1, o2) => Nodes[o1].Innovation.CompareTo(Nodes[o2].Innovation));
            outputNodes.Sort((o1, o2) => Nodes[o1].Innovation.CompareTo(Nodes[o2].Innovation));
            return id;
        }

        public int BuildConnection(int inNode, int outNode, double weight, bool expressed, int id = -1) {
            if (id < 0) {
                id = InnovationGenerator.NextInnovationNumber;
            }
            ConnectionGene g = new ConnectionGene(inNode, outNode, weight, expressed, id);
            ForwardConnections.Add(id, g);
            Sort();
            return id;
        }
        
        public void Sort()
        {
            sortedNodes = new List<int>(ForwardConnections.Count);
            visitedNodes = new List<int>(ForwardConnections.Count);
            
            foreach (KeyValuePair<int, NodeGene> g in Nodes)
            {
                SortNode(g.Key);
            }
            sortedNodes.Reverse();
            visitedNodes.Clear();
        }

        private void SortNode(int nodeId)
        {
            if (visitedNodes.Contains(nodeId))
                return;
            visitedNodes.Add(nodeId);
            foreach (int childNode in GetChildNodes(nodeId))
            {
                SortNode(childNode);
            }
            sortedNodes.Add(nodeId);
        }

        private List<int> GetChildNodes(int nodeId)
        {
            List<int> childNodes = new List<int>();
            foreach (KeyValuePair<int, ConnectionGene> g in ForwardConnections) {
                if (g.Value.InNode == nodeId)
                    childNodes.Add(g.Value.OutNode);
            }
            return childNodes;
        }

        private List<int> GetParentNodes(int nodeId) {
            List<int> parentNodes = new List<int>();
            foreach (KeyValuePair<int, ConnectionGene> g in ForwardConnections) {
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

            foreach (KeyValuePair<int, ConnectionGene> g in ForwardConnections) {
                if(g.Value.Expressed)
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

        private int ContainsConnection(int n1, int n2)
        {
            foreach (KeyValuePair<int, ConnectionGene> cg in ForwardConnections)
            {
                if (cg.Value.InNode == n1)
                {
                    if (cg.Value.OutNode == n2)
                    {
                        return cg.Key;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// In the add connection mutation, a single new connection gene with a 
        /// random weight is added connecting two previously unconnected nodes.
        /// </summary>
        /// <param name="r"></param>
        public void AddConnectionMutation()
        {
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
                // can't change other inputs directly
                // self connections are fine
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
        public void AddNodeMutation()
        {
            int c = r.Next(0, ForwardConnections.Count); // (from 0 and count -1)
            int newNodeId = BuildNode(NodeGene.NodeType.HIDDEN_NODE);
            
            ForwardConnections[c].Expressed = false;
            int oldInNode = ForwardConnections[c].InNode;
            int oldOutNode = ForwardConnections[c].OutNode;
            double oldWeight = ForwardConnections[c].Weight;

            BuildConnection(oldInNode, newNodeId, 1, true);
            BuildConnection(newNodeId, oldOutNode, oldWeight, true);
        }
    }
}
