using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// The WeightLabel class represents the TextBlock object that contains the weight of a graph.
    /// </summary>
    public class WeightLabel : TextBlock
    {
        /// <summary>
        /// The Line that represents the edge on the canvas.
        /// </summary>
        private Line edge;

        /// <summary>
        /// Initializes an instance of WeightLabel using the specified Line that represents an edge in a graph.
        /// </summary>
        /// <param name="edge">The specified Line that represents an edge in a graph</param>
        public WeightLabel(Line edge)
        {
            this.edge = edge;
        }

        /// <summary>
        /// Refresh the coordinate of this WeightLabel instance.
        /// </summary>
        public void RefreshCoordinate()
        {
            base.SetValue(Canvas.LeftProperty, (edge.X1 + edge.X2) / 2);
            base.SetValue(Canvas.TopProperty, (edge.Y1 + edge.Y2) / 2 - 30.0);
        }
    }
}
