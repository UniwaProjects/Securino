// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InverseBooleanConverter.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the InverseBooleanConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.Converters
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    /// <summary>
    ///     The inverse boolean converter.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        ///     Converts the value to inverse bool.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object" />. </returns>
        /// <exception cref="InvalidOperationException"> Is thrown for non bool input. </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("The target must be a boolean");
            }

            return !(bool)value;
        }

        /// <summary>
        ///     The convert back. Empty, just implements the interface.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object" />. </returns>
        /// <exception cref="NotSupportedException"> Is thrown for non bool input. </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}