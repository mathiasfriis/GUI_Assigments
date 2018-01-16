using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    public class Transaction: ObservableCollection<TransactionItem>, INotifyPropertyChanged
    {
        public ObservableCollection<TransactionItem> Items = new ObservableCollection<TransactionItem>();
        public Dictionary<int, int> itemQuantities = new Dictionary<int, int>();

        public Transaction()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                Add(new TransactionItem(001, "Pølse med brød", 10,3));
                Add(new TransactionItem(002, "Hotdog", 20,2));
                AddItemExecute(new Item(002, "Hotdog", 20));
            }
            AddItemExecute(new Item(002, "Hotdog", 20));
            AddItemExecute(new Item(002, "Hotdog", 20));
            AddItemExecute(new Item(002, "Hotdog", 20));
            AddItemExecute(new Item(002, "Hotdog", 20));
        }

        #region Properties

        int currentIndex = -1;

        public int cashReceived { get; set; } = 0; //cash received if paymentMethod = cash
        public int cashBack { get; set; } = 0; //cash back if paymentMethod = cash
        ///Get total price of transaction.
        public int totalPrice
        {
            get
            {
                int sum = 0;
                foreach (TransactionItem i in this)
                {
                    sum += i.TotalPrice;
                }
                return sum;
            }
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (currentIndex != value)
                {
                    currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        Item currentItem = null;

        public Item CurrentItem
        {
            get { return currentItem; }
            set
            {
                if (currentItem != value)
                {
                    currentItem = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion
        

        #region Commands

        #region AddItemCommand
        ICommand _addItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                return _addItemCommand ?? (_addItemCommand = new RelayCommand<Item>(AddItemExecute));
            }
        }

        private void AddItemExecute(Item item)
        {
            //Check if item with items ID already exists.
            
            if(itemQuantities.ContainsKey(item.ID))
            {
                //increment quantity of item.
                itemQuantities[item.ID]++;
                this.Single(i => i.ID == item.ID).Quantity++;
                this.Single(i => i.ID == item.ID).UpdateTotalPrice();
                NotifyPropertyChanged("totalPrice");

            }
            else
            {
                //create new item with quantity 1.
                Add(new TransactionItem(item.ID, item.Name, item.Price, 1));
                itemQuantities.Add(item.ID, 1);
            }

            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("totalPrice");
            CurrentIndex = Count - 1;
        }
        #endregion

        #region DeleteItemCommand
        ICommand _removeItemCommand;
        public ICommand RemoveItemCommand
        {
            get { return _removeItemCommand ?? (_removeItemCommand = new RelayCommand(RemoveItem, DeleteItem_CanExecute)); }
        }

        private void RemoveItem()
        {
            RemoveAt(CurrentIndex);
            NotifyPropertyChanged("Count");
        }

        private bool DeleteItem_CanExecute()
        {
            if (Count > 0 && CurrentIndex >= 0)
                return true;
            else
                return false;
        }
        #endregion

        #endregion
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
