using System.Device.Location;
using System.Linq;
using Geolocation.Factory;

namespace Geolocation.View
{
    public partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            mapsListPicker.ItemsSource = MapFactory.MapTypes;

            var accuracys = (from x in typeof(GeoPositionAccuracy).GetFields() 
                             where x.IsLiteral 
                             select (GeoPositionAccuracy)x.GetValue(null)).ToList();

            accuracyListPicker.ItemsSource = accuracys;
        }
    }
}