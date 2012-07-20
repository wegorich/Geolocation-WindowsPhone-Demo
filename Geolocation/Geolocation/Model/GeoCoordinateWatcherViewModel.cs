using System;
using System.Device.Location;

namespace Geolocation.Model
{
    public class GeoCoordinateWatcherViewModel : BaseNotifyPropertyChanged
    {
        #region Fields
        
        const double MovementTresholdEpsilon = 0.1;
        
        GeoCoordinateWatcher _coordinateWatcher;
        
        GeoPositionAccuracy _geoPositionAccuracy = GeoPositionAccuracy.High;
        GeoCoordinate _geoCoordinate;
        GeoPositionStatus _status;
        
        bool _isCoordinateWatcherEnable; 
        double _movementThreshold = 20;
        DateTimeOffset? _timeStamp;
        
        #endregion // Fields
        
        #region Properties
        
        public GeoPositionAccuracy GeoPositionAccuracy
        {
            get { return _geoPositionAccuracy; }
            set
            {
                if (value != _geoPositionAccuracy)
                {
                    _geoPositionAccuracy = value;   // Не работает если уже создали _coordinateWatcher.
                    NotifyPropertyChanged("GeoPositionAccuracy");
                }
            }
        }

        public double MovementThreshold
        {
            get { return _movementThreshold; }
            set
            {
                // Egor, fake Resharper. It works faster, then not inverted.
                if (Math.Abs(value - _movementThreshold) >= MovementTresholdEpsilon)
                {
                    _movementThreshold = value;
                    if (_coordinateWatcher != null)
                    {
                        _coordinateWatcher.MovementThreshold = _movementThreshold;
                    }
                
                    NotifyPropertyChanged("MovementThreshold");
                }
            }
        }
        
        public GeoCoordinate GeoCoordinate
        {
            get { return _geoCoordinate; }
            private set
            {                
                if (value != _geoCoordinate) 
                {
                    _geoCoordinate = value;
                    NotifyPropertyChanged("GeoCoordinate");
                }
            }
        }

        
        public DateTimeOffset? TimeStamp
        {
            get { return _timeStamp; }
            private set
            {
                if (value != _timeStamp)
                {
                    _timeStamp = value;
                    NotifyPropertyChanged("TimeStamp");
                }
            }
        }
        
        public GeoPositionStatus Status
        {
            get { return _status; }
            private set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }
        
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
                    GeoWatсherStart();
                }
                else
                {
                    GeoWatсherStop();
                }
                
                NotifyPropertyChanged("IsCoordinateWatcherEnable");
            }
        }
        
        #endregion // Properties.

        #region GeoWatcher functions
        
        private void GeoWatсherStart()
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

        зrivate void GeoWatсherStop()
        {
            _coordinateWatcher.Stop();

            GeoCoordinate = null;
            TimeStamp = null;
        }
        
        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        private void WatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Status = e.Status;
        }

        void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate = e.Position.Location;
            TimeStamp = e.Position.Timestamp;
        }

        #endregion // GeoWatcher functions.
    }
}
