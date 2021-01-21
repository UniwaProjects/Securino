// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageResourceExtension.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ImageResourceExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.ExtensionsXAML
{
    using System;
    using System.Reflection;

    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     The image resource extension.
    /// </summary>
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///     Returns the image resource.
        /// </summary>
        /// <param name="serviceProvider"> The service provider. </param>
        /// <returns> The <see cref="object" />. </returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Source == null
                       ? null
                       : Forms9Patch.ImageSource.FromResource(string.Format(Constants.ImagePath, this.Source), typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }
    }
}