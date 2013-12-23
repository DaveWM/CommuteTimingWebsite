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
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int RouteID { get; set; }
        [JsonIgnore]
        public virtual Route Route { get; set; }
    }
}