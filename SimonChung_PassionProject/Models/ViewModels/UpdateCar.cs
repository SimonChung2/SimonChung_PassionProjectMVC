using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models.ViewModels
{
    public class UpdateCar
    {
        //This viewmodel is a class which stores information that we need to present to /Car/Update/{id}

        //the existing car information

        public CarDto SelectedCar { get; set; }

        //also like to include all car models to choose from when updating this car

        public IEnumerable<CarModel> CarModelOptions { get; set; }

        public IEnumerable<Dealer> DealerOptions { get; set; }

    }
}