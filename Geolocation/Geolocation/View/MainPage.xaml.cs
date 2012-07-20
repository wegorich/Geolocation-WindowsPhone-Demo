using Geolocation.ModelView;

namespace Geolocation.View
{
    public partial class MainPage
    {
        private readonly GeoCoordinateWatcherViewModel _coordinate = new GeoCoordinateWatcherViewModel();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = _coordinate;
        }
    }
}