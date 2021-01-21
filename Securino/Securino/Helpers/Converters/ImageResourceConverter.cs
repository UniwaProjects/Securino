// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageResourceConverter.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   The image resource converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.Converters
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    /// <summary>
    ///     The image resource converter.
    /// </summary>
    public class ImageResourceConverter : IValueConverter
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
            if (targetType != typeof(ImageSource))
            {
                throw new InvalidOperationException("The target must be an Image Source");
            }

            if (value == null)
            {
                return value;
            }

            return Utilities.GetImageSource((string)value);
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