using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommuteTimingWebsite.Models
{
    public class Journey
    {
        public int ID { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public int RouteID { get; set; }
        [JsonIgnore]
        public Route Route { get; set; }
    }
}