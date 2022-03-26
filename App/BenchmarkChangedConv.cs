using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App
{
    public class BenchmarkChangedConv : IValueConverter
    {
        public object Convert(object value, Type target_type, object param, CultureInfo culture)
        {
            try
            {
                bool val = (bool)value;
                if (val)
                    return "Collection has been changed!";
                else
                    return "Collection has not been changed";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "ERROR";
            }
        }

        public object ConvertBack(object value, Type target_type, object param, CultureInfo culture)
        {
            return false;
        }
    }
}
