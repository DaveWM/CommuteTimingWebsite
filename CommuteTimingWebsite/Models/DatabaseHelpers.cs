using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommuteTimingWebsite.Models
{
    public static class DatabaseHelpers
    {
        public static EntityContext GetEntityModel()
        {
            var db = new EntityContext();
            db.Database.Initialize(false);
            return db;
        }
    }
}