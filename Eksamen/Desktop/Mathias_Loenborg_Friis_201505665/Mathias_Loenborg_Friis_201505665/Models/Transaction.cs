using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    [Serializable]
    public class Transaction: ObservableCollection<TransactionItem>, INotifyPropertyChanged
    {
        public ObservableCollection<TransactionItem> Items = new ObservableCollection<TransactionItem>();
        public Dictionary<int, int> itemQuantities = new Dictionary<int, int>();

        public string filename;

        public Transaction()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                Add(new TransactionItem(001, "Pølse med brød", 10,3));
                Add(new TransactionItem(002, "Hotdog", 20,2));
                AddItemExecute(new Item(002, "Hotdog", 20));
            }
        }

        #region Properties

        int currentIndex = -1;

        private int cashReceived { get; set; } = 0; //cash received if paymentMethod = cash
        private int cashBack { get; set; } = 0; //cash back if paymentMethod = cash
        private String paymentMethod { get; set; } = "Not paid";

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

        public String PaymentMethod
        {
            get { return paymentMethod; }
            set
            {
                if (paymentMethod != value)
                {
                    paymentMethod = value;
                    NotifyPropertyChanged();
                }
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

        public int CashReceived
        {
            get { return cashReceived; }
            set
            {
                if (cashReceived != value)
                {
                    cashReceived = value;
                    cashBack = cashReceived - totalPrice;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("CashBack");
                }
            }
        }

        public int CashBack
        {
            get { return cashBack; }
            set { cashBack = value; }
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
            get { return _removeItemCommand ?? (_removeItemCommand = new RelayCommand<Item>(RemoveItem)); }
        }

        private void RemoveItem(Item item)
        {
            //Check if Quantity 
            if (itemQuantities.ContainsKey(item.ID))
            {
                itemQuantities[item.ID]--;
                this.Single(i => i.ID == item.ID).Quantity--;
                this.Single(i => i.ID == item.ID).UpdateTotalPrice();
                // Remove from list if Quantity=0
                if (itemQuantities[item.ID]<=0)
                {
                    itemQuantities.Remove(item.ID);
                    Remove(this.Single(i => i.ID == item.ID));
                }
                if(itemQuantities.ContainsKey(item.ID))
                {
                    this.Single(i => i.ID == item.ID).UpdateTotalPrice();
                }
                NotifyPropertyChanged("totalPrice");

            }
            else
            {
                //create new item with quantity 1.
                MessageBox.Show("Cannot remove item from transaction.\nQuantity is already 0.");
            }

            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("totalPrice");
            CurrentIndex = Count - 1;
        }
        #endregion

        #region NewTransactionCommand
        ICommand _newTransactionCommand;
        public ICommand NewTransactionCommand
        {
            get
            {
                return _newTransactionCommand ?? (_newTransactionCommand = new RelayCommand(NewTransactionExecute, NewTransaction_CanExecute));
            }
        
        }

        private void NewTransactionExecute()
        {
            Clear();

            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("totalPrice");
            CurrentIndex = Count - 1;
        }

    private bool NewTransaction_CanExecute()
    {
            return true;
    }
    #endregion

    #region OpenTransactionCommand
    ICommand _OpenTransactionCommand;
        public ICommand OpenTransactionCommand
        {
            get { return _OpenTransactionCommand ?? (_OpenTransactionCommand = new RelayCommand<string>(OpenTransactionCommand_Execute)); }
        }

        private void OpenTransactionCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {

                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Transaction tempTransaction = new Transaction();
                tempTransaction.Clear();

                // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                XmlSerializer serializer = new XmlSerializer(typeof(Transaction));
                try
                {
                    TextReader reader = new StreamReader(filename);
                    CashReceived = int.Parse(reader.ReadLine());
                    CashBack = int.Parse(reader.ReadLine());
                    PaymentMethod = reader.ReadLine();
                    // Deserialize all items.
                    tempTransaction = (Transaction)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // We have to insert the items in the existing collection. 
                Clear();
                foreach (var Item in tempTransaction)
                {
                    Add(Item);
                    itemQuantities[Item.ID] = Item.Quantity;
                }


                NotifyPropertyChanged("totalPrice");
                NotifyPropertyChanged("Count");
                NotifyPropertyChanged("CashBack");
            }
        }
        #endregion

        #region SaveTransactionAsCommand
        ICommand _SaveTransactionAsCommand;
        public ICommand SaveTransactionAsCommand
        {
            get { return _SaveTransactionAsCommand ?? (_SaveTransactionAsCommand = new RelayCommand<string>(SaveAsCommand_Execute)); }
        }

        private void SaveAsCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {
                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                //filename = argFilename;
                SaveFileCommand_Execute();
            }
        }

        #endregion

        public void SaveFileCommand_Execute()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(Transaction));
            TextWriter writer = new StreamWriter(filename);
            writer.WriteLine(cashReceived);
            writer.WriteLine(cashBack);
            writer.WriteLine(paymentMethod);
            // Serialize all items.
            serializer.Serialize(writer, this);
            writer.Close();
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (filename != "") && (Count > 0);
        }

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
