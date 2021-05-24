using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1,int.MaxValue)]
        public double Price { get; set; }

        public string Image { get; set; }

        public string ShortDescription { get; set; }

        //EXPLICIT DECLARATION OF FOREIGN KEY 
        [ForeignKey("CategoryId")]
        [DisplayName("Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("ApplicationId")]
        [DisplayName("Application Type")]
        public int ApplicationId { get; set; }




        //EF CORE automatically adds a mapping between product and category. Create a Category Id column 
        //Which acts as the foreign key 
        public virtual  Category Category { get; set; }
                
        public virtual Application Application { get; set; }



    }
}
