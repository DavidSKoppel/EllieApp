#if ANDROID
using Android.Content;
using Android.Widget;
#endif
using EllieApp.Models;
using EllieApp.Platforms.Android;
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
        HttpResponseMessage response1 = await httpClient.GetAsync("https://totally-helpful-krill.ngrok-free.app/institute");
        var jsonString = await response1.Content.ReadAsStringAsync();
        var institutes = JsonSerializer.Deserialize<List<Institute>>(jsonString);

        placePicker.ItemsSource = institutes;
        placePicker.ItemDisplayBinding = new Binding("name");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (placePicker.SelectedItem != null && roomPicker.SelectedItem != null)
        {
            var quickObject = (Institute)roomPicker.SelectedItem;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://totally-helpful-krill.ngrok-free.app/User/AppUserLogin?roomId=" + quickObject.id, null);
            if (response.IsSuccessStatusCode)
            {
                AndroidServiceManager.StartMyNetworkService();
                User user = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
                Preferences.Set("id", user.Id);
                Preferences.Set("firstName", user.FirstName);
                Preferences.Set("lastName", user.LastName);
                Preferences.Set("points", user.Points.ToString());
                Preferences.Set("token", user.Token);
                Preferences.Set("isLoggedIn", true);
                App.Current.MainPage = new AppShell();
            } else
            {
                await DisplayAlert("Log in failed", "Nobody was found", "OK");
            }
        }
    }

    private async void placePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpClient httpClient = new HttpClient();
        var selected = (Institute)placePicker.SelectedItem;
        HttpResponseMessage response2 = await httpClient.GetAsync("https://totally-helpful-krill.ngrok-free.app/Room/byinstid?id=" + selected.id);
        var jsonString = await response2.Content.ReadAsStringAsync();
        var rooms = JsonSerializer.Deserialize<List<Institute>>(jsonString);
        rooms = rooms.OrderBy(o => o.name).ToList();

        roomPicker.ItemsSource = rooms;
        roomPicker.ItemDisplayBinding = new Binding("name");

        roomPicker.IsEnabled = true;
    }
}