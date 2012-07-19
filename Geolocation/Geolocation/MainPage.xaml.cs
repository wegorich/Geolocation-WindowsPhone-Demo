using Geolocation.Model;

namespace Geolocation
{
    public partial class MainPage
    {
        readonly GeoCoordinateWatcherViewModel _coordinate =new GeoCoordinateWatcherViewModel();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = _coordinate;
        }
    }
}