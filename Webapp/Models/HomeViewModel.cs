using Webapp.Models;
using System.Collections.Generic;

namespace Webapp.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}