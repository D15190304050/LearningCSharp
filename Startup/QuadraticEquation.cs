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

        public string Root1 { get; private set; }
        public string Root2 { get; private set; }

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
                        this.Root1 = "any real number.";
                        this.Root2 = this.Root1;
                    }
                    else
                    {
                        this.IsSolvable = false;
                        this.IsReal = true;
                        this.Root1 = "has no solution";
                        this.Root2 = this.Root1;
                    }
                }
                else
                {
                    this.IsSolvable = true;
                    this.IsReal = true;
                    this.Root1 = -this.C / this.B + "";
                    this.Root2 = this.Root1;
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

                    this.Root1 = -this.B / denominator + " + " + discriminant / denominator + "i";
                    this.Root2 = -this.B / denominator + " - " + discriminant / denominator + "i";
                }
                else
                {
                    this.IsReal = true;

                    this.Root1 = (-this.B + discriminant) / denominator + "";
                    this.Root2 = (-this.B - discriminant) / denominator + "";
                }
            }
        }
    }
}
