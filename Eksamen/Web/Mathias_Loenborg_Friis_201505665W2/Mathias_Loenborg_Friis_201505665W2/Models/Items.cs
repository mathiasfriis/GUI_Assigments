using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mathias_Loenborg_Friis_201505665W2.Models
{
    public class Items
    {
        List<String> itemList {get; set;} = new List<string>();

        public Items()
        {
            for(int i = 0; i<5;i++)
            {
                itemList.Add("Bob" + i);
            }
        }
    }
}
