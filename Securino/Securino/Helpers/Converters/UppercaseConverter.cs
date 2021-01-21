// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UppercaseConverter.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the UppercaseConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.Converters
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    /// <summary>
    ///     The uppercase converter.
    /// </summary>
    public class UppercaseConverter : IValueConverter
    {
        /// <summary>
        ///     Converts string to uppercase.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object" />. </returns>
        /// <exception cref="InvalidOperationException"> Is thrown if the input is not a string. </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("The target must be a string");
            }

            return value == null ? string.Empty : Utilities.ToUpper(value.ToString());
        }

        /// <summary>
        ///     The convert back. Empty, just implements the interface.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object" />. </returns>
        /// <exception cref="NotSupportedException"> Is thrown if the input is not a string. </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}