using ProyectoFinalM.DataAccess;

namespace ProyectoFinalM.Views;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        var dbContext = new CinemaDbContext();
        user.Text = dbContext.Users.Count().ToString();
        user1.Text = dbContext.Users.Count().ToString();
        user2.Text = dbContext.Users.Count().ToString();
    }

    
}
