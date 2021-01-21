// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartWatchdogService.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the IStartWatchdogService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Services
{
    /// <summary>
    ///     The StartWatchdogService interface.
    /// </summary>
    public interface IStartWatchdogService
    {
        /// <summary>
        ///     The start foreground watchdog service compat.
        /// </summary>
        void StartForegroundWatchdogServiceCompat();
    }
}