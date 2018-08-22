using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicNeuralNetwork
{
    class ConnectionGene : IEquatable<ConnectionGene>, IComparable<ConnectionGene>, ICloneable
    {
        private int inNode;
        private int outNode;
        private double weight;
        private bool expressed;
        private int innovation;
        private int sortingID;

        public int InNode { get => inNode; }
        public int OutNode { get => outNode; }
        public double Weight { get => weight; }
        public bool Expressed { get => expressed; }
        public int Innovation { get => innovation; }
        public int SortingID { get => sortingID; set => sortingID = value;  }

        public ConnectionGene(int inNode, int outNode, double weight, bool expressed, int innovation)
        {
            this.inNode = inNode;
            this.outNode = outNode;
            this.weight = weight;
            this.expressed = expressed;
            this.innovation = innovation;
        }

        // Functions for generic list sorting and comparing. Used for topological sorting implemented in Genome class
        public int CompareTo(ConnectionGene other)
        {
            if (other == null) // sort nulls to end
                return -1;

            if (other.SortingID < SortingID)
            {
                return 1;
            }
            else if (other.SortingID == SortingID)
                return 0;

            return -1;
        }

        public bool Equals(ConnectionGene other)
        {
            return other.Innovation == innovation;
        }

        public override int GetHashCode()
        {
            return innovation; // this is a global id
        }

        public override string ToString()
        {
            return "{In:" + inNode + ", Out:" + outNode + ", W:" + weight + ", E:" + expressed + ", I:" + innovation + "}";
        }

        public object Clone()
        {
            return new ConnectionGene(inNode, outNode, weight, expressed, innovation);
        }
    }
}
