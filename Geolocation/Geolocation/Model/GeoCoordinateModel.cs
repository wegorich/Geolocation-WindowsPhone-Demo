using System;
using System.Device.Location;

namespace Geolocation.Model
{
    public class GeoCoordinateModel
    {
        public double Distance;
        public GeoCoordinate GeoCoordinate;
        public GeoPositionAccuracy GeoPositionAccuracy;
        public GeoPositionStatus Status;
        public DateTimeOffset? TimeStamp;
    }
}
