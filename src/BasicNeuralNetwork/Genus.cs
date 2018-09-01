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
            Genome g = Genome.CreateGenome(numberOfInputs, numberOfOutputs);
            g.MutateConnectionWeights();
            s.AddGenome(g);
            Species.Add(s); // start with one species

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

        public void ReduceSpecies(bool one = false) {
            foreach (Species species in Species) {
                species.TrimSpecies(one);
            }
        }

        /// <summary>
        /// Gets the genome with the highest fitness
        /// </summary>
        /// <returns></returns>
        public Genome BestGenome() {
            List<Genome> genomes = Genomes();
            genomes.Sort((o1, o2) => o2.Fitness.CompareTo(o1.Fitness));
            return genomes[0];
        }

        /// <summary>
        /// Compares to all existing species. If it doesn't match any existing then we create a new species
        /// </summary>
        /// <param name="genome"></param>
        public void AddToSpecies(Genome genome) {
            foreach (Species species in Species) {
                if (species.BestGenome.IsCompatibility(genome)) {
                    species.AddGenome(genome);
                    return;
                }
            }

            Species newspecies = new Species();
            newspecies.AddGenome(genome);
        }

        /// <summary>
        /// Removes the worst of the species. Gives species a number of a generations to optimize
        /// </summary>
        /// <param name="speciesToRemain"></param>
        public void RemoveWeakestSpecies(int speciesToRemain = -1) {
            if (speciesToRemain < 0) {
                speciesToRemain = NEATSettings.MaximumSpecies;
            }

            Species.Sort((o1, o2) => o2.GetAverageFitness().CompareTo(o1.GetAverageFitness()));

            int SpeciesToRemove = Species.Count - speciesToRemain;

            if (SpeciesToRemove < 1)
                return;

            for(int i = 0; i < SpeciesToRemove; i++) {
                if (Species[Species.Count - 1].Generations > NEATSettings.MinimumGenerations)
                    Species.RemoveAt(Species.Count - 1);
            }
        }

        /// <summary>
        /// Genomes are compared, crossover, and speciated
        /// </summary>
        public void NewGeneration() {
            generationNumber++;

           
            if (generationNumber == 1) {
                foreach (Genome g in Genomes()) {
                    g.Mutate();
                }
                return;
            }
            
            RemoveWeakestSpecies();

            // no need to go through new species which are added in the loop
            int speciesCount = Species.Count;
            
            for(int i =0; i < speciesCount; i++) {
                Species s = Species[i];
                s.Generations++;
                s.TrimSpecies(false);

                Genome g1 = s.BestGenome;
                Genome g2 = g1;
                if (s.Genomes.Count > 1) {
                    g2 = s.Genomes[1];
                }
                Genome child = g1.Crossover(g2);
                if (child.IsCompatibility(s.BestGenome))
                    s.AddGenome(child);
                else {
                    Species s2 = new Species();
                    s2.AddGenome(child);
                    Species.Add(s2);
                }
            }

            foreach (Genome g in Genomes())
            {
                if (g == BestGenome())
                    continue;
                g.Mutate();
            }
        }
    }
}
