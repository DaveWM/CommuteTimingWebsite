using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Http.OData.Builder;

namespace CommuteTimingWebsite.Models
{
    public class EntityContext:DbContext
    {
        static EntityContext()
        {
            Database.SetInitializer(new DropCreateTablesIfIncompatible());
        }
        public DbSet<Route> Routes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Waypoint> Waypoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            new List<DecimalPropertyConfiguration>
                                 {
                                     modelBuilder.Entity<Waypoint>().Property(w => w.Latitude),
                                     modelBuilder.Entity<Waypoint>().Property(w => w.Longitude)
                                 }.ForEach(p => p.HasPrecision(15, 10));
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DropCreateTablesIfIncompatible : IDatabaseInitializer<EntityContext>
    {
        public void InitializeDatabase(EntityContext context)
        {
            context.Database.CreateIfNotExists();
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (!dbExists)
            {
                throw new ApplicationException("No database instance");
            }
            //if (!context.Database.CompatibleWithModel(false))
            //{
            //    // create all tables
            //    var dbCreationScript = ((IObjectContextAdapter)
            //                            context).ObjectContext.CreateDatabaseScript();
            //    context.Database.ExecuteSqlCommand(dbCreationScript);
            //    context.SaveChanges();
            //}
        }
    }
}