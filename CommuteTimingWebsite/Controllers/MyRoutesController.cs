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
        public ActionResult MyRoutes()
        {
            using (var db = DatabaseHelpers.GetEntityModel())
            {
                var id = WebSecurity.CurrentUserId;
                var newRoute = db.Routes.Create();
                newRoute.UserID = id;
                ViewBag.NewRoute = newRoute;
                return View(db.Routes.Include("Waypoints").Include("Journeys").Include("User").Where(r=>r.UserID == id).ToList());
            }
        }
        [HttpGet]
        public ActionResult Route(int id)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                var route = db.Routes.Include("Waypoints").Include("Journeys").Include("User").FirstOrDefault(r => r.ID == id);
                if(route == null)
                    throw new Exception(String.Format("Route with ID {0} not found",id));
                var newJourney = db.Journeys.Create();
                newJourney.RouteID = id;
                ViewBag.NewJourney = newJourney;
                return View(route);
            }
        }
        public JsonResult SaveRoute(Route r)
        {
            using(var db=DatabaseHelpers.GetEntityModel())
            {
                db.Routes.Add(r);
                db.SaveChanges();
                return Json(true);
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
    }
}
