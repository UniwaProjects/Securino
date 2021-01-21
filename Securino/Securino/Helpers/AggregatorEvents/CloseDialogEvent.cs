// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseDialogEvent.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ProgressCompletedEvent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.AggregatorEvents
{
    using Prism.Events;

    /// <summary>
    ///     The progress completed event, used to turn off the progress dialog without
    /// </summary>
    public class CloseDialogEvent : PubSubEvent<uint>
    {
    }
}