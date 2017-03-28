using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
            tbInput.Focus();
        }

        #region Event Handlers
        // When the Search button is clicked, the movie search executes.
        void ButtonClick(object sender, RoutedEventArgs e)
        {
            ExecuteMovieSearch();
        }

        // When the user hits the Enter key, the movie search executes.
        void CheckEnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExecuteMovieSearch();
        }
        #endregion


        // Runs a movie search on TMDb and displays the results using UI elements.
        void ExecuteMovieSearch()
        {
            resultsGrid.Children.Clear();
            SearchContainer<SearchMovie> movies = QuerySearchMovie(tbInput.Text);
            int count = 0;
            foreach (SearchMovie mov in movies.Results)
            {
                // if it doesn't even have a release date, probably not worth showing
                if (mov.ReleaseDate == null)
                    continue;

                // create a stack panel to hold this movie's data
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(10d);
                sp.Width = 250d;
                //sp.Background = Brushes.LightCyan;
                resultsGrid.Children.Add(sp);

                // place the title into the UI...
                TextBlock title = new TextBlock();
                title.Text = mov.Title;
                title.FontSize = 20d;
                title.TextWrapping = TextWrapping.Wrap;
                sp.Children.Add(title);

                // and then the release date...
                Label rDate = new Label();
                rDate.Content = mov.ReleaseDate;
                rDate.FontSize = 12d;
                sp.Children.Add(rDate);

                // and finally the synopsis/overview
                TextBlock overview = new TextBlock();
                overview.Text = mov.Overview;
                overview.HorizontalAlignment = HorizontalAlignment.Left;
                overview.TextWrapping = TextWrapping.Wrap;
                overview.FontSize = 10d;
                sp.Children.Add(overview);

                // i want to show posters directly fromthe search... but that's proving to be challenging/tedious
                //Image poster = GetImageFromPath(mov.PosterPath);
                //sp.Children.Add(poster);

                count++; // # of displayed movies
                tbInput.SelectAll(); // so user can immediately enter a new query
            }
            
            lbResultCount.Content = (count < 20) ? $"{count} entries found:" : $"Results cap reached! {count} entries found:";
            resultsGrid.Rows = 1 + count / 3;
            resultsGrid.Columns = 3;
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
