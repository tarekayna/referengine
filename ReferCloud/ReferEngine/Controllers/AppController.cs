using System;
using System.Linq;
using System.Web.Mvc;
using ReferLib;

namespace ReferEngine.Controllers
{
    public class AppController : Controller
    {
        private readonly ReferDb _db = new ReferDb();

        public ActionResult Index()
        {
            var model = _db.Apps.ToList();
            return View(model);
        }

        public ActionResult Details(string id)
        {
            int inputId = Convert.ToInt32(id);
            App app = _db.Apps.First(a => a.Id == inputId);
            return View(app);
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
