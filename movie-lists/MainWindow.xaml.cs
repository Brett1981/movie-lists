using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            
        }

        void ButtonClick(object sender, RoutedEventArgs e)
        {
            SearchContainer<SearchMovie> movies = QuerySearchMovie(tbQuery.Text);
            int count = 0;
            foreach( SearchMovie mov in movies.Results)
            {
                StackPanel sp = new StackPanel();
                resultsGrid.Children.Add(sp);

                Label title = new Label();
                title.Content = mov.Title;
                title.FontSize = 20d;
                sp.Children.Add(title);

                Label rDate = new Label();
                rDate.Content = mov.ReleaseDate;
                rDate.FontSize = 10d;
                sp.Children.Add(rDate);

                TextBlock overview = new TextBlock();
                overview.Text = mov.Overview;
                overview.Width = 200d;
                overview.HorizontalAlignment = HorizontalAlignment.Left;
                overview.TextWrapping = TextWrapping.Wrap;
                overview.FontSize = 8d;
                sp.Children.Add(overview);
                //Image poster = GetImageFromPath(mov.PosterPath);
                //sp.Children.Add(poster);

                count++;
            }
            resultsGrid.Rows = 1 + count / 2;
            resultsGrid.Columns = 2;
        }

        static SearchContainer<SearchMovie> QuerySearchMovie(string query)
        {
            string apiKey = ConfigurationManager.AppSettings["TMDb Key"];
            TMDbClient client = new TMDbClient(apiKey);
            return client.SearchMovieAsync(query).Result;
        }

        static Image GetImageFromPath(string posterPath)
        {
            string apiKey = ConfigurationManager.AppSettings["TMDb Key"];
            TMDbClient client = new TMDbClient(apiKey);

            Image poster = new Image();
            string imageSize = "w154";
            client.GetConfig();
            Uri uri = client.GetImageUrl(imageSize, posterPath);
            var request = WebRequest.CreateDefault(uri);
            byte[] buffer = new byte[4096];

            MemoryStream mem;
            using (mem = new MemoryStream())
                using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            mem.Write(buffer, 0, read);
                    }

            BitmapImage bmp = new BitmapImage();
            bmp.StreamSource = mem;
            poster.Source = bmp;
            return poster;
        } // needs work, low priority

        //static Uri GetImageBaseUri()
        //{
        //    string apiKey = ConfigurationManager.AppSettings["TMDb Key"];
        //    TMDbClient client = new TMDbClient(apiKey);
        //}

    }
}
