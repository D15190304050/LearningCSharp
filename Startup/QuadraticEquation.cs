using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup
{
    public class QuadraticEquation
    {
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        public string Root { get; private set; }

        public bool IsReal { get; private set; }

        public bool IsSolvable { get; private set; }

        public QuadraticEquation(double a, double b, double c)
        {
            this.A = a;
            this.B = b;
            this.C = c;

            Solve();
        }

        private void Solve()
        {
            if (this.A == 0)
            {
                if (this.B == 0)
                {
                    if (this.C == 0)
                    {
                        this.IsSolvable = true;
                        this.IsReal = true;
                        this.Root = "any real number.";
                    }
                    else
                    {
                        this.IsSolvable = false;
                        this.IsReal = true;
                        this.Root = "no solution";
                    }
                }
                else
                {
                    this.IsSolvable = true;
                    this.IsReal = true;
                    this.Root = -this.C / this.B + "";
                }
            }
            else
            {
                this.IsSolvable = true;

                double discriminant = this.B * this.B - 4 * this.A * this.C;
                double denominator = 2 * this.A;

                if (discriminant < 0)
                {
                    this.IsReal = false;
                    discriminant = -discriminant;

                    string root1 = -this.B / denominator + " + " + discriminant / denominator + "i";
                    string root2 = -this.B / denominator + " - " + discriminant / denominator + "i";

                    this.Root = root1 + ", " + root2;
                }
                else
                {
                    this.IsReal = true;

                    string root1 = (-this.B + discriminant) / denominator + "";
                    string root2 = (-this.B - discriminant) / denominator + "";

                    this.Root = root1 + ", " + root2;
                }
            }
        }

        public override string ToString()
        {
            return this.A + " * x^2 + " + this.B + " * x + " + this.C + " = 0";
        }
    }
}
