// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the App type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino
{
    using Prism;
    using Prism.Ioc;
    using Prism.Navigation;

    using Securino.Dialogs.ViewModels;
    using Securino.Dialogs.Views;
    using Securino.Models;
    using Securino.Services;
    using Securino.ViewModels;
    using Securino.Views;

    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    ///     The app.
    /// </summary>
    public partial class App
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        /// <param name="initializer"> The initializer. </param>
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        /// <summary>
        ///     The on initialized.
        /// </summary>
        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += this.Connectivity_ConnectivityChanged;

            // Initialize token
            await Ubidots.Instance().LoadTokenAsync();

            // Start the watchdog service
            DependencyService.Get<IStartWatchdogService>().StartForegroundWatchdogServiceCompat();

            await this.NavigationService.NavigateAsync("NavigationPage/" + nameof(AuthenticationPage));
        }

        /// <summary>
        ///     The on resume. Return to root.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            this.NavigationService.GoBackToRootAsync();
        }

        /// <summary>
        ///     The register types.
        /// </summary>
        /// <param name="containerRegistry"> The container registry. </param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Navigation Pages
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<AuthenticationPage, AuthenticationPageViewModel>();
            containerRegistry.RegisterForNavigation<AlarmPage, AlarmPageViewModel>();

            // Dialogs
            containerRegistry.RegisterDialog<ProgressDialog, ProgressDialogViewModel>();
            containerRegistry.RegisterDialog<ToastDialog, ToastDialogViewModel>();
        }

        /// <summary>
        ///     The connectivity_ connectivity changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            await this.NavigationService.GoBackToRootAsync();
        }
    }
}