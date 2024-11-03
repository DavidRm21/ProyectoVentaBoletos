using ProyectoFinalM.DataAccess;
using System.Collections.ObjectModel;

namespace ProyectoFinalM.Views;

public partial class MoviesPage : ContentPage
{
    private readonly CinemaDbContext _dbContext;
    private ObservableCollection<Movie> _movies;
    private List<Movie> _allMovies;

    public MoviesPage()
    {
        InitializeComponent();
        _dbContext = new CinemaDbContext();
        LoadMovies();
    }

    private void LoadMovies()
    {
        _allMovies = _dbContext.Movies.ToList();
        _movies = new ObservableCollection<Movie>(_allMovies);
        moviesCollection.ItemsSource = _movies;
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
        {
            // Deseleccionar el item
            ((CollectionView)sender).SelectedItem = null;

            // Navegar al detalle
            var uri = $"//main/{nameof(MovieDetailPage)}?id={selectedMovie.Id}";
            await Shell.Current.GoToAsync(uri);
        }
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        FilterMovies(searchBar.Text);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterMovies(e.NewTextValue);
    }

    private void FilterMovies(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            _movies.Clear();
            foreach (var movie in _allMovies)
            {
                _movies.Add(movie);
            }
        }
        else
        {
            var filteredMovies = _allMovies
                .Where(m => m.Titulo.ToLower().Contains(searchText.ToLower()))
                .ToList();

            _movies.Clear();
            foreach (var movie in filteredMovies)
            {
                _movies.Add(movie);
            }
        }
    }
}