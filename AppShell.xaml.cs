using ProyectoFinalM.Views;

namespace ProyectoFinalM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
            this.Navigating += AppShell_Navigating;
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(MovieDetailPage), typeof(MovieDetailPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(SalaPage), typeof(SalaPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MoviesPage), typeof(MoviesPage));
            Routing.RegisterRoute(nameof(VentasPage), typeof(VentasPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }

        private async void AppShell_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.Target.Location.OriginalString.Contains("login"))
            {
                MainTabBar.IsVisible = false;
            }
            else
            {
                MainTabBar.IsVisible = true;
            }
        }

        public static async Task LogoutAsync()
        {
            await Current.GoToAsync("//login");
        }
    }
}
