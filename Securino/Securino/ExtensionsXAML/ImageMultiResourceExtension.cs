// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageMultiResourceExtension.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ImageMultiResourceExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.ExtensionsXAML
{
    using System;

    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    using ImageSource = Forms9Patch.ImageSource;

    /// <summary>
    ///     The image multi resource extension. Fetches images for 9 patch with size matching
    ///     the resolution of the device.
    /// </summary>
    [ContentProperty(nameof(Source))]
    public class ImageMultiResourceExtension : IMarkupExtension
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
                       : ImageSource.FromMultiResource(string.Format(Constants.ImagePath, this.Source));
        }
    }
}