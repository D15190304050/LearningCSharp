using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp
{
    public class WeightLabel : TextBlock
    {
        private Line edge;

        public WeightLabel(Line edge)
        {
            this.edge = edge;
        }

        public void RefreshCoordinate()
        {
            base.SetValue(Canvas.LeftProperty, (edge.X1 + edge.X2) / 2);
            base.SetValue(Canvas.TopProperty, (edge.Y1 + edge.Y2) / 2 - 30.0);
        }
    }
}
