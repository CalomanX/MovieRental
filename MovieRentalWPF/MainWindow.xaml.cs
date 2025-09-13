using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MovieRentalWPF.Models;
using MovieRentalWPF.Services;

namespace MovieRentalWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly MovieService _movieService;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize HttpClient and services
            _httpClient = new HttpClient();
            _movieService = new MovieService(_httpClient);
            
            
            // Load initial data
            LoadMovies();
        }

        private async void LoadMovies()
        {
            try
            {
                StatusTextBlock.Text = "Loading movies...";
                var movies = await _movieService.GetAllMoviesAsync();
                MoviesDataGrid.ItemsSource = movies;
                StatusTextBlock.Text = $"Loaded {movies.Count} movies";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error loading movies: {ex.Message}";
                MessageBox.Show($"Error loading movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMovies();
        }

        private async void AddMovieButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MovieTitleTextBox.Text))
                {
                    MessageBox.Show("Please enter a movie title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                StatusTextBlock.Text = "Adding movie...";
                var movie = new Movie { Title = MovieTitleTextBox.Text.Trim() };
                var createdMovie = await _movieService.AddMovieAsync(movie);
                
                MovieTitleTextBox.Clear();
                LoadMovies(); // Refresh the list
                StatusTextBlock.Text = $"Movie '{createdMovie.Title}' added successfully";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error adding movie: {ex.Message}";
                MessageBox.Show($"Error adding movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _httpClient?.Dispose();
            base.OnClosed(e);
        }
    }
}