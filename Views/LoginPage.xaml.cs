namespace ProyectoFinalM.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    public async void OnComprar(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }


}