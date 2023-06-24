using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models.ViewModels
{
    public class DetailsCarModel
    {
        public CarModel SelectedCarModel { get; set; }
        public IEnumerable<CarDto> RelatedCars { get; set; }

    }
}