using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CommuteTimingWebsite.Models;
using WebMatrix.WebData;

namespace CommuteTimingWebsite.Controllers
{
    [Authorize]
    public class MyRoutesController : Controller
    {
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            var result = new JsonNetResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding };
            return result;
        }
        public ActionResult MyRoutes()
        {
            using (var db = DatabaseHelpers.GetEntityModel())
            {
                var id = WebSecurity.CurrentUserId;
                var newRoute = db.Routes.Create();
                newRoute.Journeys=new List<Journey>();
                newRoute.Waypoints = new List<Waypoint>();
                newRoute.UserID = id;
                ViewBag.NewRoute = newRoute;
                return View(db.Routes.Include("Waypoints").Include("Journeys").Include("User").Where(r=>r.UserID == id).ToList());
            }
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Journeys(int id)
        {
            using (var db = DatabaseHelpers.GetEntityModel())
            {
                var route =
                    db.Routes.Include("Waypoints").Include("Journeys").Include("User").FirstOrDefault(
                        r => r.ID == id && r.UserID == WebSecurity.CurrentUserId);
                if (route == null)
                    throw new Exception(String.Format("Route with ID {0} not found", id));
                return View(route);
            }
        }
        public ActionResult Waypoints(int id)
        {
            using (var db = DatabaseHelpers.GetEntityModel())
            {
                var route =
                    db.Routes.Include("Waypoints").Include("Journeys").Include("User").FirstOrDefault(
                        r => r.ID == id && r.UserID == WebSecurity.CurrentUserId);
                if (route == null)
                    throw new Exception(String.Format("Route with ID {0} not found", id));
                ViewBag.WaypointTypes =
                   Enum.GetValues(typeof(WaypointType)).OfType<WaypointType>().Select(e => new { ID = (int)e, Name = e.ToString() });
                return View(route);
            }
        }
        [HttpPost]
        public JsonResult SaveRoute(Route r)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var toSave = db.Routes.FirstOrDefault(e => e.ID == r.ID);
                if (toSave == null)
                {
                    toSave = r;
                    toSave.UserID = WebSecurity.CurrentUserId;
                    db.Routes.Add(toSave);
                }
                else
                {
                    toSave.Name = r.Name;
                }
                
                db.SaveChanges();
                return Json(toSave.ID);
            }
        }
        public JsonResult DeleteRoute(Route toDelete)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var route = db.Routes.FirstOrDefault(r => r.ID == toDelete.ID);
                if (route != null)
                {
                    db.Routes.Remove(route);
                    db.SaveChanges();
                }
                return Json(true);
            }
        }
        public JsonResult AddNewJourney(int routeID)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var newJourney = db.Journeys.Create();
                newJourney.RouteID = routeID;
                newJourney.StartDate = DateTime.UtcNow;
                db.Journeys.Add(newJourney);
                db.SaveChanges();
                return Json(newJourney);
            }
        }
        public JsonResult EndJourney(int journeyID)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var journey = db.Journeys.FirstOrDefault(j => j.ID == journeyID);
                if(journey != null)
                {
                    DateTime endDate = DateTime.UtcNow;
                    journey.EndDate = endDate;
                    db.SaveChanges();
                    return Json(endDate);
                }
                return Json(null);
            }
        }
        public JsonResult AddWaypoint(int routeID,decimal lat,decimal lng)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var newWP = db.Waypoints.Create();
                newWP.Latitude = lat;
                newWP.Longitude = lng;
                newWP.Name = "New Waypoint";
                newWP.RouteID = routeID;
                newWP.Index =
                    db.Routes.FirstOrDefault(r => r.ID == routeID).Waypoints.Select(w => w.Index).DefaultIfEmpty().Max() + 1;
                db.Waypoints.Add(newWP);
                db.SaveChanges();
                return Json(newWP);
            }
        }
        [HttpPost]
        public void SaveWaypoints(List<Waypoint> waypoints)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                waypoints.ForEach(w=>
                                      {
                                          var entity = db.Waypoints.FirstOrDefault(wp => wp.ID == w.ID);
                                          if(entity == null)
                                          {
                                              entity = db.Waypoints.Create();
                                              entity.RouteID = w.RouteID;
                                              db.Waypoints.Add(entity);
                                          }
                                          entity.Index = w.Index;
                                          entity.Latitude = w.Latitude;
                                          entity.Longitude = w.Longitude;
                                          entity.Name = w.Name;
                                          entity.Type = w.Type;
                                          db.SaveChanges();
                                      });
            }
        }
        public void DeleteWaypoint(int waypointID)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var wp = db.Waypoints.FirstOrDefault(w => w.ID == waypointID);
                db.Waypoints.Remove(wp);
                db.SaveChanges();
            }
        }
        public void DeleteJourney(int journeyID)
        {
            using (var db = DatabaseHelpers.GetEntityModel())
            {
                var entity = db.Journeys.FirstOrDefault(j=>j.ID == journeyID);
                db.Journeys.Remove(entity);
                db.SaveChanges();
            }
        }

        public JsonResult SaveJourneys(List<Journey> journeys)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                journeys.ForEach(journey=>
                                     {
                                         var existingEntity = db.Journeys.FirstOrDefault(j => j.ID == journey.ID);
                                         if (existingEntity == null) return;
                                         existingEntity.StartDate = journey.StartDate.ToUniversalTime();
                                         existingEntity.EndDate = journey.EndDate == null
                                                                      ? (DateTime?)null
                                                                      : journey.EndDate.Value.ToUniversalTime();
                                         db.SaveChanges();
                                     });
                return Json(true);
            }
        }
    }
}
