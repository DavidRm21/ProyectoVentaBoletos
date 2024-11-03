
namespace ProyectoFinalM.ViewModel;

public class LoginPageViewModel
{
    public async void OnComprar(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MoviesPage");
    }
}
