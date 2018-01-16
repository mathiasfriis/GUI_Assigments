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

namespace Mathias_Loenborg_Friis_201505665.Windows
{
    /// <summary>
    /// Interaction logic for payCashWindow.xaml
    /// </summary>
    public partial class payCashWindow : Window
    {        public payCashWindow()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (isNumeric(txtCash.Text))
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        public int cashPaid
        {
            get { return int.Parse(txtCash.Text); }
        }
       
        //Checks if string is numeric - Taken from https://stackoverflow.com/questions/894263/how-do-i-identify-if-a-string-is-a-number
        private bool isNumeric(string s)
        {
            return Regex.IsMatch(s, @"^\d+$");
        }
    }
}
