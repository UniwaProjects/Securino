namespace Securino.Droid
{
    using System;

    using Android.App;
    using Android.Runtime;

    using AndroidX.AppCompat.App;

    using Xamarin.Essentials;

    [Application(Theme = "@style/MainTheme")]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Platform.Init(this);
        }
    }
}