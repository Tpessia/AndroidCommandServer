using Android.Graphics;
using AndroidCommandServer.Common;
using AndroidCommandServer.Services;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AndroidCommandServer.Droid.Services
{
    public class CommandHandler : ICommandHandler
    {
        public async Task<string> HandleCommand(string command)
        {
            if (command == "screenshot")
            {
                //https://docs.microsoft.com/en-us/xamarin/essentials/screenshot
                //https://www.xamarinhelp.com/taking-a-screenshot-in-xamarin-forms/

                //if (!Screenshot.IsCaptureSupported)

                var screenshot = await Screenshot.CaptureAsync(); // Don't take screenshot from other apps
                var stream = await screenshot.OpenReadAsync();
                //var img = Image.FromStream(stream);
                var imgBase64 = await stream.ToBase64Async();

                //var bytes = ScreenCapture();
                //var imgBase64 = bytes.ToBase64();

                return imgBase64;
            }

            return "Invalid Command";
        }

        private byte[] ScreenCapture()
        {
            var view = Platform.CurrentActivity.Window.DecorView;

            view.DrawingCacheEnabled = true;
            var bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}