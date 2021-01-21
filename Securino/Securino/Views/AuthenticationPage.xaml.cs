// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationPage.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the AuthenticationPage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Views
{
    using System.Threading.Tasks;

    using Securino.ViewModels;

    using Xamarin.Forms;

    /// <summary>
    ///     The authentication page.
    /// </summary>
    public partial class AuthenticationPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationPage" /> class.
        /// </summary>
        public AuthenticationPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     On appearing, start playing the loading animation and authenticate the user.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // On appearing play the animation
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

            // Get new access token
            ((AuthenticationPageViewModel)this.BindingContext).GetLatestStatusCommand.Execute();
        }

        /// <summary>
        ///     On disappearing, stop the loading animation.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Stops the active animation sequence
            this.AbortAnimation("LoadingAnimation");
        }
    }
}