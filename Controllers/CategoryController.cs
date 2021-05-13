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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            //add the category object to the database
            _db.Category.Add(obj);
            //save the changes 
            _db.SaveChanges();
            return View();
        }


    }
}
