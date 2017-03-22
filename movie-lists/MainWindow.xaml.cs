using System;
using System.Collections.Generic;
//using System.Configuration;
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
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace movie_lists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var results = QuerySearch("Batman");
        }

        static SearchContainer<SearchMovie> QuerySearch(string query)
        {
            string apiKey = "23d2932e34f9b78c0cd934523cf85cf3";
            TMDbClient client = new TMDbClient(apiKey);
            return client.SearchMovieAsync(query).Result;
        }

    }
}
