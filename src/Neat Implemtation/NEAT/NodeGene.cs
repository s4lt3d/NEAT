﻿using System;

namespace Neat_Implemtation  {
    public class NodeGene : IEquatable<NodeGene>, IComparable<NodeGene>, ICloneable {
        public enum NodeType {
            INPUT_NODE, 
            OUTPUT_NODE,
            HIDDEN_NODE,
            BIAS_NODE
        }

        NodeType type;
        int innovation;
        int SortingID = -1;

        public NodeType Type { get => type;  }
        public int Innovation { get => innovation; }

        public double evaluatedValue;


        public NodeGene(NodeType type, int id) {
            this.type = type;
            this.innovation = id;
        }

        public object Clone() {
            return new NodeGene(type, innovation);
        }

        public static double sigmoid(double x)
        {
            return 2.0 / (1.0 + Math.Exp(-4.9 * x)) - 1;
        }

        public override string ToString() {
            string s = "{";
            if (type == NodeType.INPUT_NODE)
                s += "INPUT,  ";
            else if (type == NodeType.OUTPUT_NODE)
                s += "OUTPUT, ";
            else if (type == NodeType.HIDDEN_NODE)
                s += "HIDDEN, ";
            else if (type == NodeType.BIAS_NODE)
                s += "BIAS,   ";

            s += innovation + "}";
            return s;
        }

        // Functions for generic list sorting and comparing
        public int CompareTo(NodeGene other) {
            if (other == null) // sort nulls to end
                return -1;

            if (other.SortingID > SortingID) {
                return 1;
            }
            else if (other.SortingID == SortingID)
                return 0;

            return -1;
        }

        public bool Equals(NodeGene other) {
            return other.Innovation == innovation;
        }

        public override int GetHashCode() {
            return innovation; // this is a global id
        }
    }
}
