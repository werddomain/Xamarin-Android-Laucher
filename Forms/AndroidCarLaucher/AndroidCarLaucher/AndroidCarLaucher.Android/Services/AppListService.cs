using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidCarLaucher.Droid.Services;
using AndroidCarLaucher.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppListService))]
namespace AndroidCarLaucher.Droid.Services
{

    public class AppListService : IAppListService
    {
        public IEnumerable<IAppListServiceApplication> GetApplications()
        {
            var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);
            foreach (var app in apps)
            {
                var label = app.LoadLabel(Android.App.Application.Context.PackageManager);
                if (!label.ToLower().StartsWith("com.")) { 
                    var drawable = GetAppIconDrawable(app);
                    var bitmap = drawableToBitmap(drawable);
                    yield return new AppListServiceApplication
                    {
                        Label = label,
                        PackageName = app.PackageName,
                        AppIcon = GetAppIcon(drawable),
                        AppInfo = app,
                        AppIconDrawable = drawable,
                        Bitmap = bitmap
                    };
                }
            }
        }
        public MemoryStream GetAppIcon(string packageName) {
            var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);
            ApplicationInfo app = apps.FirstOrDefault(o => o.PackageName == packageName);
            return GetAppIcon(app);
        }

        public MemoryStream GetAppIcon(ApplicationInfo app) {
            try
            {
                var drawable = GetAppIconDrawable(app);
                var bitmap = drawableToBitmap(drawable);
                return MemoryStreamFromBitmap(bitmap);
            }
            catch (Exception e)
            {
                var message = e.ToString();
                return null;
            }
            
        }
        public MemoryStream GetAppIcon(Drawable Drawable)
        {
            try
            {
                
                var bitmap = drawableToBitmap(Drawable);
                return MemoryStreamFromBitmap(bitmap);
            }
            catch (Exception e)
            {
                var message = e.ToString();
                return null;
            }

        }

        Drawable GetAppIconDrawable(string packageName)
        {
            var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MatchAll);
            ApplicationInfo app = apps.FirstOrDefault(o => o.PackageName == packageName);
            return GetAppIconDrawable(app);
            
        }
        Drawable GetAppIconDrawable(ApplicationInfo app)
        {
            try
            {
                return app.LoadIcon(Android.App.Application.Context.PackageManager);
               
               
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                return null;
            }


        }
        ImageSource ImageSourceFromMemoryStream(MemoryStream ms)
        {
            return ImageSource.FromStream(() => ms);
        }
        MemoryStream MemoryStreamFromBitmap(Bitmap bmp)
        {
            MemoryStream stream = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
            return stream;
        }
        Bitmap drawableToBitmap(Drawable drawable)
        {
            Bitmap bitmap = null;

            if (drawable is BitmapDrawable) {
                BitmapDrawable bitmapDrawable = (BitmapDrawable)drawable;
                if (bitmapDrawable.Bitmap != null)
                {
                    return bitmapDrawable.Bitmap;
                }
            }
            if (drawable is AdaptiveIconDrawable)
            {
                AdaptiveIconDrawable adaptiveIconDrawable = (AdaptiveIconDrawable)drawable;
                if (!(adaptiveIconDrawable.IntrinsicWidth <= 0 || adaptiveIconDrawable.IntrinsicHeight <= 0))
                {
                    //bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
                    //Canvas c = new Canvas(bitmap);
                    var bd = adaptiveIconDrawable.Background;
                    var fd = adaptiveIconDrawable.Foreground;

                    Drawable[] drr = new Drawable[2];
                    drr[0] = bd;
                    drr[1] = fd;

                    LayerDrawable layerDrawable = new LayerDrawable(drr);

                    int width = layerDrawable.IntrinsicWidth;
                    int height = layerDrawable.IntrinsicHeight;

                    bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

                    Canvas c = new Canvas(bitmap);

                    layerDrawable.SetBounds(0, 0, c.Width, c.Height);
                    layerDrawable.Draw(c);

                    return bitmap;

                    //adaptiveIconDrawable.SetBounds(0, 0, c.Width, c.Height);
                    //adaptiveIconDrawable.Draw(c);

                    return bitmap;

                }
            }

            if (drawable.IntrinsicWidth <= 0 || drawable.IntrinsicHeight <= 0)
            {
                bitmap = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888); // Single color bitmap will be created of 1x1 pixel
            }
            else
            {
                bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
            }

            Canvas canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
            drawable.Draw(canvas);
            return bitmap;
        }
        public void LauchApp(IAppListServiceApplication app)
        {
            Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(app.PackageName);
            if (intent != null)
                Android.App.Application.Context.StartActivity(intent);
        }
    }
    public class AppListServiceApplication : IAppListServiceApplication
    {
        public string Label { get; set; }
        public string PackageName { get; set; }
        public ApplicationInfo AppInfo { get; set; }
        //@[<PackageName>:]<ResourceType>/<ResourceName>.
        public MemoryStream AppIcon { get; set; }
        public Drawable AppIconDrawable { get; set; }
        public Bitmap Bitmap { get; set; }
    }
}