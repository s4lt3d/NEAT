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
        
        public Genus(int numberOfInputs, int numberOfOutputs) {
            Species s = new Species();
            for (int i = 0; i < NEATSettings.InitialPopulation; i++)
            {
                Genome g = Genome.CreateGenome(numberOfInputs, numberOfOutputs);
                g.MutateConnectionWeights();
                s.AddGenome(g);
            }

            instance = this;
        }

        /// <summary>
        /// Function to get all genomes to evaluate
        /// </summary>
        /// <returns>List of all genomes available</returns>
        public List<Genome> Genomes() {
            List<Genome> genomes = new List<Genome>();
            foreach (Species s in Species) {
                genomes.AddRange(s.Genomes);
            }

            return genomes;
        }

        /// <summary>
        /// Gets the genome with the highest fitness
        /// </summary>
        /// <returns></returns>
        public Genome BestGenome() {
            List<Genome> genomes = Genomes();
            genomes.Sort((o1, o2) => o1.Fitness.CompareTo(o2.Fitness));
            return genomes[0];
        }

        /// <summary>
        /// Genomes are compared, crossover, and speciated
        /// </summary>
        public void NewGeneration() {

        }
    }
}
