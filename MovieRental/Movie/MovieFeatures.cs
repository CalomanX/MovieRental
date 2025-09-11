using Microsoft.EntityFrameworkCore;

using MovieRental.Data;

namespace MovieRental.Movie
{
    public class MovieFeatures : IMovieFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        public MovieFeatures(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public async Task<Movie> Save(Movie movie)
        {
            await _movieRentalDb.Movies.AddAsync(movie);
            await _movieRentalDb.SaveChangesAsync();
            return movie;
        }

        /*
		 * Blob of data
		 * 
		 * GetAll is a method that can return a lot of data, mostly unnecessary.
		 * 
		 * If you need all the fields its fine but think if you really require all the records. Think about pagination or filtering.
		 * Although databases are really fast, transferring a lot of data is not. Also remember that data is nor usually being returned as binary but mostly as text (json/xml)
		 * wich increses the payload size.
		 * Think about the target for this data. If you are builing complex UIs for desktop or mobile, all the time required to build and show will strongly impact the user experience.
		 * Also consider the memory and allocations required to handle large blobs of data.
		 */
        public async Task<List<Movie>> GetAll()
        {
            return await _movieRentalDb.Movies.ToListAsync();
        }


    }
}
