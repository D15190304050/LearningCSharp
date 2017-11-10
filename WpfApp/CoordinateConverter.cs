using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace WpfApp
{
    //[ValueConversion(typeof(double), typeof(double))]
    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double referenceCoordinate = (double)parameter;
            double lblCoordinate = (double)value;
            //if (!double.TryParse(value.ToString(), ))

                return lblCoordinate + referenceCoordinate / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
