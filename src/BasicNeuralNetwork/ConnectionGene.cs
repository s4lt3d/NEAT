﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicNeuralNetwork
{
    class ConnectionGene : IEquatable<ConnectionGene>, ICloneable
    {
        private int inNode;
        private int outNode;
        private double weight;
        private bool expressed;
        private int innovation;
        private bool reversed;

        public int InNode { get => inNode; }
        public int OutNode { get => outNode; }
        public double Weight { get => weight; }
        public bool Expressed { get => expressed; }
        public bool Reversed { get => reversed; }
        public int Innovation { get => innovation; }

        public ConnectionGene(int inNode, int outNode, double weight, bool expressed, int innovation, bool reversed)
        {
            this.inNode = inNode;
            this.outNode = outNode;
            this.weight = weight;
            this.expressed = expressed;
            this.innovation = innovation;
            this.reversed = reversed;
        }

        // Functions for generic list sorting and comparing. Used for topological sorting implemented in Genome class
       
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
            return new ConnectionGene(inNode, outNode, weight, expressed, innovation, reversed);
        }
    }
}
