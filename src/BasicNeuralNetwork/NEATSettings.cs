using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEATNeuralNetwork {
    public class NEATSettings {
        public static double CompatibilityThreshold = 1;
        public static double DeltaDisjoint = 2;
        public static double DeltaWeights = 0.4;
        public static double DeltaThreshold = 1;

        public static double WeightMutation = 0.25;
        public static double ConnectionMutation = 2;
        public static double NodeMutation = 0.5;
        public static double EnableMutation = 0.2;
        public static double DisableMutation = 0.4;
        public static double Perturbation = 0.9;
        public static double Crossover = 0.75;

    }
}
