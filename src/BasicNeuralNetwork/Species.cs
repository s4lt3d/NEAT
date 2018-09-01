using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATNeuralNetwork {
    class Species {
        public List<Genome> Genomes = new List<Genome>();

        Genome bestGenome = new Genome();
        public int Generations = 0;

        internal Genome BestGenome { get => bestGenome; }

        public double GetAverageFitness() {
            double fitness = 0;

            foreach (Genome g in Genomes) {
                fitness += g.Fitness;
            }

            return fitness / Math.Max(Genomes.Count, 1); // prevent div by zero
        }

        public void AddGenome(Genome g) {
            Genomes.Add(g);
        }

        void SortByFitness() {
            Genomes.Sort((g1, g2) =>  g1.Fitness.CompareTo(g2.Fitness));
            bestGenome = Genomes[0];
        }

        public void TrimSpecies(bool one)
        {
            SortByFitness();

            if (one == true){
                Genomes.Clear();
                Genomes.Add(BestGenome);
                return;
            }

            while(Genomes.Count > NEATSettings.IdealPopulation) { 
                Genomes.RemoveAt(Genomes.Count - 1);
            }
        }
    }
}
