using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Application Name")]
        public string Name { get; set; }
    }
}
