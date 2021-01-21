// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRendererResolver.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the IRendererResolver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.CustomRenderers
{
    using Xamarin.Forms;

    /// <summary>
    ///     The RendererResolver interface.
    /// </summary>
    public interface IRendererResolver
    {
        /// <summary>
        ///     Gets the element renderer from the iOS or Android class.
        /// </summary>
        /// <param name="element"> The element. </param>
        /// <returns> The <see cref="object" />. </returns>
        object GetRenderer(VisualElement element);

        /// <summary>
        ///     Is true if the renderer is initialized or false otherwise.
        ///     Check from the on change event on the specific element.
        /// </summary>
        /// <param name="element"> The element. </param>
        /// <returns> The <see cref="bool" />. </returns>
        bool HasRenderer(VisualElement element);
    }
}