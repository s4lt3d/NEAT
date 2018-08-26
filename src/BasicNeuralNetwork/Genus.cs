using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATNeuralNetwork
{
    class Genus
    {
        List<Species> Species = new List<Species>();
        int generationNumber = 0;

        public static Genus instance = null;

        public Genus() {
            instance = this;
        }
    }
}
