using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGI_4.Models.ViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<Furniture> Furnitures { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }
}