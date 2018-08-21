using System;

namespace BasicNeuralNetwork
{
    class NodeGene : IEquatable<NodeGene>, IComparable<NodeGene>, ICloneable
    {
        public enum NodeType
        {
            INPUT_NODE,
            OUTPUT_NODE,
            HIDDEN_NODE,
            BIAS_NODE
        }

        private NodeType type;
        private int innovation;
        private int sortingID = -1;

        public NodeType Type { get => type; }
        public int Innovation { get => innovation; }
        public int SortingID { get => sortingID; set => sortingID = value; }

        public NodeGene(int innovation, NodeType type) {
            this.innovation = innovation;
            this.type = type;
        }

        // Functions for generic list sorting and comparing
        public int CompareTo(NodeGene other)
        {
            if (other == null) // sort nulls to end
                return -1;

            if (other.SortingID > SortingID)
            {
                return 1;
            }
            else if (other.SortingID == SortingID)
                return 0;

            return -1;
        }

        public bool Equals(NodeGene other)
        {
            return other.Innovation == innovation;
        }

        public object Clone()
        {
            return new NodeGene(innovation, type);
        }

    }
}
