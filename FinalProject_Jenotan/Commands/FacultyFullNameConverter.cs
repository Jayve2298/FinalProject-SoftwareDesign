using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FinalProject_Jenotan.Commands
{
    public class FacultyFullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Faculty faculty)
                return "";

            var first = faculty.FName ?? string.Empty;
            var last = faculty.LName ?? string.Empty;

            return $"{first} {last}".Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
