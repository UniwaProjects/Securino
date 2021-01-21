// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetValueWrapper.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the GetValueWrapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers.Wrappers
{
    /// <summary>
    ///     The get value wrapper.
    /// </summary>
    public class GetValueWrapper
    {
        /// <summary>
        ///     Gets or sets the last_activity.
        /// </summary>
        public long last_activity { get; set; }

        /// <summary>
        ///     Gets or sets the last_value.
        /// </summary>
        public LastValue last_value { get; set; }

        /// <summary>
        ///     The last value.
        /// </summary>
        public class LastValue
        {
            /// <summary>
            ///     Gets or sets the timestamp.
            /// </summary>
            public long timestamp { get; set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            public double value { get; set; }
        }
    }
}