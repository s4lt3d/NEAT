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

        public void InitializeGenus(int numberOfInputs, int numberOfOutputs) {

            Species s = new Species();
            for (int i = 0; i < NEATSettings.InitialPopulation; i++)
            {
                Genome g = Genome.CreateGenome(numberOfInputs, numberOfOutputs);
                g.MutateConnectionWeights();
                s.AddGenome(g);
            }
        }
    }
}
