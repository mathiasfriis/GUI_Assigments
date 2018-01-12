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

namespace Lab_4___Delopgave_1og2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Sailboat boat;
        public MainWindow()
        {
            InitializeComponent();
            boat = new Sailboat();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            boat.Name = TB_boatName.Text;
            boat.Length = double.Parse(TB_boatLength.Text);
            TB_speed.Text = boat.Hullspeed().ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String envDetails = "";
            envDetails += "Current Directory: " + Environment.CurrentDirectory + "\n";
            envDetails += "Machine name: " + Environment.MachineName + "\n";
            envDetails += "OS version: " + Environment.OSVersion + "\n";
            envDetails += "User Name: " + Environment.UserName + "\n";

            MessageBox.Show(envDetails);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boat.Name = TB_boatName.Text;   
            boat.Length = double.Parse(TB_boatLength.Text);
            MessageBox.Show("The name of the ship is: " + boat.Name);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //ctrl+L - Make text Larger
           if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key==Key.L)
            {
                Application.Current.MainWindow.FontSize++;
            }

            //ctrl+S - Make text Smaller
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Application.Current.MainWindow.FontSize--;
            }
        }
    }
}
