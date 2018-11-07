using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace AndroidCarLaucher
{
    public class AppListModel:BaseVM
    {
        public AppList Page { get; set; }

        public AppListModel()
        {
           
            Title = "Applications";
        }
        public async Task LoadAsync() {
            AppService = Services.UseService.GetAppListService();
            Applications = AppService.GetApplications().Select(o=> new Services.Application(o));
            //Applications = await Task.Run<IEnumerable<Services.IAppListServiceApplication>>( () => { return AppService.GetApplications(); });
            RaisePropertyChange(nameof(Applications));
        }
        public IEnumerable<Services.Application> Applications { get; set; }

        public Services.IAppListService AppService { get; set; }

        Services.Application selectedApp;

        public Services.Application SelectedApp {
            get => selectedApp;
            set {
                selectedApp = value;
                //OpenApp(value);
                Page.display(value);
            } }

        void OpenApp(Services.Application app) {
            AppService.LauchApp(app);
        }
    }
}
