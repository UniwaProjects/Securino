// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogExtensions.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the DialogExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.Extensions
{
    using System;
    using System.Threading.Tasks;

    using Prism.Services.Dialogs;

    /// <summary>
    /// Prism dialog extensions.
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// Shows dialog async without parameters.
        /// </summary>
        /// <param name="dialogService"> The dialog service. </param>
        /// <param name="dialogName"> The dialog dialogName. </param>
        /// <returns> IDialogResult result. </returns>
        public static Task<IDialogResult> ShowDialogAsync(this IDialogService dialogService, string dialogName)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();

            try
            {
                dialogService.ShowDialog(dialogName, result => { tcs.SetResult(result); });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Show dialog async with parameters.
        /// </summary>
        /// <param name="dialogService"> The dialog service. </param>
        /// <param name="dialogName"> The dialog name. </param>
        /// <param name="parameters"> The parameters. </param>
        /// <returns> IDialogResult result. </returns>
        public static Task<IDialogResult> ShowDialogAsync(
            this IDialogService dialogService,
            string dialogName,
            IDialogParameters parameters)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();

            try
            {
                dialogService.ShowDialog(
                    dialogName,
                    parameters,
                    result =>
                        {
                            if (result.Exception != null)
                            {
                                tcs.SetException(result.Exception);
                                return;
                            }

                            tcs.SetResult(result);
                        });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Shows dialog async without parameters.
        /// </summary>
        /// <typeparam name="T"> Type of returned result. </typeparam>
        /// <param name="dialogService"> The dialog service. </param>
        /// <param name="dialogName"> The dialog dialogName. </param>
        /// <returns> Type T result. </returns>
        public static Task<T> ShowDialogAsync<T>(this IDialogService dialogService, string dialogName)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            try
            {
                dialogService.ShowDialog(
                    dialogName,
                    result =>
                        {
                            if (result.Exception != null)
                            {
                                tcs.SetException(result.Exception);
                                return;
                            }

                            tcs.SetResult(result.Parameters.GetValue<T>(typeof(T).Name));
                        });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}