using EllieApp.Views;

namespace EllieApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            bool isLoggedIn = Preferences.Get("isLoggedIn", defaultValue: false);
            if (isLoggedIn)
            {
                MainPage = new MainPage();
            }
            else
            {
                //Preferences.Set("isLoggedIn", true);
                MainPage = new LoginPage();
            }
        }
    }
}