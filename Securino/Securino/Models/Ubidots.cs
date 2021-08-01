// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ubidots.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the Ubidots type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Models
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Prism.Mvvm;

    using Securino.Helpers;
    using Securino.Helpers.Wrappers;
    using Securino.Resources;

    using Xamarin.Essentials;

    /// <summary>
    ///     The ubidots.
    /// </summary>
    public class Ubidots : BindableBase
    {
        /// <summary>
        ///     The max update interval. The device updates its status every 120 seconds,
        ///     so with 180 seconds, there is a minute to spare for any network related delay.
        /// </summary>
        private const long MaxUpdateInterval = 180 * 1000;

        /// <summary>
        ///     The auth token.
        /// </summary>
        private static string authToken = string.Empty;

        /// <summary>
        ///     The instance.
        /// </summary>
        private static Ubidots instance;

        /// <summary>
        ///     The is alarm triggered.
        /// </summary>
        private bool isAlarmTriggered;

        /// <summary>
        ///     The is arm away.
        /// </summary>
        private bool isArmAway;

        /// <summary>
        ///     The is arm stay.
        /// </summary>
        private bool isArmStay;

        /// <summary>
        ///     The is disarmed.
        /// </summary>
        private bool isDisarmed;

        /// <summary>
        ///     The is magnet sensor triggered.
        /// </summary>
        private bool isMagnetSensorTriggered;

        /// <summary>
        ///     The is online.
        /// </summary>
        private bool isOnline;

        /// <summary>
        ///     The is pir sensor triggered.
        /// </summary>
        private bool isPirSensorTriggered;

        /// <summary>
        ///     The is sensor offline.
        /// </summary>
        private bool isSensorOffline;

        /// <summary>
        ///     The last activity.
        /// </summary>
        private long lastActivity;

        /// <summary>
        ///     The last updated.
        /// </summary>
        private string lastUpdated;

        /// <summary>
        ///     The state changed.
        /// </summary>
        private bool stateChanged;

        /// <summary>
        ///     Gets or sets a value indicating whether is alarm triggered.
        /// </summary>
        public bool IsAlarmTriggered
        {
            get => this.isAlarmTriggered;
            set => this.SetProperty(ref this.isAlarmTriggered, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is arm away.
        /// </summary>
        public bool IsArmAway
        {
            get => this.isArmAway;
            set => this.SetProperty(ref this.isArmAway, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is arm stay.
        /// </summary>
        public bool IsArmStay
        {
            get => this.isArmStay;
            set => this.SetProperty(ref this.isArmStay, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is disarmed.
        /// </summary>
        public bool IsDisarmed
        {
            get => this.isDisarmed;
            set => this.SetProperty(ref this.isDisarmed, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is magnet sensor triggered.
        /// </summary>
        public bool IsMagnetSensorTriggered
        {
            get => this.isMagnetSensorTriggered;
            set => this.SetProperty(ref this.isMagnetSensorTriggered, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is online.
        /// </summary>
        public bool IsOnline
        {
            get => this.isOnline;
            set => this.SetProperty(ref this.isOnline, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is pir sensor triggered.
        /// </summary>
        public bool IsPirSensorTriggered
        {
            get => this.isPirSensorTriggered;
            set => this.SetProperty(ref this.isPirSensorTriggered, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is sensor offline.
        /// </summary>
        public bool IsSensorOffline
        {
            get => this.isSensorOffline;
            set => this.SetProperty(ref this.isSensorOffline, value);
        }

        /// <summary>
        ///     Gets or sets the last updated.
        /// </summary>
        public string LastUpdated
        {
            get => this.lastUpdated;
            set => this.SetProperty(ref this.lastUpdated, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether state changed.
        /// </summary>
        public bool StateChanged
        {
            get => this.stateChanged;
            set => this.SetProperty(ref this.stateChanged, value);
        }

        /// <summary>
        ///     The instance.
        /// </summary>
        /// <returns>
        ///     The <see cref="Ubidots" />.
        /// </returns>
        public static Ubidots Instance()
        {
            return instance ?? (instance = new Ubidots());
        }

        /// <summary>
        ///     The arm away async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<RequestResult> ArmAwayAsync()
        {
            return await this.ChangeValues(Values.State.Armed, Values.Method.ArmAway, Values.Sensor.None);
        }

        /// <summary>
        ///     The arm stay async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<RequestResult> ArmStayAsync()
        {
            return await this.ChangeValues(Values.State.Armed, Values.Method.ArmStay, Values.Sensor.None);
        }

        /// <summary>
        ///     The create token async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<RequestResult> CreateTokenAsync()
        {
            // For no network, return
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                return RequestResult.NetworkError;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Headers
                    client.DefaultRequestHeaders.Add(Headers.ApiKeyHeader, UbidotsVariables.ApiKey);

                    // Request
                    byte[] buffer = Encoding.UTF8.GetBytes(string.Empty);
                    ByteArrayContent byteContent = new ByteArrayContent(buffer);
                    string url = string.Format(Requests.CreateTokenRequest, Requests.UbidotsServer);
                    HttpResponseMessage response = await client.PostAsync(url, byteContent).ConfigureAwait(false);

                    // Answer body
                    string answerBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // Response
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            CreateTokenWrapper answer = JsonConvert.DeserializeObject<CreateTokenWrapper>(answerBody);
                            authToken = answer.token;

                            // Save the new token
                            await SecureStorage.SetAsync(Constants.SecureStorageToken, authToken).ConfigureAwait(false);

                            return RequestResult.Ok;
                        default:
                            return RequestResult.ServerError;
                    }
                }
                catch
                {
                    return RequestResult.ServerError;
                }
            }
        }

        /// <summary>
        ///     The disarm async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<RequestResult> DisarmAsync()
        {
            return await this.ChangeValues(Values.State.Disarmed, Values.Method.None, Values.Sensor.None);
        }

        /// <summary>
        ///     The get latest state of all the variables.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<RequestResult> GetLatestState()
        {
            // Get the state
            (RequestResult result, double value) stateResult = await this.GetValueAsync(UbidotsVariables.StateLabel);
            switch (stateResult.result)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            // Get method
            (RequestResult result, double value) methodResult = await this.GetValueAsync(UbidotsVariables.MethodLabel);
            switch (methodResult.result)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            // Get the sensor
            (RequestResult result, double value) sensorResult = await this.GetValueAsync(UbidotsVariables.SensorLabel);
            switch (sensorResult.result)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            // If the state  is different than the previous
            bool wasDisarmed = this.isDisarmed;
            this.IsDisarmed = stateResult.value == Values.State.Disarmed;
            this.StateChanged = wasDisarmed != this.isDisarmed;

            // Set the variables from the results
            this.IsAlarmTriggered = stateResult.value == Values.State.Triggered;
            this.IsArmStay = methodResult.value == Values.Method.ArmStay;
            this.IsArmAway = methodResult.value == Values.Method.ArmAway;
            this.IsMagnetSensorTriggered = sensorResult.value == Values.Sensor.MagnetTriggered;
            this.IsPirSensorTriggered = sensorResult.value == Values.Sensor.PirTriggered;
            this.IsSensorOffline = sensorResult.value == Values.Sensor.Offline;

            // Get the state
            this.SetIsOnline();
            this.SetLastUpdated();

            // Return ok
            return RequestResult.Ok;
        }

        /// <summary>
        ///     The load token async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task LoadTokenAsync()
        {
            authToken = await SecureStorage.GetAsync(Constants.SecureStorageToken);
        }

        /// <summary>
        ///     The change values.
        /// </summary>
        /// <param name="stateValue">
        ///     The state value.
        /// </param>
        /// <param name="methodValue">
        ///     The method value.
        /// </param>
        /// <param name="sensorValue">
        ///     The sensor value.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task<RequestResult> ChangeValues(double stateValue, double methodValue, double sensorValue)
        {
            // Method first in case the requests are slow. Alarm will arm when the state is changed, not the method.
            RequestResult methodResult = await this.SendValueAsync(UbidotsVariables.MethodLabel, methodValue);
            switch (methodResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            // This will trigger the state change
            RequestResult stateResult = await this.SendValueAsync(UbidotsVariables.StateLabel, stateValue);
            switch (stateResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            // The sensors are of no importance, they just need to be reset
            RequestResult sensorResult = await this.SendValueAsync(UbidotsVariables.SensorLabel, sensorValue);
            switch (sensorResult)
            {
                case RequestResult.Ok:
                    break;
                case RequestResult.ServerError:
                    return RequestResult.ServerError;
                case RequestResult.NetworkError:
                    return RequestResult.NetworkError;
            }

            return RequestResult.Ok;
        }

        /// <summary>
        ///     The get value async.
        /// </summary>
        /// <param name="valueLabel">
        ///     The value label.
        /// </param>
        /// <param name="refreshed">
        ///     The refreshed.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task<(RequestResult result, double value)> GetValueAsync(
            string valueLabel,
            bool refreshed = false)
        {
            // For no network, return
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                return (RequestResult.NetworkError, -1);
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Headers
                    client.DefaultRequestHeaders.Add(Headers.AuthTokenHeader, authToken);

                    // Request
                    string url = string.Format(
                        Requests.GetValueRequest,
                        Requests.UbidotsServer,
                        UbidotsVariables.DeviceLabel,
                        valueLabel);
                    HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);

                    // Answer body
                    string answerBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // Response
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            GetValueWrapper answer = JsonConvert.DeserializeObject<GetValueWrapper>(answerBody);
                            this.lastActivity = answer.last_activity;
                            return (RequestResult.Ok, answer.last_value.value);
                        case HttpStatusCode.Forbidden:
                            // If this is the first call, refresh and resend
                            if (!refreshed)
                            {
                                RequestResult refreshResult = await this.CreateTokenAsync().ConfigureAwait(false);

                                if (refreshResult != RequestResult.Ok)
                                {
                                    return (RequestResult.ServerError, -1);
                                }

                                // Call recursively the same function
                                return await this.GetValueAsync(valueLabel, true).ConfigureAwait(false);
                            }

                            return (RequestResult.ServerError, -1);
                        case HttpStatusCode.Unauthorized:
                            // If this is the first call, refresh and resend
                            if (!refreshed)
                            {
                                RequestResult refreshResult = await this.CreateTokenAsync().ConfigureAwait(false);

                                if (refreshResult != RequestResult.Ok)
                                {
                                    return (RequestResult.ServerError, -1);
                                }

                                // Call recursively the same function
                                return await this.GetValueAsync(valueLabel, true).ConfigureAwait(false);
                            }

                            return (RequestResult.ServerError, -1);
                        default:
                            return (RequestResult.ServerError, -1);
                    }
                }
                catch
                {
                    return (RequestResult.ServerError, -1);
                }
            }
        }

        /// <summary>
        ///     The send value async.
        /// </summary>
        /// <param name="valueLabel">
        ///     The value label.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="refreshed">
        ///     The refreshed.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private async Task<RequestResult> SendValueAsync(string valueLabel, double value, bool refreshed = false)
        {
            // For no network, return
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                return RequestResult.NetworkError;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Headers
                    client.DefaultRequestHeaders.Add(Headers.AuthTokenHeader, authToken);

                    // Request
                    string json = JsonConvert.SerializeObject(new SendValueWrapper { value = value });
                    string url = string.Format(
                        Requests.SendValueRequest,
                        Requests.UbidotsServer,
                        UbidotsVariables.DeviceLabel,
                        valueLabel);
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(json, Encoding.UTF8, Headers.ContentTypeJson);

                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                    // Response
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            return RequestResult.Ok;
                        case HttpStatusCode.Forbidden:
                            // If this is the first call, refresh and resend
                            if (!refreshed)
                            {
                                RequestResult refreshResult = await this.CreateTokenAsync().ConfigureAwait(false);

                                if (refreshResult != RequestResult.Ok)
                                {
                                    return RequestResult.ServerError;
                                }

                                // Call recursively the same function
                                return await this.SendValueAsync(valueLabel, value, true).ConfigureAwait(false);
                            }

                            return RequestResult.ServerError;
                        case HttpStatusCode.Unauthorized:
                            // If this is the first call, refresh and resend
                            if (!refreshed)
                            {
                                RequestResult refreshResult = await this.CreateTokenAsync().ConfigureAwait(false);

                                if (refreshResult != RequestResult.Ok)
                                {
                                    return RequestResult.ServerError;
                                }

                                // Call recursively the same function
                                return await this.SendValueAsync(valueLabel, value, true).ConfigureAwait(false);
                            }

                            return RequestResult.ServerError;
                        default:
                            return RequestResult.ServerError;
                    }
                }
                catch
                {
                    return RequestResult.ServerError;
                }
            }
        }

        /// <summary>
        ///     The set is online. Compares the current millis to those of the last value timestamp.
        /// </summary>
        private void SetIsOnline()
        {
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long millisSinceUpdate = currentTime - this.lastActivity;
            this.IsOnline = millisSinceUpdate < MaxUpdateInterval;
        }

        /// <summary>
        ///     The set last updated. Sets the text of the last value timestamp in a date format.
        /// </summary>
        private void SetLastUpdated()
        {
            DateTime timestamp = Utilities.UnixTimeStampToDateTime(this.lastActivity);

            this.LastUpdated = string.Format(AppResources.lastUpdated, timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        /// <summary>
        ///     The headers.
        /// </summary>
        private static class Headers
        {
            /// <summary>
            ///     The API key header. Needed to get token.
            /// </summary>
            public static readonly string ApiKeyHeader = "x-ubidots-apikey";

            /// <summary>
            ///     The auth token header. Needed to pass token.
            /// </summary>
            public static readonly string AuthTokenHeader = "X-Auth-Token";

            /// <summary>
            ///     The content type json.
            /// </summary>
            public static readonly string ContentTypeJson = "application/json";
        }

        /// <summary>
        ///     The requests.
        /// </summary>
        private static class Requests
        {
            /// <summary>
            ///     The create token request.
            /// </summary>
            public const string CreateTokenRequest = "{0}/auth/token/";

            /// <summary>
            ///     The get value request.
            /// </summary>
            public const string GetValueRequest = "{0}/devices/{1}/{2}/";

            /// <summary>
            ///     The send value request.
            /// </summary>
            public const string SendValueRequest = "{0}/devices/{1}/{2}/values/";

            /// <summary>
            ///     The ubidots server.
            /// </summary>
            public const string UbidotsServer = "https://industrial.api.ubidots.com/api/v1.6";
        }

        /// <summary>
        ///     The ubidots variables.
        /// </summary>
        private static class UbidotsVariables
        {
            /// <summary>
            /// The api key.
            /// </summary>
            public const string ApiKey = "BBFF-663c3a8cc3e86ab5500531f0fec374cba42";

            /// <summary>
            ///     The device label.
            /// </summary>
            public const string DeviceLabel = "securino";

            /// <summary>
            ///     The method label.
            /// </summary>
            public const string MethodLabel = "method";

            /// <summary>
            ///     The sensor label.
            /// </summary>
            public const string SensorLabel = "sensor";

            /// <summary>
            ///     The state label.
            /// </summary>
            public const string StateLabel = "state";
        }

        /// <summary>
        ///     The values.
        /// </summary>
        private static class Values
        {
            /// <summary>
            ///     The method.
            /// </summary>
            public static class Method
            {
                /// <summary>
                ///     The arm away.
                /// </summary>
                public const double ArmAway = 2.0;

                /// <summary>
                ///     The arm stay.
                /// </summary>
                public const double ArmStay = 1.0;

                /// <summary>
                ///     The none.
                /// </summary>
                public const double None = 0.0;
            }

            /// <summary>
            ///     The sensor.
            /// </summary>
            public static class Sensor
            {
                /// <summary>
                ///     The magnet triggered.
                /// </summary>
                public const double MagnetTriggered = 2.0;

                /// <summary>
                ///     The none.
                /// </summary>
                public const double None = 0.0;

                /// <summary>
                ///     The offline.
                /// </summary>
                public const double Offline = 3.0;

                /// <summary>
                ///     The pir triggered.
                /// </summary>
                public const double PirTriggered = 1.0;
            }

            /// <summary>
            ///     The state.
            /// </summary>
            public static class State
            {
                /// <summary>
                ///     The armed.
                /// </summary>
                public const double Armed = 1.0;

                /// <summary>
                ///     The disarmed.
                /// </summary>
                public const double Disarmed = 0.0;

                /// <summary>
                ///     The triggered.
                /// </summary>
                public const double Triggered = 2.0;
            }
        }
    }
}