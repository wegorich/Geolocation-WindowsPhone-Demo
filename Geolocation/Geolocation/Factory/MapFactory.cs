using System;
using System.Collections.Generic;

namespace Geolocation.Factory
{
    public class MapFactory
    {
        private const string BingKey = "AslbzCOX3iwxX97TSAf28_rxjy-Z6HrIQZhAh6wgB18kBK7LOOGzTFCiFNHN-Ruk";

        #region Maps

        private static readonly Dictionary<string, MapFactory> Map = new Dictionary<string, MapFactory>
        {
            {"google",new MapFactory((x=> "http://maps.google.com/maps/api/staticmap?center={0},{1}&" +
                                            "zoom={2}&" +
                                            "size={3}x{4}&" +
                                            "sensor=true&" +
                                            "markers=color:blue|{0},{1}"+
                                            (x?"&maptype=satellite":""))
                                    )},
            {"bing",new MapFactory((x=> "http://dev.virtualearth.net/REST/v1/Imagery/Map/" +
                                        (x?"Road": "Aerial")+
                                        "/{0},{1}/{2}?" +
                                        "mapSize={3},{4}&" +
                                        "pushpin={0},{1};35&" +
                                        "key="+BingKey)
            )},
            {"osm",new MapFactory((x=> "http://staticmap.openstreetmap.de/staticmap.php?" +
                                        "center={0},{1}&" +
                                        "zoom={2}&" +
                                        "size={3}x{4}&" +
                                        "markers={0},{1},red-pushpin"+
                                        (x?"&maptype=cycle":""))
            )},
        };

        #endregion

        private MapFactory(Func<bool, string> getUrl)
        {
            GetUrl = getUrl;
        }

        public readonly Func<bool, string> GetUrl;
        
        public static IEnumerable<string> MapTypes { get { return Map.Keys; } }
        
        public static MapFactory GetMap(string type)
        {
            return Map[type];
        }
    }
}