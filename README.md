# NEAT (Neuroevolution of Augmenting Topologies)

> A C# implementation of NEAT for evolving neural network topologies based on the original research paper.

---

## Overview

This is a practice implementation of NEAT (Neuroevolution of Augmenting Topologies), an evolutionary algorithm for optimizing both neural network weights and structure. Unlike traditional neural networks with fixed architectures, NEAT evolves networks from simple topologies into complex, efficient solutions.

---

## What is NEAT?

NEAT is an evolutionary algorithm that:
- **Evolves topology** — Adds and removes connections and neurons
- **Evolves weights** — Optimizes connection strengths
- **Starts simple** — Begins with minimal network structure
- **Grows complexity** — Only adds nodes when beneficial
- **Speciation** — Groups similar networks to maintain diversity

Key advantages:
- Automatically discovers optimal network structure
- More efficient than fixed-architecture networks
- Produces interpretable topologies
- Scales well to complex problems

---

## Features

- **Topology Evolution** — Dynamic network structure expansion
- **Weight Optimization** — Evolutionary weight adjustment
- **Speciation** — Maintains population diversity
- **Mutation Operators** — Add/remove nodes and connections
- **Crossover** — Genetic recombination
- **Fitness Tracking** — Detailed evolution metrics

---

## Getting Started

### Requirements

- **.NET Framework 4.7+**
- **Visual Studio 2017+** (or any C# IDE)

### Building

1. Open `NEAT.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Run the executable

---

## Technology Stack

- **Language:** C#
- **Platform:** Windows 10
- **Framework:** .NET Framework 4.7+
- **IDE:** Visual Studio Community 2017+

---

## Architecture

### Core Components

- **Genome** — Represents network structure (nodes and connections)
- **Network** — Evaluates fitness from genome
- **Population** — Manages species and evolution
- **Species** — Groups similar genomes
- **Mutation Engine** — Applies evolutionary operators

### Evolution Process

1. **Initialization** — Create initial population with random genomes
2. **Evaluation** — Calculate fitness for each network
3. **Speciation** — Group genomes by similarity
4. **Selection** — Pick best performers for reproduction
5. **Mutation** — Apply add/remove node/connection operators
6. **Crossover** — Combine parent genomes
7. **Repeat** — Continue for desired generations

---

## Usage

```csharp
// Create population
Population pop = new Population(populationSize);

// Run evolution loop
for (int generation = 0; generation < maxGenerations; generation++)
{
    // Evaluate fitness
    foreach (Genome g in pop.Members)
        g.Fitness = EvaluateFitness(g);

    // Evolve next generation
    pop.EvolveGeneration();
}

// Get best network
Genome best = pop.GetBest();
```

---

## References

- [Original NEAT Paper](http://nn.cs.utexas.edu/downloads/papers/stanley.ec02.pdf) — Stanley & Miikkulainen
- [NEAT Research](http://www.cs.utexas.edu/~nn/research/) — Official NEAT research page

---

## License

Copyright © Walter Gordy
