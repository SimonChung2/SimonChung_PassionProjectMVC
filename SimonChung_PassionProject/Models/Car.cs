using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public int Price { get; set; }

        //foreign key
        [ForeignKey("CarModels")]
        public int ModelID { get; set; }
        public virtual CarModel CarModels { get; set; }

        [ForeignKey("Dealers")]
        public int DealerID { get; set; }
        public virtual Dealer Dealers { get; set; }

    }

    public class CarDto
    {
        public int CarID { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set;}
        public int Price { get; set; }


        public string CarModelName { get; set; }

        public string CarMakeName { get; set; }
        public string DealerName { get; set; }
    }
}