// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressDialog.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ProgressDialog type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Dialogs.Views
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Securino.CustomRenderers;

    using Xamarin.Forms;

    /// <summary>
    ///     The progress dialog.
    /// </summary>
    public partial class ProgressDialog
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressDialog" /> class.
        /// </summary>
        public ProgressDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     Starts playing the loading animation.
        ///     On renderer unload, stop the animation.
        /// </summary>
        /// <param name="propertyName"> The property Name. </param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (!propertyName.Equals("Renderer", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (DependencyService.Get<IRendererResolver>().HasRenderer(this))
            {
                // Start animation
                Animation animation = new Animation(v => this.LoadingImage.Rotation = v, 0, 360);
                animation.Commit(
                    this,
                    "LoadingAnimation",
                    16,
                    1000,
                    Easing.SinInOut,
                    async (v, c) =>
                        {
                            await Task.Delay(250);
                            this.LoadingImage.Rotation = 0;
                        },
                    () => true);
            }
            else
            {
                this.AbortAnimation("LoadingAnimation");
            }
        }
    }
}