using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace AndroidCarLaucher.Services
{

    public interface IAppListService
    {
        IEnumerable<IAppListServiceApplication> GetApplications();
        void LauchApp(IAppListServiceApplication app);
        MemoryStream GetAppIcon(string packageName);
    }
    public interface IAppListServiceApplication
    {
        string Label { get; set; }
        string PackageName { get; set; }
        MemoryStream AppIcon { get; set; }
        Drawable AppIconDrawable { get; set; }
        Bitmap Bitmap { get; set; }

    }
    public class Application : IAppListServiceApplication
    {
        public Application(IAppListServiceApplication i)
        {
            Label = i.Label;
            PackageName = i.PackageName;
            AppIcon = i.AppIcon;
            AppIconDrawable = i.AppIconDrawable;
            Bitmap = i.Bitmap;
            //AppIconSource = ImageSource.FromUri(new Uri("http://placehold.it/32x32&text=image1"));
            if (i.AppIcon != null)
                AppIconSource = ImageSource.FromStream(
                    () => { return i.AppIcon; }
                    );
        }
        public string Label { get; set; }
        public string PackageName { get; set; }
        public MemoryStream AppIcon { get; set; }
        public ImageSource AppIconSource { get; set; }
        public Drawable AppIconDrawable { get; set; }
        public Bitmap Bitmap { get; set; }
    }
}
