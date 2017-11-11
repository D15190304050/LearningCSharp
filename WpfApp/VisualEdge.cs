using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfApp
{
    public class VisualEdge : IComparable<VisualEdge>
    {
        private int v;
        private int w;
        public double Weight { get; }
        public Line Line { get; }

        public VisualEdge(int v, int w, double weight, Line line)
        {
            if ((v < 0) || (w < 0))
                throw new ArgumentException("Vertex name must be a non-negative integer.");
            if (double.IsNaN(weight))
                throw new ArgumentException("Weight is NaN.");
            if (line == null)
                throw new ArgumentException("The visual line is null.");

            this.v = v;
            this.w = w;
            this.Weight = weight;
            this.Line = line;
        }

        public int Either() => v;

        public int Other(int vertex)
        {
            if (vertex == v)
                return w;
            else if (vertex == w)
                return v;
            else
                throw new ArgumentException("Illegal end-point.");
        }

        public int CompareTo(VisualEdge that)
        {
            const double FloatingPointEpsilon = 1E-5;

            if (this.Weight - that.Weight < -FloatingPointEpsilon)
                return -1;
            else if (this.Weight - that.Weight > FloatingPointEpsilon)
                return 1;
            else
                return 0;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1} {2:F2}", v, w, this.Weight);
        }
    }
}
