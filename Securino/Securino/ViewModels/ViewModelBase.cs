// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ViewModelBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.ViewModels
{
    using System;
    using System.Threading.Tasks;

    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Navigation;
    using Prism.Services.Dialogs;

    using Securino.Dialogs.ViewModels;
    using Securino.Dialogs.Views;
    using Securino.Helpers;
    using Securino.Helpers.AggregatorEvents;
    using Securino.Helpers.Extensions;
    using Securino.Resources;

    using Xamarin.Forms;

    /// <summary>
    ///     The view model base.
    /// </summary>
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        /// <summary>
        ///     The is command running.
        /// </summary>
        private bool isCommandRunning;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelBase" /> class.
        /// </summary>
        /// <param name="navigationService"> The navigation service. </param>
        /// <param name="dialogService"> The dialog service. </param>
        /// <param name="eventAggregator"> The event aggregator. </param>
        public ViewModelBase(
            INavigationService navigationService,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
        {
            this.NavigationService = navigationService;
            this.DialogService = dialogService;
            this.EventAggregator = eventAggregator;
        }

        /// <summary>
        ///     Gets a value indicating whether is command running.
        /// </summary>
        public bool IsCommandRunning
        {
            get => this.isCommandRunning;
            private set => this.SetProperty(ref this.isCommandRunning, value);
        }

        /// <summary>
        ///     Gets the dialog service.
        /// </summary>
        protected IDialogService DialogService { get; }

        /// <summary>
        ///     Gets the event aggregator.
        /// </summary>
        protected IEventAggregator EventAggregator { get; }

        /// <summary>
        ///     Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService { get; }

        /// <summary>
        ///     Guards the command execution via the IsCommandRunning flag, allowing the async commands
        ///     to not run at the same time and giving the IsCommandRunning status to bind to the
        ///     can execute changed of the commands.
        /// </summary>
        /// <param name="command"> The command. </param>
        /// <returns> The <see cref="Task" />. </returns>
        public async Task CommandRun(Func<Task> command)
        {
            if (this.IsCommandRunning)
            {
                return;
            }

            this.IsCommandRunning = true;

            await command();

            this.IsCommandRunning = false;
        }

        /// <summary>
        ///     The destroy.
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        ///     The initialize event.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        /// <summary>
        ///     The on navigated from.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        /// <summary>
        ///     The on navigated to.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        /// <summary>
        ///     The send request with progress.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="Task" />. </returns>
        public async Task<RequestResult> SendRequestWithProgress(Func<Task<RequestResult>> request)
        {
            // Send the request while showing a progress dialog
            Device.BeginInvokeOnMainThread(() => this.DialogService.ShowDialog($"{nameof(ProgressDialog)}"));
            uint dialogId = DialogBase.LatestDialogId;

            RequestResult result = await request();

            this.EventAggregator.GetEvent<CloseDialogEvent>().Publish(dialogId);

            return result;
        }

        /// <summary>
        ///     The show network error dialog async.
        /// </summary>
        /// <returns> The <see cref="Task" />. </returns>
        public async Task ShowNetworkErrorAndRestart()
        {
            // Show toast dialog
            if (this.DialogService != null)
            {
                await this.ShowNetworkErrorDialogAsync();
            }

            // Go back to root
            if (this.NavigationService != null)
            {
                await this.NavigationService.GoBackToRootAsync();
            }
        }

        /// <summary>
        ///     The show server error dialog async.
        /// </summary>
        /// <returns> The <see cref="Task" />. </returns>
        public Task<IDialogResult> ShowNetworkErrorDialogAsync()
        {
            return this.DialogService.ShowDialogAsync(
                $"{nameof(ToastDialog)}",
                new DialogParameters
                    {
                        { "toastText", AppResources.noNetwork },
                        { "closeTimer", Constants.LongToastMillis },
                        { "isErrorToast", true }
                    });
        }

        /// <summary>
        ///     The show server error dialog async.
        /// </summary>
        /// <returns> The <see cref="Task" />. </returns>
        public async Task ShowServerErrorAndRestart()
        {
            // Show toast dialog
            if (this.DialogService != null)
            {
                await this.ShowServerErrorDialogAsync();
            }

            // Go back to root
            if (this.NavigationService != null)
            {
                await this.NavigationService.GoBackToRootAsync();
            }
        }

        /// <summary>
        ///     The show server error dialog async.
        /// </summary>
        /// <returns> The <see cref="Task" />. </returns>
        public Task<IDialogResult> ShowServerErrorDialogAsync()
        {
            return this.DialogService.ShowDialogAsync(
                $"{nameof(ToastDialog)}",
                new DialogParameters
                    {
                        { "toastText", AppResources.serverError },
                        { "closeTimer", Constants.LongToastMillis },
                        { "isErrorToast", true }
                    });
        }
    }
}