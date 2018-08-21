using System;

namespace Neat_Implemtation {
    class ConnectionGene : IEquatable<ConnectionGene>, IComparable<ConnectionGene>, ICloneable {
        int inNode;
        int outNode;
        float weight;
        bool expressed;
        int innovation;
        private int sortingID = -1;

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
        public int SortingID {
            get => sortingID;

            set  {
                   sortingID = value;
            }
        }

        public void mutateWeight(Random r) {
            if(r.NextDouble() > 0.5)
                weight += (float)(2 * r.NextDouble() - 1);
        }

        public override string ToString() {
            return "{" + inNode + ", " + outNode + ", " + weight + ", " + expressed + ", " + innovation + "}";
        }

        public object Clone() {
            return new ConnectionGene(inNode, outNode, weight, expressed, innovation);
        }

        // Functions for generic list sorting and comparing. Used for topological sorting implemented in Genome class
        public int CompareTo(ConnectionGene other) {
            if (other == null) // sort nulls to end
                return -1;

            if (other.SortingID < SortingID) {
                return 1;
            } else if(other.SortingID == SortingID)
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
