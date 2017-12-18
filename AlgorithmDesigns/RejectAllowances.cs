using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics;

namespace AlgorithmDesigns
{
    public class RejectAllowances
    {

        private LinkedList<int> policy;
        public IEnumerable<int> Policy { get { return policy; } }

        private double expectedTotalCost;
        public double ExpectedTotalCost { get { return expectedTotalCost; } }

        private CostFunction costFunction;

        /// <summary>
        /// Gets or sets the cost function for this particular RejectAllowance problem.
        /// </summary>
        /// <remarks>
        /// The cost function here is a uniary function that returns a double value which computes the cost of given lot size with given following cost.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// If null is used to set this CostFunction.
        /// </exception>
        public CostFunction CostFunction
        {
            get { return costFunction; }

            set
            {
                costFunction = value ?? throw new ArgumentNullException("Cost function must not be null.");
                expectedTotalCost = double.MaxValue;
                policy = null;
            }
        }

        private int runCount;
        public int RunCount
        {
            get { return runCount; }

            set
            {
                if (value <= 0)
                    throw new ArgumentException("The number of production run must be a positive integer.");
                expectedTotalCost = double.MaxValue;
                policy = null;
                runCount = value;
            }
        }

        private int maxLot;
        public int MaxLot
        {
            get { return maxLot; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("The maximum number of lot must be a positive integer.");
                expectedTotalCost = double.MaxValue;
                policy = null;
                maxLot = value;
            }
        }

        private double penaltyCost;
        public double PenaltyCost
        {
            get { return penaltyCost; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Penalty cost must be a positive number.");
                expectedTotalCost = double.MaxValue;
                policy = null;
                penaltyCost = value;
            }
        }

        private double defectiveProbability;
        public double DefectiveProbability
        {
            get { return defectiveProbability; }
            set
            {
                if ((value < 0) || (value > 1))
                    throw new ArgumentException("The value of defective probability must in the range of [0,1].");
                expectedTotalCost = double.MaxValue;
                policy = null;
                defectiveProbability = value;
            }
        }

        public RejectAllowances(int runCount, double penaltyCost, CostFunction costFunction, double defectiveProbability = 0.5, int maxLot = 1)
        {
            if (runCount <= 0)
                throw new ArgumentException("The number of production run must be a positive integer.");
            if (penaltyCost <= 0)
                throw new ArgumentException("Penalty cost must be a positive number.");
            if (costFunction == null)
                throw new ArgumentNullException("Cost function must not be null.");
            if ((defectiveProbability < 0) || (defectiveProbability > 1))
                throw new ArgumentException("The value of defective probability must in the range of [0,1].");
            if (maxLot <= 0)
                throw new ArgumentException("The maximum number of lot must be a positive integer.");

            this.runCount = runCount;
            this.penaltyCost = penaltyCost;
            this.costFunction = costFunction;
            this.defectiveProbability = defectiveProbability;
            this.maxLot = maxLot;
        }

        public void Execute()
        {
            policy = new LinkedList<int>();
            double followingCost = PenaltyCost;
            double[] costs = new double[maxLot + 1];

            for (int i = RunCount; i >= 1; i--)
            {
                for (int lotSize = 0; lotSize <= maxLot; lotSize++)
                    costs[lotSize] = costFunction(followingCost, defectiveProbability, lotSize);

                followingCost = MathUtility.Min(costs, out int bestLotSize);
                policy.AddFirst(bestLotSize);
            }

            expectedTotalCost = followingCost;
        }
    }
}
