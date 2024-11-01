using ProyectoFinalM.DataAccess;

namespace ProyectoFinalM.Views;

public partial class MoviesPage : ContentPage
{
	public MoviesPage()
	{
		InitializeComponent();
		var dbContext = new CinemaDbContext();

		foreach(var movie in dbContext.Movies)
		{
			var button = new Button { Text = movie.Titulo};
			button.Clicked += async (s, a) =>
			{
				var uri = $"{nameof(MovieDetailPage)}?id{movie.Id}";
				await Shell.Current.GoToAsync(uri);
			};

			container.Children.Add(button);
		}
	}
}