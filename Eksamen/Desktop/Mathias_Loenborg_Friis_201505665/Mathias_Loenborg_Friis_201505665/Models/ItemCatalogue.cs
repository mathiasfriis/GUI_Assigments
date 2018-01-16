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
    public class ItemCatalogue : ObservableCollection<Item>, INotifyPropertyChanged
    {
        public List<Item> Items = new List<Item>();

        public string filename;

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

        #region EditItemCommand
        ICommand _editItemCommand;
        public ICommand EditItemCommand
        {
            get
            {
                return _editItemCommand ?? (_editItemCommand = new RelayCommand<Item>(EditItemExecute));
            }
        }

        private void EditItemExecute(Item item)
        {
            if (item == null)
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

        #region OpenCatalogueCommand
        ICommand _OpenCatalogueCommand;
        public ICommand OpenCatalogueCommand
        {
            get { return _OpenCatalogueCommand ?? (_OpenCatalogueCommand = new RelayCommand<string>(OpenCatalogueCommand_Execute)); }
        }

        private void OpenCatalogueCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {

                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                ItemCatalogue tempCatalogue = new ItemCatalogue();
                tempCatalogue.Clear();

                // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                XmlSerializer serializer = new XmlSerializer(typeof(ItemCatalogue));
                try
                {
                    TextReader reader = new StreamReader(filename);
                    // Deserialize all items.
                    tempCatalogue = (ItemCatalogue)serializer.Deserialize(reader);
                    foreach(Item i in tempCatalogue)
                    {
                        MessageBox.Show(i.Name);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // We have to insert the items in the existing collection. 
                Clear();
                foreach (var Item in tempCatalogue)
                    Add(Item);

                NotifyPropertyChanged("Count");
            }
        }
        #endregion

        #region SaveCatalogueAsCommand
        ICommand _SaveCatalogueAsCommand;
        public ICommand SaveCatalogueAsCommand
        {
            get { return _SaveCatalogueAsCommand ?? (_SaveCatalogueAsCommand = new RelayCommand<string>(SaveAsCommand_Execute)); }
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
            XmlSerializer serializer = new XmlSerializer(typeof(ItemCatalogue));
            TextWriter writer = new StreamWriter(filename);
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
