using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Category
    {

        [Key] //Tells that the property will be a primary key 

        //Id column autoincremenets in the database 
        //if the name of the property is Id or CategoryId , it iwill be auto recognised as a primary entity 
        public int Id { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }




    }
}
