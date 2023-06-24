using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models.ViewModels
{
    public class CreateCar
    {
        public IEnumerable<CarModel> CarModelOptions { get; set; }

        public IEnumerable<Dealer> DealerOptions { get; set; }
    }
}