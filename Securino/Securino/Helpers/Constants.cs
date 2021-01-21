// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the RequestResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers
{
    /// <summary>
    ///     The web service request result.
    /// </summary>
    public enum RequestResult
    {
        Ok,

        ServerError,

        NetworkError
    }

    /// <summary>
    ///     Constants class for everything.
    /// </summary>
    public class Constants
    {
        /// <summary>
        ///     The image path.
        /// </summary>
        public const string ImagePath = "Securino.Assets.Images.{0}";

        /// <summary>
        ///     The long toast millis.
        /// </summary>
        public const uint LongToastMillis = 3000;

        /// <summary>
        ///     The long vibration duration.
        /// </summary>
        public const double LongVibrationDuration = 300;

        /// <summary>
        ///     The secure storage password.
        /// </summary>
        public const string SecureStoragePassword = "password";

        /// <summary>
        ///     The secure storage token.
        /// </summary>
        public const string SecureStorageToken = "token";

        /// <summary>
        ///     The short toast millis.
        /// </summary>
        public const uint ShortToastMillis = 2000;

        /// <summary>
        ///     The short vibration duration.
        /// </summary>
        public const double ShortVibrationDuration = 100;
    }
}