// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslateExtension.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Used for providing the XAML with translated strings
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.ExtensionsXAML
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     Used for providing the XAML with translated strings
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        /// <summary>
        ///     The resource manager.
        /// </summary>
        private static readonly Lazy<ResourceManager> ResourceManager = new Lazy<ResourceManager>(
            () => new ResourceManager(
                "Securino.Resources.AppResources",
                typeof(TranslateExtension).GetTypeInfo().Assembly));

        /// <summary>
        ///     The culture of the app, resource will be for that culture.
        /// </summary>
        private readonly CultureInfo culture;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TranslateExtension" /> class.
        /// </summary>
        public TranslateExtension()
        {
            this.culture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is uppercase.
        /// </summary>
        public bool IsUppercase { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Fetch the given value from the resource matching the culture.
        /// </summary>
        /// <param name="serviceProvider"> The service provider. </param>
        /// <returns> The <see cref="object" />. </returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            // If the value given is null, return empty
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return string.Empty;
            }

            // Get translation
            string translation = ResourceManager.Value.GetString(this.Name, this.culture);

            // If uppercase
            translation = this.IsUppercase ? Utilities.ToUpper(translation) : translation;

            // If no translation exists, return empty
            return string.IsNullOrEmpty(translation) ? string.Empty : translation;
        }
    }
}