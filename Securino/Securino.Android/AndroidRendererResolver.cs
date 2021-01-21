// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndroidRendererResolver.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved// </copyright>
// <summary>
//   Defines the AndroidRendererResolver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Securino.Droid;

using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidRendererResolver))]

namespace Securino.Droid
{
    using Securino.CustomRenderers;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    ///     The android renderer resolver.
    /// </summary>
    public class AndroidRendererResolver : IRendererResolver
    {
        /// <summary>
        ///     The get renderer.
        /// </summary>
        /// <param name="element">
        ///     The element.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public object GetRenderer(VisualElement element)
        {
            return Platform.GetRenderer(element);
        }

        /// <summary>
        ///     The has renderer.
        /// </summary>
        /// <param name="element">
        ///     The element.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool HasRenderer(VisualElement element)
        {
            return this.GetRenderer(element) != null;
        }
    }
}