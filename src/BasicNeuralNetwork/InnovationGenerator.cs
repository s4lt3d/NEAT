using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicNeuralNetwork
{
    public static class InnovationGenerator
    {
        private static int innovation = 0;

        public static int NextInnovationNumber { get => innovation++; }
    }
}
