using Mathias_Loenborg_Friis_201505665.Models;
using Mathias_Loenborg_Friis_201505665.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mathias_Loenborg_Friis_201505665
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string PaymentMethod = cbPaymentMethod.Text.ToString();

                Transaction currentTransaction = (Transaction)Resources["currentTransaction"];

                currentTransaction.PaymentMethod = PaymentMethod;

                if (PaymentMethod == "Cash")
                {
                    //create Window to prompt user for data.
                    var payCashWindow = new payCashWindow();

                    //if 'OK' pressed
                    if (payCashWindow.ShowDialog() == true)
                    {
                        //Get data from create-window.
                        int cashPaid = payCashWindow.cashPaid;
                        currentTransaction.CashReceived = cashPaid;

                    }
                }

            }
            catch
            {
                MessageBox.Show("Please choose payment method");
            }
        }

        private void Menu_NewItem_Click(object sender, RoutedEventArgs e)
        {
            //create Window to prompt user for data.
            var createItemWindow = new NewItemWindow();

            //if 'OK' pressed
            if (createItemWindow.ShowDialog() == true)
            {
                //Get data from create-window.
                int ID = createItemWindow.ID;
                string Name = createItemWindow.Name;
                int Price = createItemWindow.Price;

                //Create new item with data.
                Item item = new Item(ID, Name, Price);

                //Get item catalogue from XAML and add new item.
                ItemCatalogue items = (ItemCatalogue)Resources["itemCatalogue"];
                items.NewItemCommand.Execute(item);
            }
        }

        private void addItemToTransActionButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Adding item to Current Transaction.");
        }

        private void removeItemFromTransActionButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Removing item from Current Transaction.");
        }

        private void Menu_EditItem_Click(object sender, RoutedEventArgs e)
        {
            ItemCatalogue items = (ItemCatalogue)Resources["itemCatalogue"];

            int index = items.CurrentIndex;

            //create Window to prompt user for data.
            var editItemWindow = new EditItemWindow(items[index]);

            //if 'OK' pressed
            if (editItemWindow.ShowDialog() == true)
            {
                //Get data from create-window.
                int ID = editItemWindow.ID;
                string Name = editItemWindow.Name;
                int Price = editItemWindow.Price;

                //Create new item with data.
                Item item = new Item(ID, Name, Price);

                //Replace item with edited item.
                items[index] = item;
            }
        }

        private void _menuItemSaveCatalogueAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog()==true)
            {
                string filename = saveFileDialog.FileName.ToString();
                ItemCatalogue catalogue = getItemCatalogue();
                catalogue.filename = filename;

            }
        }

        private void _menuItemOpenCatalogue(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName.ToString();
                ItemCatalogue catalogue = getItemCatalogue();
                catalogue.filename = filename;
            }
        }

        private void _menuItemSaveTransactionAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string filename = saveFileDialog.FileName.ToString();
                Transaction transaction = getCurrentTransaction();
                transaction.filename = filename;

            }
        }

        private void _menuItemOpenTransaction(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName.ToString();
                Transaction transaction = getCurrentTransaction();
                transaction.filename = filename;
            }
        }

        ItemCatalogue getItemCatalogue()
        {
            return (ItemCatalogue)Resources["itemCatalogue"];
        }

        Transaction getCurrentTransaction()
        {
            return (Transaction)Resources["currentTransaction"];
        }

        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
