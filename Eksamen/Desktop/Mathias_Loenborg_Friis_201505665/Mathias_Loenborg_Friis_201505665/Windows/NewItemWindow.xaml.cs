using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mathias_Loenborg_Friis_201505665
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        public NewItemWindow()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if(isNumeric(txtID.Text) && isNumeric(txtPrice.Text) && !isNumeric(txtName.Text))
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter valid information. \nID and Price must be numeric, and Name must contain at least 1 letter.");
            }
        }

        public int ID
        {
            get { return int.Parse(txtID.Text); }
        }

        public string Name
        {
            get { return txtName.Text; }
        }

        public int Price
        {
            get { return int.Parse(txtPrice.Text); }
        }

        //Checks if string is numeric - Taken from https://stackoverflow.com/questions/894263/how-do-i-identify-if-a-string-is-a-number
        private bool isNumeric(string s)
        {
            return Regex.IsMatch(s, @"^\d+$");
        }
    }
}
