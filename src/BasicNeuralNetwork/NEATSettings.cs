using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATNeuralNetwork {
    public class NEATSettings {
        public static double CompatibilityDistance = 1;
        public static double CompatibilityDeltaDisjoint = 2;
        public static double CompatibilityDeltaWeights = 0.4;
        public static double CompatibilityDeltaThreshold = 1;

        public static double WeightMutation = 0.25;
        public static double ConnectionMutation = 2;
        public static double NodeMutation = 0.5;
        public static double EnableMutation = 0.2;
        public static double DisableMutation = 0.4;
        public static double Perturbation = 0.9;
        public static double Crossover = 0.75;

        public static int IdealSpeciesPopulation = 100;
        public static int NewGenerationSpecies = 10;
        public static int MaximumSpecies = 200;
        public static int MinimumGenerations = 5;
    }
}
