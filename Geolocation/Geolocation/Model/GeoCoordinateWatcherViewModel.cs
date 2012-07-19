using System;
using System.Device.Location;

namespace Geolocation.Model
{
    public class GeoCoordinateWatcherViewModel : BaseNotifyPropertyChanged
    {
        private GeoCoordinateWatcher _coordinateWatcher;

        #region Prop
        private GeoPositionAccuracy _geoPositionAccuracy = GeoPositionAccuracy.High;
        public GeoPositionAccuracy GeoPositionAccuracy
        {
            get { return _geoPositionAccuracy; }
            set
            {
                if (value == _geoPositionAccuracy) return;

                _geoPositionAccuracy = value;   //не работает если уже создали _coordinateWatcher 
                NotifyPropertyChanged("GeoPositionAccuracy");
            }
        }

        private double _movementThreshold = 20;
        public double MovementThreshold
        {
            get { return _movementThreshold; }
            set
            {
                if (Math.Abs(value - _movementThreshold) < 0.1) return;

                _movementThreshold = value;
                if (_coordinateWatcher != null)
                {
                    _coordinateWatcher.MovementThreshold = _movementThreshold;
                }
                NotifyPropertyChanged("MovementThreshold");

            }
        }

        private GeoCoordinate _geoCoordinate;
        public GeoCoordinate GeoCoordinate
        {
            get { return _geoCoordinate; }
            private set
            {
                if (value == _geoCoordinate) return;

                _geoCoordinate = value;
                NotifyPropertyChanged("GeoCoordinate");
            }
        }

        private DateTimeOffset? _timeStamp;
        public DateTimeOffset? TimeStamp
        {
            get { return _timeStamp; }
            private set
            {
                if (value == _timeStamp) return;

                _timeStamp = value;
                NotifyPropertyChanged("TimeStamp");
            }
        }

        private GeoPositionStatus _status;
        public GeoPositionStatus Status
        {
            get { return _status; }
            private set
            {
                if (value == _status) return;

                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private bool _isCoordinateWatcherEnable;
        public bool IsCoordinateWatcherEnable
        {
            get
            {
                return _isCoordinateWatcherEnable;
            }
            set
            {
                if (_isCoordinateWatcherEnable && value) return;

                _isCoordinateWatcherEnable = value;

                if (_isCoordinateWatcherEnable)
                {
                    GeoWatherStart();
                }
                else
                {
                    GeoWatherStop();
                }
                NotifyPropertyChanged("IsCoordinateWatcherEnable");
            }
        }
        #endregion

        #region GeoWather functions
        private void GeoWatherStart()
        {
            // The watcher variable was previously declared as type GeoCoordinateWatcher. 
            if (_coordinateWatcher == null)
            {
                _coordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy); // using high accuracy
                _coordinateWatcher.MovementThreshold = MovementThreshold; // use MovementThreshold to ignore noise in the signal
                _coordinateWatcher.StatusChanged += WatcherStatusChanged;
                _coordinateWatcher.PositionChanged += WatcherPositionChanged;
            }

            _coordinateWatcher.Start();
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        private void WatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Status = e.Status;
        }

        void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate= e.Position.Location;
            TimeStamp = e.Position.Timestamp;
        }

        private void GeoWatherStop()
        {
            _coordinateWatcher.Stop();

            GeoCoordinate = null;
            TimeStamp = null;
        }

        #endregion
    }
}
