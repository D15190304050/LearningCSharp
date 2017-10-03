using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.BasicDataStructures
{
    /// <summary>
    /// The StdRandom class provides static methods for generating random number for various descrete and coutinuous distributions,
    /// including Bernouli, uniform, Gaussian, exponential, pareto, Poisson, and Cauchy. It also provides method for shuffling an array or sub-array.
    /// </summary>
    public static class StdRandom
    {
        /// <summary>
        /// Pseudo-random number generator.
        /// </summary>
        private static Random random;

        /// <summary>
        /// Checks the probabiliy p given by the argument passed by the client, throw an ArgumentException if p is out of the range [0,1].
        /// </summary>
        /// <param name="p">the probabiliy given by the argument passed by the client</param>
        private static void ProbabilityRangeCheck(double p)
        {
            if (p < 0 || p > 1)
                throw new ArgumentException("Probability must between [0,1].");
        }

        /// <summary>
        /// Static initializer.
        /// </summary>
        static StdRandom() { random = new Random(); }

        /// <summary>
        /// Sets the seed of the pseudo-random number generator.
        /// This method enables you to produce the same sequence of "random" number for each excution of the program.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public static void SetSeed(int seed) { random = new Random(seed); }

        /// <summary>
        /// Returns a double number uniformly in [0, 1) as the probability generated randomly.
        /// </summary>
        /// <returns>A double number uniformly in [0, 1).</returns>
        public static double Uniform() { return random.NextDouble(); }

        /// <summary>
        /// Returns a random double number uniformly in [a,b).
        /// </summary>
        /// <param name="a">The left end-point.</param>
        /// <param name="b">The right end-point.</param>
        /// <returns>A random double number uniformly in [a,b).</returns>
        public static double Uniform(double a, double b)
        {
            if (a >= b)
                throw new ArgumentException("Invalid range.");
            return a + Uniform() * (b - a);
        }

        /// <summary>
        /// Returns a random integer number uniformly in [0,n).
        /// </summary>
        /// <param name="n">Number of possibile numebrs.</param>
        /// <returns>A random integer numebr uniformly in [0,n).</returns>
        public static int Uniform(int n) { return random.Next(n); }

        /// <summary>
        /// Returns a random real number uniformly in [a,b).
        /// </summary>
        /// <param name="a">The left end-point.</param>
        /// <param name="b">The right end-point.</param>
        /// <returns>Returns a random real number uniformly in [a,b).</returns>
        public static int Uniform(int a, int b)
        {
            if (b <= a)
                throw new ArgumentException("Invalid range.");
            if (((long)(b - a)) > int.MaxValue)
                throw new ArgumentException("Invalid range");

            return a + Uniform(b - a);
        }

        /// <summary>
        /// Returns a random boolean from a Bernouli distribution with success probability p.
        /// </summary>
        /// <param name="p">The probability of returning true.</param>
        /// <returns>True with probability p and false with probability (1-p).</returns>
        public static bool Bernouli(double p)
        {
            ProbabilityRangeCheck(p);
            return Uniform() < p;
        }

        /// <summary>
        /// Returns a random boolean from a Bernouli distribution with success probability 0.5.
        /// </summary>
        /// <returns>True with probability 0.5, false with probability 0.5.</returns>
        public static bool Bernouli() { return Bernouli(0.5); }

        /// <summary>
        /// Returns a random real number from standard Gaussian distribution (mean 0 and deviation 1).
        /// </summary>
        /// <returns>A random real number from standard Gaussian distribution (mean 0 and deviation 1).</returns>
        public static double Gaussian()
        {
            // Use the polar form of the Box-Muller transform.
            double x, y, r;
            do
            {
                x = Uniform(-1.0, 1.0);
                y = Uniform(-1.0, 1.0);
                r = x * x + y * y;
            }
            while (r >= 1 || r == 0);

            return x * Math.Sqrt(-2 * Math.Log(r) / r);

            // Remark: y * Math.Sqrt(-2 * Math.Log(r) / r)
            // is an independent random gaussian.
        }

        /// <summary>
        /// Returns a random real number from a Gaussian distribution with mean &mu; and standard deviation &sigma;.
        /// </summary>
        /// <param name="mu">The mean.</param>
        /// <param name="sigma">The standart deviation.</param>
        /// <returns>A random real number from a Gaussian distribution with mean &mu; and standard deviation &sigma;.</returns>
        public static double Gaussian(double mu, double sigma) { return mu + Gaussian() * sigma; }

        /// <summary>
        /// Returns a random integer from a geometric distribution with success probability p.
        /// </summary>
        /// <param name="p">The parameter of the geometric distribution.</param>
        /// <returns>A random integer from a geometric distribution with success probability p; or int.MaxValue if p is (nearly) equal to 1.0.</returns>
        public static int Geometric(double p)
        {
            ProbabilityRangeCheck(p);
            
            // Using algorithm given by Knuth.
            return (int)Math.Ceiling(Math.Log(Uniform()) / Math.Log(1 - p));
        }

        /// <summary>
        /// Returns a random integer from a Poisson distribution with mean &lambda;.
        /// </summary>
        /// <param name="lambda">The mean of the Poisson distribution.</param>
        /// <returns>A random integer from a Poisson distribution with mean &lambda;.</returns>
        public static int Poisson(double lambda)
        {
            if (lambda <= 0)
                throw new ArgumentException("Parameter lambda must be positive.");
            if (double.IsInfinity(lambda))
                throw new ArgumentException("Parameter lambda must not be infinity.");

            // Using algorithm given by Knuth.
            // See http://en.wikipedia.org/wiki/Poisson_distribution
            int k = 0;
            double p = 1.0;
            double expLambda = Math.Exp(-lambda);
            do
            {
                k++;
                p *= Uniform();
            }
            while (p >= expLambda);

            return k - 1;
        }

        /// <summary>
        /// Returns a random real number from a Pareto distribution with shape parameter &alpha;.
        /// </summary>
        /// <param name="alpha">The shape parameter.</param>
        /// <returns>A random real number from a Pareto distribution with shape parameter &alpha;.</returns>
        public static double Pareto(double alpha)
        {
            if (alpha <= 0)
                throw new ArgumentException("Shape parameter alpha must be positive.");
            return Math.Pow(1 - Uniform(), -1.0 / alpha) - 1.0;
        }

        /// <summary>
        /// Returns a random real number from the standard Pareto distribution.
        /// </summary>
        /// <returns>A random real number from the standard Pareto distribution.</returns>
        public static double Pareto() { return Pareto(1); }

        /// <summary>
        /// Returns a random real number from the cauchy distribution.
        /// </summary>
        /// <returns>A random real number from the cauchy distribution: with probablity probabilities[i].</returns>
        public static double Cauchy() { return Math.Tan(Math.PI * (Uniform() - 0.5)); }

        /// <summary>
        /// Returns a random real number from the specified discrete distribution.
        /// </summary>
        /// <param name="probabilities">The probability of occurance of each integer.</param>
        /// <returns>A random real number from the specified discrete distribution.</returns>
        public static int Discrete(double[] probabilities)
        {
            if (probabilities == null)
                throw new ArgumentNullException("Argument array is null.");

            const double Epsilon = 1E-4;
            double sum = 0;

            for (int i = 0; i < probabilities.Length; i++)
            {
                if (probabilities[i] < 0)
                    throw new ArgumentException("Array entry " + i + " must be non-negative: " + probabilities[i]);

                sum += probabilities[i];
            }

            if (Math.Abs(sum - 1) > Epsilon)
                throw new ArgumentException("Sum of array entries does not approximately equal to 1.0:" + sum);

            // The for loop may not return a value when both r is (nearly) 1.0 and when cumulative sum is less than 1.0 (as a result of floating-point round-off error).
            while (true)
            {
                double r = Uniform();
                sum = 0.0;
                for (int i = 0; i < probabilities.Length; i++)
                {
                    sum += probabilities[i];
                    if (sum > r)
                        return i;
                }
            }
        }

        /// <summary>
        /// Returns a random integer from the specified discrete distribution.
        /// </summary>
        /// <param name="frequencies">The frequency of occurance of each integer.</param>
        /// <returns>A random integer from the specified discrete distribution: with probability proportional to frequencies[i].</returns>
        public static int Discrete(int[] frequencies)
        {
            if (frequencies == null)
                throw new ArgumentNullException("Argument array is null.");

            long sum = 0;
            for (int i = 0; i < frequencies.Length; i++)
            {
                if (frequencies[i] < 0)
                    throw new ArgumentException("Array entry " + i + " must be non-negative: " + frequencies[i]);

                sum += frequencies[i];
            }

            if (sum == 0)
                throw new ArgumentException("At least one array entry must be positive");
            if (sum > int.MaxValue)
                throw new ArgumentException("Sum of frequencies overflows an int");

            // Pick index i with probablity proportional to frequency.
            double r = Uniform((int)sum);
            sum = 0;
            for (int i = 0; i < frequencies.Length; i++)
            {
                sum += frequencies[i];
                if (sum > r)
                    return i;
            }

            // Can't reach here.
            return -1;
        }

        /// <summary>
        /// Returns a random real number from the exponential distribution with rate &lambda;.
        /// </summary>
        /// <param name="lambda">The rate of exponential distribution.</param>
        /// <returns>A random real number from the exponential distribution with rate &lambda;.</returns>
        public static double Exponential(double lambda)
        {
            if (lambda <= 0)
                throw new ArgumentException("Rate lambda must be positive.");
            return -Math.Log(1-Uniform()) / lambda;
        }

        /// <summary>
        /// Re-arranges the elements of the specified array in uniformly random order.
        /// </summary>
        /// <typeparam name="T">A type parameter with no constrain.</typeparam>
        /// <param name="a">The array to shuffle.</param>
        public static void Shuffle<T>(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Argument array is null.");

            int length = a.Length;
            for (int i = 0; i < length; i++)
            {
                // Between i and n-1.
                int indexToChange = i + Uniform(length - i);

                // Swap them.
                T temp = a[i];
                a[i] = a[indexToChange];
                a[indexToChange] = temp;
            }
        }

        /// <summary>
        /// Re-arranges the elements in the specified sub-array (with index [low,high]) in uniformly random order.
        /// </summary>
        /// <typeparam name="T">A type parameter with no constrain.</typeparam>
        /// <param name="a">The array to shuffle.</param>
        /// <param name="low">The left end-point (inclusive).</param>
        /// <param name="high">The right end-point (inclusive).</param>
        public static void Shuffle<T>(T[] a, int low, int high)
        {
            if (a == null)
                throw new ArgumentNullException("Argument array is null.");
            if (low < 0 || low > high || high > a.Length)
                throw new IndexOutOfRangeException("Illegal sub-array range.");

            for (int i = low; i <= high; i++)
            {
                // Between i and high
                int indexToChange = i + Uniform(high - i);

                // Swap them.
                T temp = a[i];
                a[i] = a[indexToChange];
                a[indexToChange] = temp;
            }
        }
    }
}