using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CommuteTimingWebsite.Models
{
    public class Waypoint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int RouteID { get; set; }
        public WaypointType Type { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public virtual Route Route { get; set; }
    }
    public enum WaypointType
    {
        Walk=0,
        Car=1,
        Bus=2,
        Train=3,
        Tube=4,
        Bike=5
    }
}