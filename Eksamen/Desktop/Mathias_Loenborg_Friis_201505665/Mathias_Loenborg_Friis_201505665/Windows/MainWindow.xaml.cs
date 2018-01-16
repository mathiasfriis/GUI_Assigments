using Mathias_Loenborg_Friis_201505665.Models;
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
    }
}
