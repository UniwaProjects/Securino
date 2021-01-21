// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the Utilities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    ///     The utilities class.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        ///     The get image source from image name.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <returns> The <see cref="ImageSource" />. </returns>
        public static ImageSource GetImageSource(string name)
        {
            return Forms9Patch.ImageSource.FromResource(
                string.Format(Constants.ImagePath, name),
                typeof(Utilities).GetTypeInfo().Assembly);
        }

        /// <summary>
        ///     Vibrates phone for seconds, if phone supports it.
        /// </summary>
        public static void LongVibration()
        {
            try
            {
                // Use default vibration length
                Vibration.Vibrate(Constants.LongVibrationDuration);
            }
            catch
            {
                // Do no vibrate
            }
        }

        /// <summary>
        ///     Vibrates phone for seconds, if phone supports it.
        /// </summary>
        public static void ShortVibration()
        {
            try
            {
                // Use default vibration length
                Vibration.Vibrate(Constants.ShortVibrationDuration);
            }
            catch
            {
                // Do no vibrate
            }
        }

        /// <summary>
        ///     Returns the string to uppercase using Greek uppercase rules.
        /// </summary>
        /// <param name="source"> The string that will be converted to uppercase </param>
        /// <returns> The uppercase text. </returns>
        public static string ToUpper(this string source)
        {
            source = source.ToUpper(CultureInfo.CurrentCulture);
            if (!CultureInfo.CurrentCulture.ToString().Equals("el-GR"))
            {
                return source;
            }

            Dictionary<char, char> mappings = new Dictionary<char, char>
                                                  {
                                                      { 'Ά', 'Α' },
                                                      { 'Έ', 'Ε' },
                                                      { 'Ή', 'Η' },
                                                      { 'Ί', 'Ι' },
                                                      { 'Ό', 'Ο' },
                                                      { 'Ύ', 'Υ' },
                                                      { 'Ώ', 'Ω' }
                                                  };

            char[] result = new char[source.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = mappings.ContainsKey(source[i]) ? mappings[source[i]] : source[i];
            }

            return new string(result);
        }

        /// <summary>
        ///     The unix time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp"> The unix time stamp. </param>
        /// <returns> The <see cref="DateTime" />. </returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}