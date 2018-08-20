using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat_Implemtation {
    class Species {
        Genome Representative;
        List<Genome> population = new List<Genome>();
        public static double CompatibilityDistance = 1;

        public Species() { }

        public Species(Genome g) {
            Representative = g;
            population.Add(g);
        }
    }
}
