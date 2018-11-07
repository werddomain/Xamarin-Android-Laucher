using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AndroidCarLaucher
{
    public class MainPageModel : BaseVM
    {
        public MainPageModel()
        {
            CreateViewAppsRelayCmd();
            Time = "00:00.00";
            Date = "Jeudi, 23 Décembre";
            //Get the sunrize and sunset : nuget SolarCalculator
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                // Do something
                UpdateTime();
                return true; // True = Repeat again, False = Stop the timer
            });
        }

        void UpdateTime() {
            Time = DateTime.Now.ToString("HH:mm.ss");
            Date = DateTime.Now.ToString("dddd, dd MMMM");
        }

        public INavigation Navigation { get; set; }
        #region Commands
        #region ViewApps 
        public RelayCommand<object> ViewAppsCmd { get; set; }
        public bool CanViewApps { get { return true; } }
        private void CreateViewAppsRelayCmd()
        {
            ViewAppsCmd = new RelayCommand<object>(OnViewApps, (o) => CanViewApps);
            RaisePropertyChange(nameof(ViewAppsCmd));
        }
        private async void OnViewApps(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new AppList());
            
        }
        #endregion
        #endregion

        #region Properties

        private string pTime;
        public string Time
        {
            get { return pTime; }
            set { pTime = value; RaisePropertyChange("Time"); }
        }


        private string pDate;
        public string Date
        {
            get { return pDate; }
            set { pDate = value; RaisePropertyChange("Date"); }
        }
        #endregion


    }
}
