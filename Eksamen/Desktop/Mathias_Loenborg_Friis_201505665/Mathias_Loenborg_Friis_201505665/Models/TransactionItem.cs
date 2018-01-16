using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    public class TransactionItem:INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        private int quantity = 0;

        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                NotifyPropertyChanged("Quantity");
            }
        }

        public TransactionItem(int id, string name, int price, int quantity)
        {
            ID = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            TotalPrice = Price * Quantity;
        }

        public void UpdateTotalPrice()
        {
            TotalPrice = Price * Quantity;
        }

        #region INotifyPropertyChanged implementation

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
