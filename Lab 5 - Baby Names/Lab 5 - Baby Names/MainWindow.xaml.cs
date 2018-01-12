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

namespace Lab_5___Baby_Names
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<BabyName> babyNames = new List<BabyName>(); //Holds all data about all babynames.
        int selectedDecade = 2000; //Default value.

        Dictionary<int, BabyName[]> top10babyNamesByDecade = new Dictionary<int, BabyName[]>(); //Info about top 10 baby names by decade. Holds both boys and girls names.
        Dictionary<int, string[]> top10babyNamesStrings = new Dictionary<int, string[]>(); //Strings based on info about top 10 for bboth boys and girls.

        List<String> selectedNameOverYears = new List<string>(); //Strings based on popularity for the searched name over the years.

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LB_names.ItemsSource = top10babyNamesStrings[selectedDecade]; //init ListBox.
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CB_decade.Text!="") //for initial checking.
            {
                //Updates the selected decade and updates the ListBox accordingly.
                ComboBoxItem comboBoxItem = (ComboBoxItem)CB_decade.SelectedItem;
                selectedDecade = int.Parse(comboBoxItem.Content.ToString());
                //MessageBox.Show("selected decade: " + selectedDecade);
                LB_names.ItemsSource = top10babyNamesStrings[selectedDecade];
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //read all baby names on loadup.
            using (var reader = new StreamReader(@"../../../05-babynames.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    babyNames.Add(new BabyName(line));
                }
            }

            //create matrix of top 10 baby names pr. decade
            for (int i = 1900; i <= 2000; i += 10)
            {
                //MessageBox.Show("Calculating top 10 names for: " + i);

                //Make placeholder array for top 10 names (boys and girls. 1= place 0 & 10, 2 =1 & 11, etc..)
                BabyName[] top10ofTheDecade = new BabyName[20];
                foreach (BabyName BN in babyNames)
                {
                    int rank = BN.Rank(i); //get rank
                    if (rank <= 10 && rank != 0) //if in top 10
                    {
                        if (top10ofTheDecade[rank - 1] == null) // if first value holder is null.
                        {
                            top10ofTheDecade[rank - 1] = BN;
                        }
                        else
                        {
                            top10ofTheDecade[rank + 10 - 1] = BN;
                        }

                    }
                }
                top10babyNamesByDecade.Add(i, top10ofTheDecade); //add to matrix of top names.

                String[] strings = new String[10]; //create strings containing both boy and girl names.
                for (int j = 0; j < 10; j++)
                {
                    strings[j] = top10ofTheDecade[j].Name + " and " + top10ofTheDecade[j + 10].Name;
                }

                //add to matrix of name strings.
                top10babyNamesStrings.Add(i, strings);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String currentName = TB_searchName.Text.ToString(); //get name to search for
            MessageBox.Show("Searching for name: " + currentName);
            try {
                BabyName currentNameObject = babyNames.Find(x => x.Name == currentName); //find name object.
                TB_avg_rating.Text = currentNameObject.AverageRank().ToString(); //get average rating.

                //get rating over years.
                selectedNameOverYears.Clear();
                for (int i = 1900; i <= 2000; i += 10)
                {
                    selectedNameOverYears.Add(i + ": " + currentNameObject.Rank(i));
                }

                //update ListBox
                LB_nameOverYears.ItemsSource = selectedNameOverYears;

                //Get trend
                String trend = "";
                if (currentNameObject.Trend() == 0)
                {
                    trend = "Popularity stable";
                }
                else if (currentNameObject.Trend() < 0)
                {
                    trend = "Decreasing popularity";
                }
                else if (currentNameObject.Trend() > 0)
                {
                    trend = "Increasing popularity";
                }

                //update trend
                TB_trend.Text = trend;
            }
            catch //if no data for name is found
            {
                MessageBox.Show("No data for name: " + currentName);
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FontMenu_Small_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 10;
        }

        private void FontMenu_Normal_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 12;
        }

        private void FontMenu_Large_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 14;
        }

        private void FontMenu_Huge_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 16;
        }
    }
}
