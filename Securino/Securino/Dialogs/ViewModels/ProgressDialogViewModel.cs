// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressDialogViewModel.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ProgressDialogViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Dialogs.ViewModels
{
    using System.Threading.Tasks;

    using Prism.Events;

    using Securino.Helpers.AggregatorEvents;

    /// <summary>
    ///     The progress dialog view model.
    /// </summary>
    public class ProgressDialogViewModel : DialogBase
    {
        /// <summary>
        ///     The progress completed event.
        /// </summary>
        private readonly CloseDialogEvent closeDialogEvent;

        /// <summary>
        ///     The dialog id.
        /// </summary>
        private readonly uint dialogId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressDialogViewModel" /> class.
        /// </summary>
        /// <param name="eventAggregator"> The event aggregator. </param>
        public ProgressDialogViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.dialogId = LatestDialogId;

            // Get the event and subscribe
            this.closeDialogEvent = this.EventAggregator.GetEvent<CloseDialogEvent>();
            this.closeDialogEvent.Subscribe(this.CloseProgressDialog);
        }

        /// <summary>
        ///     Close progress dialog and unsubscribe from event.
        /// </summary>
        /// <param name="id"> The dialog id. </param>
        private async void CloseProgressDialog(uint id)
        {
            if (this.dialogId != id)
            {
                return;
            }

            // Wait a second so that the popup won't just flash
            this.closeDialogEvent.Unsubscribe(this.CloseProgressDialog);
            this.Close(null);
            await Task.Delay(500);
        }
    }
}