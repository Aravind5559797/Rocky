using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class CategoryController: Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }




        //GET INDEX
        public IActionResult Index()
        {
            //Retrieves all the categories from the database 
            //Assigning to IEnumerable make it readonly
            IEnumerable<Category> objList = _db.Category; //select * from category 
            return View(objList); //we pass the list to the view 

        }


        //GET_CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST_CREATE

        //states the the action is for POST request 
        [HttpPost]
        //appends an anit-frogery token to form data (for security)
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)//checks if the rules defined for the model is valid 
            {
                //add the category object to the database
                _db.Category.Add(obj);
                //save the changes 
                _db.SaveChanges();
                // redirect to another action ; in this case, to index.
                // Since , we are redirecting to the same controller, we don't need to indicate the controller 
                return RedirectToAction("Index");
            }


            return View(obj);//we return the obj back to the view to display the error messages to be displayed


        }


        //GET EDIT
        public IActionResult Edit(int? id)
        {
            //id iwll be 0 as it is not a nullable field 

            if(id == 0)
            {
                return NotFound();
            }

            //Find retreives result based on the Primary key provided 
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            //if the record is found , the obj is passed to the Edit view 
            return View(obj);

        }


        //POST_EDIT

        //states the the action is for POST request 
        [HttpPost]
        //appends an anit-frogery token to form data (for security)
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)//checks if the rules defined for the model is valid 
            {
                //update the category object to the database
                _db.Category.Update(obj);
                //save the changes 
                _db.SaveChanges();
                // redirect to another action ; in this case, to index.
                // Since , we are redirecting to the same controller, we don't need to indicate the controller 
                return RedirectToAction("Index");
            }


            return View(obj);//we return the obj back to the view to display the error messages to be displayed


        }


        //GET DELETE
        public IActionResult Delete(int? id)
        {
            //if the id is not assinged , it will be null as it is a nullable field 
            //Hence we will check check the value of id 
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Find retreives result based on the Primary key provided 
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            //if the record is found , the obj is passed to the Delete view 
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
            var obj = _db.Category.Find(id);

            //remove category object to the database
            _db.Category.Remove(obj);
            //save the changes 
            _db.SaveChanges();
            // redirect to another action ; in this case, to index.
            // Since , we are redirecting to the same controller, we don't need to indicate the controller 
            return RedirectToAction("Index");
        }


    }
}
