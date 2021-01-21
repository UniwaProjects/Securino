// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationPageViewModel.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the AuthenticationPageViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.ViewModels
{
    using System.Threading.Tasks;

    using Prism.Commands;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services.Dialogs;

    using Securino.Helpers;
    using Securino.Models;
    using Securino.Views;

    using Xamarin.Essentials;

    /// <summary>
    ///     The authentication page view model.
    /// </summary>
    public class AuthenticationPageViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationPageViewModel" /> class.
        /// </summary>
        /// <param name="navigationService">
        ///     The navigation service.
        /// </param>
        /// <param name="dialogService">
        ///     The dialog service.
        /// </param>
        /// <param name="eventAggregator">
        ///     The event aggregator.
        /// </param>
        public AuthenticationPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : base(navigationService, dialogService, eventAggregator)
        {
            // Initialize the get latest status command
            this.GetLatestStatusCommand = new DelegateCommand(async () => await this.GetLatestStatusCommandExecute());
        }

        /// <summary>
        ///     Gets the get latest status command.
        /// </summary>
        public DelegateCommand GetLatestStatusCommand { get; }

        /// <summary>
        ///     The refresh token.
        /// </summary>
        /// <returns> The <see cref="Task" />. </returns>
        private async Task GetLatestStatusCommandExecute()
        {
            // Try to get the status until it succeeds
            while (true)
            {
                RequestResult result = await Ubidots.Instance().GetLatestState();
                switch (result)
                {
                    case RequestResult.Ok:
                        // Proceed to next page
                        await this.NavigationService.NavigateAsync($"{nameof(AlarmPage)}");
                        return;

                    case RequestResult.ServerError:
                        // Show an error with a small delay
                        await Task.Delay(500);
                        await this.ShowServerErrorDialogAsync();
                        return;

                    case RequestResult.NetworkError:
                        // Show an error with a small delay
                        await Task.Delay(500);
                        await this.ShowNetworkErrorDialogAsync();
                        return;
                }
            }
        }
    }
}