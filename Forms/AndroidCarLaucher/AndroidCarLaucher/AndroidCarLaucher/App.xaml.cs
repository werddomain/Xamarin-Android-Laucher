using DLToolkit.Forms.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AndroidCarLaucher
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }
        public static MainPage MainPageInstance { get; private set; }

        public App()
        {
            InitializeComponent();
            FlowListView.Init();
            App.MainPageInstance = new MainPage();

            App.NavigationPage = new NavigationPage(App.MainPageInstance);
            MainPage = NavigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
