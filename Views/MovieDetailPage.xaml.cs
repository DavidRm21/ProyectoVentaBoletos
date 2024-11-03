
using ProyectoFinalM.DataAccess;
using System.Text.Json;
using System.Web;

namespace ProyectoFinalM.Views;

public partial class MovieDetailPage : ContentPage, IQueryAttributable
{
    int movieId = 0;
	public MovieDetailPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            var dbContext = new CinemaDbContext();
            var id = int.Parse(query["id"].ToString());
            Movie movie = dbContext.Movies.FirstOrDefault(p => p.Id == id);

            if (movie == null)
            {
                DisplayAlert("Error", "Película no encontrada", "OK");
                return;
            }
            movieId = id;

            // Actualizar la UI con los datos de la película
            Title = "Película: " + movie.Titulo;
            TituloLabel.Text = movie.Titulo;
            GeneroLabel.Text = movie.Genero;
            DuracionLabel.Text = $"{movie.Duracion} minutos";
            ClasificacionLabel.Text = movie.Clasificacion;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Error al cargar los detalles de la película: " + ex.Message, "OK");
        }
    }

    private async void OnComprarClicked(object sender, EventArgs e)
    {

        var uri = $"//main/{nameof(SalaPage)}?id={movieId}";
        await Shell.Current.GoToAsync(uri);
    }

}