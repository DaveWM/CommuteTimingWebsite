using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CommuteTimingWebsite.Models
{
    public class Route
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Waypoint> Waypoints { get; set; }
        public virtual ICollection<Journey> Journeys { get; set; } 
    }
}