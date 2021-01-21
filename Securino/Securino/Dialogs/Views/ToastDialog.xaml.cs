// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToastDialog.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ToastDialog type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Dialogs.Views
{
    using System;
    using System.Runtime.CompilerServices;

    using Securino.CustomRenderers;
    using Securino.Dialogs.ViewModels;
    using Securino.Helpers;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    ///     The toast dialog.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToastDialog
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ToastDialog" /> class.
        /// </summary>
        public ToastDialog()
        {
            this.InitializeComponent();
            this.RootObject.Scale = 0;
        }

        /// <summary>
        ///     Animates the popup by resetting its scale to 1.
        /// </summary>
        protected virtual async void Animate()
        {
            await this.RootObject.ScaleTo(1, 500, Easing.CubicIn);

            // Vibration duration is longer for errors
            if (((ToastDialogViewModel)this.BindingContext).IsErrorToast)
            {
                Utilities.LongVibration();
            }
            else
            {
                Utilities.ShortVibration();
            }
        }

        /// <summary>
        ///     Animates the view during the rendering.
        /// </summary>
        /// <param name="propertyName"> The property Name. </param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (!propertyName.Equals("Renderer", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!DependencyService.Get<IRendererResolver>().HasRenderer(this))
            {
                return;
            }

            // If this is an error toast, paint it red
            if (((ToastDialogViewModel)this.BindingContext).IsErrorToast)
            {
                this.Background.BackgroundColor = (Color)Application.Current.Resources["ErrorColor"];
            }

            this.Animate();
        }
    }
}