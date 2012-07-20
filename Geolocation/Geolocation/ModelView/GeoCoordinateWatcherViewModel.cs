using System;
using System.Device.Location;
using Geolocation.Model;

namespace Geolocation.ModelView
{
    public class GeoCoordinateWatcherViewModel : BaseNotifyPropertyChanged
    {
        private GeoCoordinate _previewPos = new GeoCoordinate();
        private GeoCoordinateWatcher _coordinateWatcher;
        private readonly GeoCoordinateModel _geo=new GeoCoordinateModel();

        private bool _isCoordinateWatcherEnable;

        private double _movementThreshold;

        #region Prop

        public GeoPositionAccuracy GeoPositionAccuracy
        {
            get { return _geo.GeoPositionAccuracy; }
            set
            {
                if (value == _geo.GeoPositionAccuracy) return;

                _geo.GeoPositionAccuracy = value; //не работает если уже создали _coordinateWatcher 
                NotifyPropertyChanged("GeoPositionAccuracy");
            }
        }

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

        public GeoCoordinate GeoCoordinate
        {
            get { return _geo.GeoCoordinate; }
            private set
            {
                if (value == _geo.GeoCoordinate) return;

                _geo.GeoCoordinate = value;
                NotifyPropertyChanged("GeoCoordinate");
            }
        }

        public DateTimeOffset? TimeStamp
        {
            get { return _geo.TimeStamp; }
            private set
            {
                if (value == _geo.TimeStamp) return;

                _geo.TimeStamp = value;
                NotifyPropertyChanged("TimeStamp");
            }
        }

        public GeoPositionStatus Status
        {
            get { return _geo.Status; }
            private set
            {
                if (value == _geo.Status) return;

                _geo.Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public double Distance
        {
            get { return _geo.Distance; }
            private set
            {
                if (Math.Abs(value - _geo.Distance) < 0.1) return;

                _geo.Distance = value;
                NotifyPropertyChanged("Distance");
            }
        }

        public bool IsCoordinateWatcherEnable
        {
            get { return _isCoordinateWatcherEnable; }
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
                _coordinateWatcher.MovementThreshold = MovementThreshold;
                // use MovementThreshold to ignore noise in the signal
                _coordinateWatcher.StatusChanged += WatcherStatusChanged;
                _coordinateWatcher.PositionChanged += WatcherPositionChanged;
            }
            if (_coordinateWatcher.Permission == GeoPositionPermission.Granted)
                _coordinateWatcher.Start();
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        private void WatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Status = e.Status;
        }

        private void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate = e.Position.Location;
                TimeStamp = e.Position.Timestamp;

                if (!GeoCoordinate.IsUnknown && !_previewPos.IsUnknown)
                {
                    Distance += GeoCoordinate.GetDistanceTo(_previewPos);
                }

                _previewPos = GeoCoordinate;
            }
        }

        private void GeoWatherStop()
        {
            _coordinateWatcher.Stop();

            GeoCoordinate = null;
            TimeStamp = null;

            if (_coordinateWatcher == null) return;

            _coordinateWatcher.Stop();
            _coordinateWatcher.StatusChanged += WatcherStatusChanged;
            _coordinateWatcher.PositionChanged += WatcherPositionChanged;
            _coordinateWatcher.Dispose();
            _coordinateWatcher = null;
        }

        #endregion
    }
}