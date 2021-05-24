using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ApplicationController : Controller
    {



        private readonly ApplicationDbContext _db;

        public ApplicationController(ApplicationDbContext db)
        {
            this._db = db;
        }

        //GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Application> objList = _db.Application; 
            return View(objList);
        }


        //GET CREATE

        public IActionResult Create()
        {

            return View(); 
        }



        //POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Application obj)
        {
            if (ModelState.IsValid)
            {
                _db.Application.Add(obj);
                //save the changes 
                _db.SaveChanges();
                // redirect to another action ; in this case, to index.
                // Since , we are redirecting to the same controller, we don't need to indicate the controller 
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET EDIT 

        public IActionResult Edit(int? id)
        {
            if (id==0 || id == null)
            {
                return NotFound();
            }

            var obj = _db.Application.Find(id);

            if (obj == null)
            {
                return NotFound();

            }

            return View(obj);
        }


        //POST EDIT 
        //states the the action is for POST request 
        [HttpPost]
        //appends an anit-frogery token to form data (for security)
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Application obj)
        {
            if (ModelState.IsValid)
            {
                _db.Application.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        //GET DELETE
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var obj = _db.Application.Find(id);

            if (obj == null)
            {
                return NotFound();

            }

            return View(obj);

        }





        //POST_DELETE

        //states the the action is for POST request 
        [HttpPost]
        //appends an anit-frogery token to form data (for security)
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {

            //Find retreives result based on the Primary key provided 
            var obj = _db.Application.Find(id);

            //remove category object to the database
            _db.Application.Remove(obj);
            //save the changes 
            _db.SaveChanges();
            // redirect to another action ; in this case, to index.
            // Since , we are redirecting to the same controller, we don't need to indicate the controller 
            return RedirectToAction("Index");
        }











    }
}
