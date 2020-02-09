using System;
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
            decadeTopNames = new ObservableCollection<string>();
            ListDecadeTopNames.ItemsSource = decadeTopNames;
        }

        private ObservableCollection<string> decadeTopNames;
        private List<BabyName> babyNames;

        private void ListDecadeTopNames_Loaded(object sender, RoutedEventArgs e)
        {
            string filePath = @".\babynames.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            
            string str;
            List<string> lines = new List<string>();
            int index = 0;

            //while (!sr.EndOfStream)
            while(index < 10)
            {
                str = sr.ReadLine();
                decadeTopNames.Add(str);
                index++;
            }
        }
    }
}
