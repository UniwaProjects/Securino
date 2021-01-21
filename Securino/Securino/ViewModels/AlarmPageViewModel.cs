// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlarmPageViewModel.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the AlarmPageViewModel type.
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

    /// <summary>
    ///     The alarm page view model.
    /// </summary>
    public class AlarmPageViewModel : ViewModelBase
    {
        /// <summary>
        ///     The ubidots model.
        /// </summary>
        private Ubidots ubidotsModel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlarmPageViewModel" /> class.
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
        public AlarmPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : base(navigationService, dialogService, eventAggregator)
        {
            // Get instance
            this.UbidotsModel = Ubidots.Instance();

            // Initialize commands
            this.ArmAwayCommand = new DelegateCommand(
                    async () => await this.CommandRun(this.ArmAwayCommandExecute),
                    () => !this.IsCommandRunning && this.UbidotsModel.IsDisarmed)
                .ObservesProperty(() => this.IsCommandRunning).ObservesProperty(() => this.UbidotsModel.IsDisarmed);

            this.ArmStayCommand = new DelegateCommand(
                    async () => await this.CommandRun(this.ArmStayCommandExecute),
                    () => !this.IsCommandRunning && this.UbidotsModel.IsDisarmed)
                .ObservesProperty(() => this.IsCommandRunning).ObservesProperty(() => this.UbidotsModel.IsDisarmed);

            this.DisarmCommand = new DelegateCommand(
                    async () => await this.CommandRun(this.DisarmCommandExecute),
                    () => !this.IsCommandRunning && !this.UbidotsModel.IsDisarmed)
                .ObservesProperty(() => this.IsCommandRunning).ObservesProperty(() => this.UbidotsModel.IsDisarmed);

            this.RefreshCommand = new DelegateCommand(
                async () => await this.CommandRun(this.RefreshCommandExecute),
                () => !this.IsCommandRunning).ObservesProperty(() => this.IsCommandRunning);
        }

        /// <summary>
        ///     Gets the arm away command.
        /// </summary>
        public DelegateCommand ArmAwayCommand { get; }

        /// <summary>
        ///     Gets the arm stay command.
        /// </summary>
        public DelegateCommand ArmStayCommand { get; }

        /// <summary>
        ///     Gets the disarm command.
        /// </summary>
        public DelegateCommand DisarmCommand { get; }

        /// <summary>
        ///     Gets the refresh command.
        /// </summary>
        public DelegateCommand RefreshCommand { get; }

        /// <summary>
        ///     Gets or sets the ubidots model.
        /// </summary>
        public Ubidots UbidotsModel
        {
            get => this.ubidotsModel;
            set => this.SetProperty(ref this.ubidotsModel, value);
        }

        /// <summary>
        ///     The arm away command execute.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task ArmAwayCommandExecute()
        {
            // Send the arm away command and handle the result
            RequestResult sendResult = await this.SendRequestWithProgress(() => this.UbidotsModel.ArmAwayAsync());
            switch (sendResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    await this.ShowServerErrorAndRestart();
                    return;
                case RequestResult.NetworkError:
                    await this.ShowNetworkErrorAndRestart();
                    return;
            }

            // Then Refresh the state
            await this.RefreshCommandExecute();
        }

        /// <summary>
        ///     The arm stay command execute.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task ArmStayCommandExecute()
        {
            // Send the arm stay command and handle the result
            RequestResult sendResult = await this.SendRequestWithProgress(() => this.UbidotsModel.ArmStayAsync());
            switch (sendResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    await this.ShowServerErrorAndRestart();
                    return;
                case RequestResult.NetworkError:
                    await this.ShowNetworkErrorAndRestart();
                    return;
            }

            // Then Refresh the state
            await this.RefreshCommandExecute();
        }

        /// <summary>
        ///     The disarm command execute.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task DisarmCommandExecute()
        {
            // Send the arm stay command and handle the result
            RequestResult sendResult = await this.SendRequestWithProgress(() => this.UbidotsModel.DisarmAsync());
            switch (sendResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    await this.ShowServerErrorAndRestart();
                    return;
                case RequestResult.NetworkError:
                    await this.ShowNetworkErrorAndRestart();
                    return;
            }

            // Then Refresh the state
            await this.RefreshCommandExecute();
        }

        /// <summary>
        ///     The refresh command execute.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task RefreshCommandExecute()
        {
            // Send the refresh command and handle the result
            RequestResult result = await this.SendRequestWithProgress(() => this.UbidotsModel.GetLatestState());
            switch (result)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    await this.ShowServerErrorAndRestart();
                    return;
                case RequestResult.NetworkError:
                    await this.ShowNetworkErrorAndRestart();
                    return;
            }
        }
    }
}