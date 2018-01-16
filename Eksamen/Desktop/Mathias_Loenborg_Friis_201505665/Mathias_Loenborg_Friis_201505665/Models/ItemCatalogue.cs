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
    public class ItemCatalogue : ObservableCollection<Item>, INotifyPropertyChanged
    {
        public List<Item> Transactions = new List<Item>();

        public ItemCatalogue()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                Add(new Item(001, "Pølse med brød", 10));
                Add(new Item(002, "Hotdog", 20));
                Add(new Item(003, "Fransk Hotdog", 15));
                Add(new Item(004, "Cocio", 15));
                Add(new Item(005, "Sodavand", 15));
                Add(new Item(006, "100g rent guld", 10000));
            }
        }

        #region Properties

        int currentIndex = -1;

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

        #region NewItemCommand
        ICommand _newItemCommand;
        public ICommand NewItemCommand
        {
            get
            {
                return _newItemCommand ?? (_newItemCommand = new RelayCommand<Item>(AddItemExecute));
            }
        }

        private void AddItemExecute(Item item)
        {
            if(item==null)
            {
                Add(new Item(0, "N/A", 0));
            }
            else
            {
                Add(item);
            }
            NotifyPropertyChanged("Count");
            CurrentIndex = Count - 1;
        }
        #endregion

        #region DeleteItemCommand
        ICommand _deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get { return _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand(DeleteItem, DeleteItem_CanExecute)); }
        }

        private void DeleteItem()
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
