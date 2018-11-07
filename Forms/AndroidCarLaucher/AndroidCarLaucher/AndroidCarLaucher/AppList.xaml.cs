using Android.Runtime;
using DLToolkit.Forms.Controls;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidCarLaucher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppList : ContentPage
    {
        //https://github.com/daniel-luberda/DLToolkit.Forms.Controls/tree/master/FlowListView
        public AppList()
        {
            try
            {
InitializeComponent();
            //FlowListView.Init();
            Load();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public void display(Services.Application app)
        {
            var bitmap = app.Bitmap;

            ByteBuffer buffer = ByteBuffer.Allocate(bitmap.ByteCount);
            bitmap.CopyPixelsToBuffer(buffer);
            buffer.Rewind();

            IntPtr classHandle = JNIEnv.FindClass("java/nio/ByteBuffer");
            IntPtr methodId = JNIEnv.GetMethodID(classHandle, "array", "()[B");
            IntPtr resultHandle = JNIEnv.CallObjectMethod(buffer.Handle, methodId);
            byte[] byteArray = JNIEnv.GetArray<byte>(resultHandle);
            JNIEnv.DeleteLocalRef(resultHandle);
            ImgPreview.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
        }
        async void Load() {
            AppListModel vm = BindingContext as AppListModel;
            if (vm != null) {
                vm.Page = this;
                await vm.LoadAsync();
            }
        }
    }
}