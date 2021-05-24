using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        //GET INDEX
        public IActionResult Index()
        {
            //Retrieves all the categories from the database
            //Assigning to IEnumerable make it readonly
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category).Include(u => u.Application); //select * from Product

            //Load the category
            //foreach (var obj in objList)
            //{
            //    //if id in Category == CategoryId of obj
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);

            //}

            return View(objList); //we pass the list to the view
        }

        //GET_UPSERT
        //If it is an edit , then id will be an int otherwise ,
        //if it is a insert, then id will be null
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.CategoryName,
            //    Value = i.Id.ToString(),
            //});

            ////ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString(),
                }),

                ApplicationSelectList = _db.Application.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                //this is for update
                return View(productVM);
            }
        }

        //POST_UPSERT

        //states the the action is for POST request
        [HttpPost]
        //appends an anit-frogery token to form data (for security)
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)//checks if the rules defined for the model is valid
            {
                //get the file to be uploaded
                var files = HttpContext.Request.Form.Files;
                //path of the webroot
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //the path to which the image has to be copied to
                    string upload = webRootPath + WC.ImagePath;

                    //generate a ui as a file name
                    string fileName = Guid.NewGuid().ToString();

                    //get the extension of the file
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);

                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    //retrieve object from database. We are adding AsNoTracking as there are 2 objects with the same Id
                    //when we update the database , ERC will not know which object to be used for update
                    //hence , we shal disable tracking for object retreived from the database
                    var obj = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        //the path to which the image has to be copied to
                        string upload = webRootPath + WC.ImagePath;

                        //generate a ui as a file name
                        string fileName = Guid.NewGuid().ToString();

                        //get the extension of the file
                        string extension = Path.GetExtension(files[0].FileName);

                        //get the directory of the old image
                        var oldFile = Path.Combine(upload, obj.Image);

                        //check if the file exists in the system
                        if (System.IO.File.Exists(oldFile))
                        {
                            //delete file from system
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        //add the new file name to the object received from view
                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        //add the old file name from the object recevied from view
                        productVM.Product.Image = obj.Image;
                    }
                    _db.Product.Update(productVM.Product);

                    _db.SaveChanges();

                    return RedirectToAction("Index");
                    //return View(productVM);
                }
            }

            productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString(),
                }),

                ApplicationSelectList = _db.Application.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            return View(productVM);//we return the obj back to the view to display the error messages to be displayed
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

            //retrieves result based on the Primary key provided
            //include the category property value    //find the Product that matches the ID
            var obj = _db.Product.Include(u => u.Category).Include(u => u.Application).FirstOrDefault(u => u.Id == id);

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
        //[ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            //Find retreies result based on the Primary key provided

            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            //path of the webroot
            string webRootPath = _webHostEnvironment.WebRootPath;

            //the path to which the image has to be copied to
            string upload = webRootPath + WC.ImagePath;

            //get the directory of the old image
            var oldFile = Path.Combine(upload, obj.Image);

            //check if the file exists in the system
            if (System.IO.File.Exists(oldFile))
            {
                //delete file from system
                System.IO.File.Delete(oldFile);
            }

            //remove Product object to the database
            _db.Product.Remove(obj);
            //save the changes
            _db.SaveChanges();
            // redirect to another action ; in this case, to index.
            // Since , we are redirecting to the same controller, we don't need to indicate the controller

            return RedirectToAction("Index");
        }

       


    }


}