using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CommuteTimingWebsite.Models
{
    public class Journey
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RouteID { get; set; }
        [JsonIgnore]
        public Route Route { get; set; }
    }
}