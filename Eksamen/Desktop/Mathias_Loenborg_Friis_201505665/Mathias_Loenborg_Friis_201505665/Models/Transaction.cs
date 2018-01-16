using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    public class Transaction: ObservableCollection<Item>, INotifyPropertyChanged
    {
        public List<Item> items = new List<Item>(); //List of all items including price, ID and name.
        public Dictionary<string, int> itemQuantities = new Dictionary<string, int>(); //Keeps track of quantity of items. Key=name, Value=quantity.
        public enum Payment { notPaid, Cash, MobilePay }; //enum to keep track of paid/not paid + payment method.
        public Payment paymentMethod = Payment.notPaid;
        int cashReceived = 0; //cash received if paymentMethod = cash
        int cashBack = 0; //cash back if paymentMethod = cash
        ///Get total price of transaction.
        public int totalPrice
        {
            get
            {
                int sum = 0;
                foreach (Item i in items)
                {
                    sum += i.Price;
                }
                return sum;
            }
        }

        public Transaction()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                
                AddItem(new Item(1, "Hotter", 10));
                AddItem(new Item(1, "Hotter", 10));
                AddItem(new Item(2, "Cocio", 15));
            }
            
            AddItem(new Item(1, "Hotter", 10));
            AddItem(new Item(1, "Hotter", 10));
            AddItem(new Item(2, "Cocio", 15));
        }

        //Add item to transaction.
        public void AddItem(Item item)
        {
            Add(item);
            if (itemQuantities.ContainsKey(item.Name))
            {
                itemQuantities[item.Name]++;
            }
            else
            {
                itemQuantities.Add(item.Name, 1);
            }
        }

        //Remove item from transaction.
        public void RemoveItem(Item item)
        {
            //check if any items with name that matches
            if(itemQuantities.ContainsKey(item.Name))
            {
                //if a real quantity
                if(itemQuantities[item.Name]>0)
                {
                    //decrement quantity
                    itemQuantities[item.Name]--;

                    //Delete key from quantity list if it hits 0
                    if(itemQuantities[item.Name]==0)
                    {
                        itemQuantities.Remove(item.Name);
                    }

                    //remove item from list
                    Item itemToRemove = items.Single(i => i.ID == item.ID);
                    items.Remove(itemToRemove);
                }
                else
                {
                    MessageBox.Show("Quantity already 0. Cannot remove item.");
                }
            }
            else
            {
                MessageBox.Show("Transaction list does not contain item with name: " + item.Name);
            }
        }

        public void Pay(Payment payMethod)
        {
            if(payMethod == Payment.Cash)
            {
                paymentMethod = Payment.Cash;
                MessageBox.Show("Paying with cash");
                PayWithCash(100);

            }
            else if (payMethod == Payment.MobilePay)
            {
                paymentMethod = Payment.MobilePay;
                MessageBox.Show("Paying with MobilePay");
            }
        }

        public bool PayWithCash(int cashAmount)
        {
            if (cashReceived >= totalPrice)
            {
                cashReceived = cashAmount;
                cashBack = cashReceived - totalPrice;
                MessageBox.Show("Paid cash with: " + cashReceived);
                return true;
            }
            else
                MessageBox.Show("Not enough cash.");
                return false;
        }

        public bool PayWithMobilePay()
        {
            MessageBox.Show("Paid with MobilePay.");
            return true;
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
