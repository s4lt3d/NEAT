using System;

namespace Neat_Implemtation {
    class ConnectionGene : IEquatable<ConnectionGene>, IComparable<ConnectionGene> {
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

        public ConnectionGene replicate() {
            return new ConnectionGene(inNode, outNode, weight, expressed, innovation);
        }

        // Functions for generic list sorting and comparing
        public int CompareTo(ConnectionGene other) {
            if (other == null) // sort nulls to end
                return -1;

            if (other.Innovation > innovation) {
                return 1;
            } else if(other.Innovation == innovation)
                return 0;

            return -1;
        }

        public bool Equals(ConnectionGene other) {
            return other.Innovation == innovation;
        }

        public override int GetHashCode() {
            return innovation; // this is a global id
        }
    }
}
