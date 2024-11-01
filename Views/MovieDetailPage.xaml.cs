
using ProyectoFinalM.DataAccess;

namespace ProyectoFinalM.Views;

public partial class MovieDetailPage : ContentPage, IQueryAttributable
{
	public MovieDetailPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var dbContext = new CinemaDbContext();
        var id = int.Parse(query["Id"].ToString());
        var movie = dbContext.Movies.First(p => p.Id == id);
        moviesContainer.Children.Add(new Label { Text = movie.Titulo});
        moviesContainer.Children.Add(new Label { Text = movie.Genero});
        moviesContainer.Children.Add(new Label { Text = movie.Duracion.ToString()});
        moviesContainer.Children.Add(new Label { Text = movie.Clasificacion});
    }
}