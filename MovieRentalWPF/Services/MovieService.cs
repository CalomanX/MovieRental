using System.Net.Http;
using System.Text;
using System.Text.Json;
using MovieRentalWPF.Models;

namespace MovieRentalWPF.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{App.ApiBaseUrl}/Movie");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var movies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return movies ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching movies: {ex.Message}", ex);
            }
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            try
            {
                var json = JsonSerializer.Serialize(movie);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{App.ApiBaseUrl}/Movie", content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                var createdMovie = JsonSerializer.Deserialize<Movie>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return createdMovie ?? movie;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding movie: {ex.Message}", ex);
            }
        }
    }
}