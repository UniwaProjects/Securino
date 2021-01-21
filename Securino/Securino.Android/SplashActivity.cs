namespace Securino.Droid
{
    using Android.App;
    using Android.Content;
    using Android.Content.PM;

    using AndroidX.AppCompat.App;

    [Activity(
        Theme = "@style/MainTheme.Splash",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            this.StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}