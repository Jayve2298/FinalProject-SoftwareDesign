using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FinalProject_Jenotan.Commands
{
    public class FacultyNameInitialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Faculty faculty)
            {
                if (!string.IsNullOrWhiteSpace(faculty.FName) &&
                    !string.IsNullOrWhiteSpace(faculty.LName))
                {
                    return $"{faculty.FName[0]}. {faculty.LName}";
                }

                return faculty.LName ?? "";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
