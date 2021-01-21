// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToastDialogViewModel.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ToastDialogViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Dialogs.ViewModels
{
    using System.Threading.Tasks;

    using Prism.AppModel;
    using Prism.Events;

    using Securino.Helpers.AggregatorEvents;

    /// <summary>
    ///     The toast dialog view model.
    /// </summary>
    public class ToastDialogViewModel : DialogBase
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
        ///     The close timer.
        /// </summary>
        private int closeTimer;

        /// <summary>
        ///     The is error toast. Error toasts are painted different and vibrate longer.
        /// </summary>
        private bool isErrorToast;

        /// <summary>
        ///     The toast text.
        /// </summary>
        private string toastText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToastDialogViewModel" /> class.
        /// </summary>
        /// <param name="eventAggregator"> The event aggregator. </param>
        public ToastDialogViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.dialogId = DialogBase.LatestDialogId;

            // Get the event and subscribe
            this.closeDialogEvent = this.EventAggregator.GetEvent<CloseDialogEvent>();
            this.closeDialogEvent.Subscribe(this.CloseProgressDialog);
        }

        /// <summary>
        ///     Gets or sets the close timer.
        /// </summary>
        [AutoInitialize(true)]
        public int CloseTimer
        {
            get => this.closeTimer;
            set =>
                this.SetProperty(
                    ref this.closeTimer,
                    value,
                    async () =>
                        {
                            // If timer is zero, no point on starting a timer
                            if (value == 0)
                            {
                                return;
                            }

                            // Otherwise delay seconds before auto closing
                            await Task.Delay(value);
                            this.CloseProgressDialog(this.dialogId);
                        });
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is error toast.
        /// </summary>
        public bool IsErrorToast
        {
            get => this.isErrorToast;
            set => this.SetProperty(ref this.isErrorToast, value);
        }

        /// <summary>
        ///     Gets or sets the toast text.
        /// </summary>
        [AutoInitialize(true)]
        public string ToastText
        {
            get => this.toastText;
            set => this.SetProperty(ref this.toastText, value);
        }

        /// <summary>
        ///     Close progress dialog and unsubscribe from event.
        /// </summary>
        /// <param name="id"> The id. </param>
        private async void CloseProgressDialog(uint id)
        {
            if (this.dialogId != id)
            {
                return;
            }

            this.closeDialogEvent.Unsubscribe(this.CloseProgressDialog);
            this.Close(null);
            await Task.Delay(500);
        }
    }
}