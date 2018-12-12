using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Models
{
    public class CheeseCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<Cheese> Cheeses { get; set; }

        public CheeseCategory() { }

        public CheeseCategory(string name)
        {
            Name = name;
        }
    }
}
