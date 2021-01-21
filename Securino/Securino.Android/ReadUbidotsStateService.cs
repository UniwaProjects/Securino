// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadUbidotsStateService.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ReadUbidotsStateService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Service.Notification;

    using AndroidX.Core.App;

    using Securino.Helpers;
    using Securino.Models;

    /// <summary>
    ///     The read ubidots state service.
    /// </summary>
    [Service]
    public class ReadUbidotsStateService : Service
    {
        /// <summary>
        ///     The action main activity.
        /// </summary>
        public const string ActionMainActivity = "Securino.action.MainActivity";

        /// <summary>
        ///     The service running notification id.
        ///     This is any integer value unique to the application.
        /// </summary>
        public const int ServiceRunningNotificationId = 10000;

        /// <summary>
        ///     The service started key.
        /// </summary>
        public const string ServiceStartedKey = "SECURINO_WATCHDOWN_STARTED";

        /// <summary>
        ///     The max inbox notifications.
        /// </summary>
        private const int MaxInboxNotifications = 6;

        /// <summary>
        ///     The request interval. Gets the new state every 30 seconds.
        /// </summary>
        private const int RequestInterval = 120000;

        /// <summary>
        ///     The context.
        /// </summary>
        private readonly Context context = Application.Context;

        /// <summary>
        ///     The message queue.
        /// </summary>
        private readonly Queue<string> messageQueue;

        /// <summary>
        ///     The inbox style for multiple notifications.
        /// </summary>
        private NotificationCompat.InboxStyle inboxStyle;

        /// <summary>
        ///     The message counter, inbox style can handle up to MaxInboxNotifications.
        /// </summary>
        private int messageCounter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReadUbidotsStateService" /> class.
        /// </summary>
        public ReadUbidotsStateService()
        {
            // Create a new queue and inbox
            this.messageQueue = new Queue<string>();
            this.inboxStyle = new NotificationCompat.InboxStyle();
        }

        /// <summary>
        ///     The on bind.
        /// </summary>
        /// <param name="intent">
        ///     The intent.
        /// </param>
        /// <returns>
        ///     The <see cref="IBinder" />.
        /// </returns>
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        ///     The on start command.
        /// </summary>
        /// <param name="intent">
        ///     The intent.
        /// </param>
        /// <param name="flags">
        ///     The flags.
        /// </param>
        /// <param name="startId">
        ///     The start id.
        /// </param>
        /// <returns>
        ///     The <see cref="StartCommandResult" />.
        /// </returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Launch the watcher task
            this.UbidotsChangesWatcher();

            // Build and show the notifications
            Intent notificationIntent = new Intent(this.context, typeof(MainActivity));
            notificationIntent.SetAction(ActionMainActivity).SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask)
                .PutExtra(ServiceStartedKey, true);

            PendingIntent pendingIntent = PendingIntent.GetActivity(
                this.context,
                0,
                intent,
                PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder notificationBuilder =
                new NotificationCompat.Builder(this.context, MainActivity.ChannelId);

            // Standard notification content
            notificationBuilder.SetContentText(this.Resources.GetText(Resource.String.watchdog_running))
                .SetSmallIcon(Resource.Drawable.icon).SetContentIntent(pendingIntent).SetOngoing(true)
                .SetPriority((int)NotificationPriority.High);

            // Enlist this instance of the service as a foreground service
            this.StartForeground(ServiceRunningNotificationId, notificationBuilder.Build());
            return StartCommandResult.Sticky;
        }

        /// <summary>
        ///     The ubidots changes watcher. Runs continuously
        /// </summary>
        public async void UbidotsChangesWatcher()
        {
            Ubidots ubidots = Ubidots.Instance();

            while (true)
            {
                // Wait before requesting again
                await Task.Delay(RequestInterval);

                // Get the latest state
                RequestResult result = await ubidots.GetLatestState();

                // If the result was not ok, notify the user
                if (result != RequestResult.Ok)
                {
                    this.SendNotification(this.Resources.GetText(Resource.String.request_failed));
                    continue;
                }

                // If not online, notify
                if (!ubidots.IsOnline)
                {
                    this.SendNotification(this.Resources.GetText(Resource.String.alarm_offline));
                    continue;
                }

                // If triggered, notify
                if (ubidots.IsAlarmTriggered)
                {
                    string alarm = this.Resources.GetText(Resource.String.alarm);

                    // Display the specific reason
                    if (ubidots.IsPirSensorTriggered)
                    {
                        this.SendNotification(alarm + this.Resources.GetText(Resource.String.pir_triggered));
                    }
                    else if (ubidots.IsMagnetSensorTriggered)
                    {
                        this.SendNotification(alarm + this.Resources.GetText(Resource.String.magnet_triggered));
                    }
                    else if (ubidots.IsSensorOffline)
                    {
                        this.SendNotification(alarm + this.Resources.GetText(Resource.String.sensor_offline));
                    }
                    else
                    {
                        this.SendNotification(alarm + this.Resources.GetText(Resource.String.invalid_pin));
                    }

                    continue;
                }

                // If the state was changed, notify
                if (ubidots.StateChanged)
                {
                    // Reset the state changed since it was read
                    ubidots.StateChanged = false;

                    // Get the text depending on the new state
                    string arm = ubidots.IsDisarmed
                                     ? this.Resources.GetText(Resource.String.disarmed)
                                     : this.Resources.GetText(Resource.String.arm);

                    string method = string.Empty;

                    if (!ubidots.IsDisarmed)
                    {
                        method = ubidots.IsArmAway
                                     ? this.Resources.GetText(Resource.String.away)
                                     : this.Resources.GetText(Resource.String.stay);
                    }

                    // Then combine into a sentence
                    this.SendNotification($"{this.Resources.GetText(Resource.String.state_changed)} {arm} {method}");
                }
            }
        }

        /// <summary>
        ///     Build inbox style with the last notification being the latest.
        /// </summary>
        /// <param name="messageBody"> The message body. </param>
        private void BuildInboxStyle(string messageBody)
        {
            NotificationManager notificationManager = NotificationManager.FromContext(this.context);

            // Get currently active notifications
            StatusBarNotification[] activeNotifications = notificationManager.GetActiveNotifications();

            if (activeNotifications.Length == 0)
            {
                // For 0 notifications, clear the inbox, the queue and the counter
                this.inboxStyle = new NotificationCompat.InboxStyle();
                this.messageQueue.Clear();
                this.messageCounter = 0;
            }
            else
            {
                // Else increase the counter
                this.messageCounter++;
            }

            // Enqueue the new message
            this.messageQueue.Enqueue(messageBody);

            // For a counter bigger than the inbox notifications
            if (this.messageCounter > MaxInboxNotifications)
            {
                // Clear the notifications
                this.inboxStyle = new NotificationCompat.InboxStyle();

                // Remove the oldest
                this.messageQueue.Dequeue();

                // Add the rest to the new inbox
                foreach (string message in this.messageQueue)
                {
                    this.inboxStyle.AddLine(message);
                }
            }
            else
            {
                // Else just add the notification to the inbox
                this.inboxStyle.AddLine(messageBody);
            }
        }

        /// <summary>
        ///     The send notification.
        /// </summary>
        /// <param name="messageBody"> The message body. </param>
        private void SendNotification(string messageBody)
        {
            DateTime currentDateTime = DateTime.Now;
            string date = currentDateTime.ToShortDateString();
            string time = currentDateTime.ToShortTimeString();

            string dateTime = $"| {date} {time}";
            string finalMessage = $"{messageBody} {dateTime}";

            this.BuildInboxStyle(finalMessage);

            // Build and show the notifications
            Intent intent = new Intent(this.context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            PendingIntent pendingIntent = PendingIntent.GetActivity(
                this.context,
                0,
                intent,
                PendingIntentFlags.OneShot);

            NotificationCompat.Builder notificationBuilder =
                new NotificationCompat.Builder(this.context, MainActivity.ChannelId);

            // Standard notification content
            notificationBuilder.SetSmallIcon(Resource.Drawable.icon).SetAutoCancel(true).SetShowWhen(false)
                .SetStyle(this.inboxStyle).SetContentIntent(pendingIntent);

            // Add the priority and the text message to support older API (25 or older)
            notificationBuilder.SetPriority((int)NotificationPriority.High).SetContentText(finalMessage);

            // Get the notification manager and notify
            NotificationManager.FromContext(this.context).Notify(1, notificationBuilder.Build());
        }
    }
}