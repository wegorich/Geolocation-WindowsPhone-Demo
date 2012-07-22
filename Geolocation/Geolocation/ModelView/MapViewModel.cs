using System;
using System.Net;
using System.Windows.Media.Imaging;
using Geolocation.Factory;

namespace Geolocation.ModelView
{
    public class MapViewModel : BaseNotifyPropertyChanged
    {
        private const int MaxZoomLevel = 20;
        private const double Epsilon = 0.1;
        private string _mapType;
        
        private WebClient _client;
        private double _height=400;
        private double _width=400;
        private BitmapImage _image;
        private MapFactory _map;
        private int _zoom=12;
        private bool _isSatellite;
        
        private double _lastLon;
        private double _lastLat;

        #region Prop
        
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                if (_zoom == value || _zoom > MaxZoomLevel) return;
                _zoom = value;
                UpdateMapView(_lastLon, _lastLat);

                NotifyPropertyChanged("Zoom");
            }
        }

        public string MapType
        {
            get { return _mapType; }
            set
            {
                if (_mapType == value) return;
                _mapType = value;
                Map = MapFactory.GetMap(_mapType);

                NotifyPropertyChanged("MapType");
            }
        }

        public MapFactory Map
        {
            get { return _map; }
            set
            {
                if (_map == value) return;
                _map = value;
                UpdateMapView(_lastLon, _lastLat);

                NotifyPropertyChanged("Map");
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (Math.Abs(_width - value) < Epsilon) return;
                _width = value;
                UpdateMapView(_lastLon, _lastLat);

                NotifyPropertyChanged("Width");
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (Math.Abs(_height - value) < Epsilon) return;
                _height = value;
                UpdateMapView(_lastLon, _lastLat);

                NotifyPropertyChanged("Height");
            }
        }

        public bool IsSatellite
        {
            get { return _isSatellite; }
            set
            {
                if (_isSatellite == value) return;
                _isSatellite = value;
                UpdateMapView(_lastLon, _lastLat);

                NotifyPropertyChanged("IsSatellite");
            }
        }

        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                if (_image == value) return;
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }

        #endregion
        
        #region Functons
        public void UpdateMapView(double lon, double lat)
        {
            if (Map != null)
            {
                string url = String.Format(Map.GetUrl(IsSatellite),
                                           lat.ToString().Replace(',', '.'),
                                           lon.ToString().Replace(',', '.'),
                                           Zoom, Width, Height);
                var uri = new Uri(url, UriKind.Absolute);
                UpdateImage(uri);
            }

            _lastLat = lat;
            _lastLon = lon;
        }

        // creates a http client for loading the image asynchronously and adding it to the existing image when finished
        private void UpdateImage(Uri uri)
        {
            if (_client != null)
            {
                _client.CancelAsync();
            }

            _client = new WebClient();

            // update the source of the image when the read process is completed
            _client.OpenReadCompleted += delegate(object sender, OpenReadCompletedEventArgs e)
                                             {
                                                 try
                                                 {
                                                     Image=new BitmapImage();
                                                     Image.SetSource(e.Result);
                                                 }
                                                 catch
                                                 {
                                                 }
                                             };

            _client.OpenReadAsync(uri, uri.AbsoluteUri);
        }
        #endregion
    }
}