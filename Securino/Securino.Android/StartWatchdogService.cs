// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartWatchdogService.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the StartWatchdogService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Securino.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(StartWatchdogService))]

namespace Securino.Droid
{
    using Android.Content;
    using Android.OS;

    using Securino.Services;

    /// <summary>
    /// The start watchdog service.
    /// </summary>
    public class StartWatchdogService : IStartWatchdogService
    {
        /// <summary>
        /// The start foreground watchdog service compat.
        /// </summary>
        public void StartForegroundWatchdogServiceCompat()
        {
            // Create and start the intent on the foreground
            Intent intent = new Intent(MainActivity.Instance, typeof(ReadUbidotsStateService));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                MainActivity.Instance.StartForegroundService(intent);
            }
            else
            {
                MainActivity.Instance.StartService(intent);
            }
        }
    }
}