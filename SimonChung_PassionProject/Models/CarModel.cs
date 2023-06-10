using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models
{
    public class CarModel
    {
        [Key]
        public int ModelID { get; set; }
        public string ModelName { get; set; }
        public string Make { get; set; }

        public ICollection<Dealer> Dealers { get; set; }
    }
}