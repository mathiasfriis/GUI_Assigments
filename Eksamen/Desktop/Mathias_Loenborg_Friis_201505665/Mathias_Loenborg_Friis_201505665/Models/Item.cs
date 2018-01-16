using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    public class Item
    {
        public int ID { get; set;}
        public string Name { get; set; }
        public int Price { get; set; }
        
        public Item(int id, string name, int price)
        {
            ID = id;
            Name = name;
            Price = price;
        }
    }


}
