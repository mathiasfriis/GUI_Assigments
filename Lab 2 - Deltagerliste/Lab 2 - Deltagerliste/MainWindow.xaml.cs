using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lab_2___Deltagerliste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> listA = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            //read .csv-file and create a string for each person.            using (var reader = new StreamReader(@"../../02_deltagerliste.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    String lineString = "";
                    for (int i = 0; i<values.Length;i++)
                    {
                        lineString += values[i];
                        if(i!=6)
                        {
                            lineString += "\t";
                        }
                    }
                    listA.Add(lineString);
                }
            }

            //set itemSource for listBox to the list of strings.
            listBox.ItemsSource = listA;
        }
    }
}
