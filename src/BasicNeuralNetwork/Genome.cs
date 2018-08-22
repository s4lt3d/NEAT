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
        Dictionary<int, ConnectionGene> Connections = new Dictionary<int, ConnectionGene>();
        List<int> sortedNodes;
        List<int> visitedNodes;

        public void BuildNode(NodeGene.NodeType type, int id) {
            NodeGene n = new NodeGene(type, id);
            Nodes.Add(id, n);
            Sort();
        }

        public void BuildConnection(int inNode, int outNode, double weight, bool expressed, int id) {
            ConnectionGene g = new ConnectionGene(inNode, outNode, weight, expressed, id);
            Connections.Add(id, g);
            Sort();
        }

        public void Sort()
        {
            sortedNodes = new List<int>(Connections.Count);
            visitedNodes = new List<int>(Connections.Count);
            
            foreach (KeyValuePair<int, NodeGene> g in Nodes)
            {
                SortNode(g.Key);
            }
            
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
            foreach (KeyValuePair<int, ConnectionGene> g in Connections) {
                if (g.Value.InNode == nodeId)
                    childNodes.Add(g.Value.OutNode);
            }
            return childNodes;
        }

        //public double[] Evaluate(double[] inputs) {

        //}
    }
}
