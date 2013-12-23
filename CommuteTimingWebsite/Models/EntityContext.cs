using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CommuteTimingWebsite.Models
{
    public class EntityContext:DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Waypoint> Waypoints { get; set; } 
    }
}