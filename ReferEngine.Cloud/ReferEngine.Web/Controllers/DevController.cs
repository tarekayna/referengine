using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class DevController : Controller
    {
        private ReferEngineDatabaseContext _db = new ReferEngineDatabaseContext();

        public ActionResult Index()
        {
            var model = _db.Developers.ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            IList<Developer> model = _db.Developers.ToList();
            Developer dev = model.First(x => x.Id == id);
            return View(dev);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
