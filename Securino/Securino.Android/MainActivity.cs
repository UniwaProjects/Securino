// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="Uniwa">
//   Copyright (c) 2020 All Rights Reserved
// </copyright>
// <summary>
//   Defines the MainActivity type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Securino.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;

    using AndroidX.AppCompat.App;

    using Forms9Patch.Droid;

    using Prism;
    using Prism.Ioc;

    using Securino.Services;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using Platform = Xamarin.Essentials.Platform;

    /// <summary>
    ///     The main activity.
    /// </summary>
    [Activity(
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Locale,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        /// <summary>
        ///     The notification channel id.
        /// </summary>
        internal static readonly string ChannelId = "Securino Notifications";

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        internal static MainActivity Instance { get; private set; }

        /// <summary>
        ///     The on request permissions result.
        /// </summary>
        /// <param name="requestCode">
        ///     The request code.
        /// </param>
        /// <param name="permissions">
        ///     The permissions.
        /// </param>
        /// <param name="grantResults">
        ///     The grant results.
        /// </param>
        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        ///     The on create.
        /// </summary>
        /// <param name="savedInstanceState">
        ///     The saved instance state.
        /// </param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            // Set the instance
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Create notification channel
            this.CreateNotificationChannel();

            // Initialize packages
            Forms.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);
            Settings.Initialize(this);

            Forms.Init(this, savedInstanceState);

            this.LoadApplication(new App(new AndroidInitializer()));
        }

        /// <summary>
        ///     Creates a notification channel for API 26+.
        /// </summary>
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            string channelName = ChannelId;
            string channelDescription = string.Empty;
            NotificationChannel channel = new NotificationChannel(ChannelId, channelName, NotificationImportance.High)
                                              {
                                                  Description = channelDescription
                                              };

            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }

    /// <summary>
    ///     The android initializer.
    /// </summary>
    public class AndroidInitializer : IPlatformInitializer
    {
        /// <summary>
        ///     The register types.
        /// </summary>
        /// <param name="containerRegistry">
        ///     The container registry.
        /// </param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<IStartWatchdogService, StartWatchdogService>();
        }
    }
}