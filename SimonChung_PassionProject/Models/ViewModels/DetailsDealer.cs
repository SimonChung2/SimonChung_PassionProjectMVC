using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonChung_PassionProject.Models.ViewModels
{
    public class DetailsDealer
    {
        public Dealer SelectedDealer { get; set; }
        public IEnumerable<CarDto> RelatedCars { get; set; }

    }
}