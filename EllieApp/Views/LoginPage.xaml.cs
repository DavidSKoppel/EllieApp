namespace EllieApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        Application.Current.UserAppTheme = AppTheme.Light;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetPlaces();
    }

    private async Task GetPlaces()
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync("https://dog.ceo/api/breeds/image/random");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync("https://dog.ceo/api/breeds/image/random");
        Preferences.Set("isLoggedIn", true);
        Preferences.Set("username", "api data");
        App.Current.MainPage = new MainPage();
        //await Navigation.PushAsync(new MainPage());
    }
}