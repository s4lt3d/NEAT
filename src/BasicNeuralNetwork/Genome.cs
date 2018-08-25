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

        public void BuildNode(NodeGene.NodeType type, int id) {
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
        }

        public void BuildConnection(int inNode, int outNode, double weight, bool expressed, int id, bool reversed = false) {
            ConnectionGene g = new ConnectionGene(inNode, outNode, weight, expressed, id);
            ForwardConnections.Add(id, g);
            Sort();
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
    }
}
