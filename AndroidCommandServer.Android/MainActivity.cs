using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidCommandServer.Droid.Services;
using AndroidCommandServer.Services;
using Xamarin.Forms;

namespace AndroidCommandServer.Droid
{
    [Activity(Label = "AndroidCommandServer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            // Register Dependencies

            var httpServer = new HttpServer();
            DependencyService.RegisterSingleton<IHttpServer>(httpServer);

            var commandHandler = new CommandHandler();
            DependencyService.RegisterSingleton<ICommandHandler>(commandHandler);

            // Start App

            LoadApplication(new App());

            // WakeLock

            PowerManager pmanager = (PowerManager)GetSystemService(PowerService);
            PowerManager.WakeLock wakelock = pmanager.NewWakeLock(WakeLockFlags.Full, "AndroidCommandServer");
            wakelock.SetReferenceCounted(false);
            wakelock.Acquire();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}