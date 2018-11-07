using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AndroidCarLaucher.Services
{
    public class UseService
    {
        public static IAppListService GetAppListService() {
            return DependencyService.Get<IAppListService>();
        }
        public static async Task<double> GetSpeed(int TimeOut = 2000) {
            //https://github.com/jamesmontemagno/permissionsplugin
            //https://jamesmontemagno.github.io/GeolocatorPlugin/GettingStarted.html

            var data = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(0,0,0,0, TimeOut), null, true);
            //Meter per seconds to Km per hours (Km/H)
            return (data.Speed * 60 * 60) / 1000;
            //Location boston = new Location(42.358056, -71.063611);
            //Location sanFrancisco = new Location(37.783333, -122.416667);
            //double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
        }
    }
}
