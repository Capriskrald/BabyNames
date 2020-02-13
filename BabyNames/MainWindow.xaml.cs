using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BabyNames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            babyNames = new List<BabyName>();
            
            decadeTopNames = new ObservableCollection<string>();
            ListDecadeTopNames.ItemsSource = decadeTopNames;

            yearRank = new ObservableCollection<string>();
            YearRankBox.ItemsSource = yearRank;
        }

        private ObservableCollection<string> decadeTopNames;
        private ObservableCollection<string> yearRank;
        private List<BabyName> babyNames;
        private string[,] rankMatrix = new string[11, 10];

        private void ListDecadeTopNames_Loaded(object sender, RoutedEventArgs e)
        {
            bool fileFound = true;
            string filePath = @".\babynames.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);

            BabyName babyName;

            try
            {
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    decadeTopNames.Add(str);
                    babyName = new BabyName(str);
                    babyNames.Add(babyName);
                }
            }

            catch
            {
                MessageBox.Show("Error loading baby names");
                fileFound = false;
            }

            if(fileFound)
            {
                foreach (BabyName name in babyNames)
                {
                    for (int decade = 1900; decade <= 2000; decade += 10)
                    {
                        int rank = name.Rank(decade);
                        if (rank > 0 && rank < 11)
                        {
                            int decadeIndex = (decade - 1900) / 10;
                            if (rankMatrix[decadeIndex, rank - 1] == null)
                                rankMatrix[decadeIndex, rank - 1] = name.Name;
                            else
                                rankMatrix[decadeIndex, rank - 1] += " and " + name.Name;
                        }
                    }
                }
            }
            
        }

        private void ListDecades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ListDecades.SelectedItem as ListBoxItem;
            int decade = Convert.ToInt32(lbi.Content);
            int decadeIndex = (decade - 1900) / 10;
            decadeTopNames.Clear();

            for (int i = 1; i < 11; i++)
            {
                decadeTopNames.Add(string.Format("{0} {1}", i, rankMatrix[decadeIndex, i - 1]));
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;

            int index = 0;
            while (babyNames[index].Name != name)
                index++;

            yearRank.Clear();
            
            for(int decade = 1900; decade <= 2000; decade += 10)
                yearRank.Add(string.Format("{0} \t {1}", decade, babyNames[index].Rank(decade)));

            AverageRankBox.Text = babyNames[index].AverageRank().ToString();
            TrendBox.Text = babyNames[index].Trend().ToString();

            //string name = NameBox.Text;

            //int index = 0;
            //while (babyNames[index].Name != name)
            //    index++;

            //yearRank.Clear();
            //int averageRank = 0;
            //int timesOnRankList = 0;
            //for (int decade = 1900; decade <= 2000; decade += 10)
            //{
            //    int rank = babyNames[index].Rank(decade);
            //    if (rank > 0)
            //    {
            //        averageRank += rank;
            //        timesOnRankList++;
            //    }
            //    yearRank.Add(string.Format("{0} \t {1}", decade, rank));
            //}

            //averageRank = averageRank / timesOnRankList;
            //AverageRankBox.Text = averageRank.ToString();
        }
    }
}
