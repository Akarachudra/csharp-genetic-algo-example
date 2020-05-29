using System;
using System.Linq;

namespace GeneticString
{
    internal class Program
    {
        private const string ExpectedString = "Hello beautiful world!";

        private struct Member
        {
            public char[] Str { get; set; }

            public int Fitness { get; set; }

            public override string ToString()
            {
                return new string(Str);
            }
        }

        private static void Main(string[] args)
        {
            const int limit = 100000;
            const int populationNumber = 4;
            var best = string.Empty;
            var bestValue = -1;
            var population = new Member[populationNumber];
            for (var i = 0; i < populationNumber; i++)
            {
                population[i] = new Member
                {
                    Str = GetRandomString()
                };
            }

            for (var i = 0; i < limit; i++)
            {
                for (var j = 0; j < populationNumber; j++)
                {
                    population[j].Fitness = Fitness(population[j].Str);
                    if (population[j].Fitness >= bestValue)
                    {
                        bestValue = population[j].Fitness;
                        best = new string(population[j].Str);
                    }
                }

                population = population.OrderByDescending(x => x.Fitness).ToArray();

                // Selection
                var newPopulation = new Member[populationNumber];
                newPopulation[0] = population[0];
                newPopulation[1] = population[1];
                newPopulation[2] = population[0];
                newPopulation[3] = population[2];

                // Crossover
                var crossovered = Crossover(newPopulation[0], newPopulation[1]);
                newPopulation[0] = crossovered[0];
                newPopulation[1] = crossovered[1];
                crossovered = Crossover(newPopulation[2], newPopulation[3]);
                newPopulation[2] = crossovered[0];
                newPopulation[3] = crossovered[1];

                // Mutation
                Mutate(newPopulation[2].Str);
                Mutate(newPopulation[3].Str);

                population = newPopulation;

                if (bestValue == ExpectedString.Length)
                {
                    Console.WriteLine($"Found at: {i}");
                    break;
                }
            }

            Console.WriteLine(best);
        }

        private static void Mutate(char[] str)
        {
            var random = new Random();
            var rndPos = random.Next(ExpectedString.Length);
            var rndChar = (char)random.Next(256);
            str[rndPos] = rndChar;
        }

        private static Member[] Crossover(Member first, Member second)
        {
            var mid = ExpectedString.Length / 2;
            var firstCharArray = first.Str.ToArray();
            var secondCharArray = second.Str.ToArray();
            for (var i = 0; i <= mid; i++)
            {
                var t = firstCharArray[i];
                firstCharArray[i] = secondCharArray[i];
                secondCharArray[i] = t;
            }

            first.Str = firstCharArray;
            second.Str = secondCharArray;
            return new[] { first, second };
        }

        private static int Fitness(char[] s)
        {
            var result = 0;
            for (var i = 0; i < ExpectedString.Length; i++)
            {
                if (s[i] == ExpectedString[i])
                {
                    result++;
                }
            }

            return result;
        }

        private static char[] GetRandomString()
        {
            var random = new Random();
            var resultStr = new char[ExpectedString.Length];
            for (var i = 0; i < ExpectedString.Length; i++)
            {
                resultStr[i] = (char)random.Next(256);
            }

            return resultStr;
        }
    }
}