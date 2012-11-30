using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferEngine.Controllers
{
    public class FacebookController : Controller
    {
        //
        // GET: /Facebook/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Facebook/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Facebook/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Facebook/Create

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

        //
        // GET: /Facebook/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Facebook/Edit/5

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

        //
        // GET: /Facebook/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Facebook/Delete/5

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
    }
}
