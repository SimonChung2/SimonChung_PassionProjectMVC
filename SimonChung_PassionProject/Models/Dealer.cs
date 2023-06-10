using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models
{
    public class Dealer
    {
        [Key]
        public int DealerID { get; set; }
        public string DealerName { get; set; }

        public ICollection<CarModel> CarModels { get; set; }
    }
}