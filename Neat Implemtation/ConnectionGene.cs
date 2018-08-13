using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat_Implemtation
{
    class ConnectionGene
    {
        int inNode;
        int outNode;
        float weight;
        bool expressed;
        int innovation;

        public ConnectionGene(int inNode, int outNode, float weight, bool expressed, int innovation) {
            this.inNode = inNode;
            this.outNode = outNode;
            this.weight = weight;
            this.expressed = expressed;
            this.innovation = innovation;
        }

        public int InNode { get => inNode; }
        public int OutNode { get => outNode; }
        public float Weight { get => weight; }
        public bool Expressed { get => expressed; set => expressed = value; }
        public int Innovation { get => innovation; }

        public void mutateWeight(Random r) {
            if(r.NextDouble() > 0.5)
                weight += (float)(2 * r.NextDouble() - 1);
        }

        public override string ToString() {
            return "{" + inNode + ", " + outNode + ", " + weight + ", " + expressed + ", " + innovation + "}";
        }
    }
}
