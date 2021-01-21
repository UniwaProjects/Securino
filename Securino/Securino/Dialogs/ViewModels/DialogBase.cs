// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogBase.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the DialogBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Dialogs.ViewModels
{
    using System;

    using Prism.AppModel;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    /// <summary>
    ///     The dialog base.
    /// </summary>
    public class DialogBase : BindableBase, IDialogAware, IAutoInitialize
    {
        /// <summary>
        ///     The dialog id.
        /// </summary>
        private static uint latestDialogId;

        /// <summary>
        ///     True if a command is running.
        /// </summary>
        private bool isCommandRunning;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogBase" /> class.
        /// </summary>
        public DialogBase()
        {
            latestDialogId++;
            this.ExitCommand = new DelegateCommand(() => this.Close(null));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogBase" /> class.
        /// </summary>
        /// <param name="eventAggregator"> The event aggregator. </param>
        public DialogBase(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
        }

        /// <summary>
        ///     The request close.
        /// </summary>
        public event Action<IDialogParameters> RequestClose;

        /// <summary>
        ///     The latest latest dialog id.
        /// </summary>
        public static uint LatestDialogId => latestDialogId;

        /// <summary>
        ///     Gets the exit command.
        /// </summary>
        public DelegateCommand ExitCommand { get; }

        /// <summary>
        ///     Gets the event aggregator.
        /// </summary>
        protected IEventAggregator EventAggregator { get; }

        /// <summary>
        ///     The can close dialog.
        /// </summary>
        /// <returns> True unless override. />. </returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        ///     Closes the dialog.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public void Close(IDialogParameters parameters)
        {
            if (this.OtherCommandExecutes())
            {
                return;
            }

            this.RequestClose?.Invoke(parameters);

            this.FinishCommandExecute();
        }

        /// <summary>
        ///     Interface implementation, triggers on close.
        /// </summary>
        public virtual void OnDialogClosed()
        {
        }

        /// <summary>
        ///     Interface implementation, triggers on dialog open.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }

        /// <summary>
        ///     The finish command execute, command guard disable.
        /// </summary>
        protected void FinishCommandExecute()
        {
            this.isCommandRunning = false;
        }

        /// <summary>
        ///     The other command executes command guard check.
        /// </summary>
        /// <returns> If another command executes. </returns>
        protected bool OtherCommandExecutes()
        {
            if (this.isCommandRunning)
            {
                return true;
            }

            this.isCommandRunning = true;
            return false;
        }
    }
}