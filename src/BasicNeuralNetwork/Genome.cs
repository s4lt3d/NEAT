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

        public void InsertNewNode() {
            NodeGene n = new NodeGene();
            
        }
    }
}
