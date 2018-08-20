using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat_Implemtation {
    class Species {
        Genome ApexGenome;
        List<Genome> population = new List<Genome>();

        public Species() { }

        public Species(Genome g) {
            ApexGenome = g;
            population.Add(g);
        }


    }
}
