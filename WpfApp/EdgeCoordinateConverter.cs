using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// The EdgeCoordinatorConverter class implements the IValueConverter interfance to provide the conversion between the coordinate of vertices of a graph and that of an end point of a visual edge.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class EdgeCoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double referenceCoordinate = (double)parameter;
            double lblCoordinate = (double)value;

            return lblCoordinate + referenceCoordinate / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
