using EllieApp.Models;
using System.Text.Json;

namespace EllieApp.Views;

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
        HttpResponseMessage response = await httpClient.GetAsync("https://deep-wealthy-roughy.ngrok-free.app/institute");
        var jsonString = await response.Content.ReadAsStringAsync();
        var institutes = JsonSerializer.Deserialize<List<Institute>>(jsonString);
        placePicker.ItemsSource = institutes;
        placePicker.ItemDisplayBinding = new Binding("name");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (placePicker.SelectedItem != null && roomField.Text != "")
        {
            /*var quickObject = (place: placePicker.SelectedItem, room: roomField.Text);
            var jsonObject = JsonSerializer.Serialize(quickObject);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://deep-wealthy-roughy.ngrok-free.app/", new StringContent(jsonObject));
            User user = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Preferences.Set("isLoggedIn", true);
            Preferences.Set("firstName", user.FirstName);
            Preferences.Set("lastName", user.LastName);
            Preferences.Set("room", user.Room);
            Preferences.Set("active", user.Active);
            Preferences.Set("points", user.Points);
            Preferences.Set("contactPersonId", user.ContactPersonId);*/
            Preferences.Set("isLoggedIn", true);
            App.Current.MainPage = new AppShell();
        }
    }
}