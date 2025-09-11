namespace MovieRental.Movie;

public interface IMovieFeatures
{
	Task<Movie> Save(Movie movie);
	Task<List<Movie>> GetAll();
}