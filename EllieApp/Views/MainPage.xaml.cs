#if ANDROID
using Android.App;
using Android.Content;
#endif
using EllieApp.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text.Json;

namespace EllieApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Alarm> alarms;
        public MainPage()
        {
            BindingContext = this;
            InitializeComponent();
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
            collectionView.ItemsSource = alarms;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("https://deep-wealthy-roughy.ngrok-free.app/alarm");
            var jsonAlarms = JsonSerializer.Deserialize<ObservableCollection<Alarm>>(await response.Content.ReadAsStringAsync());
            alarms = jsonAlarms;
        }


        private async void LogOut_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Log out?", "Password for logging out is required");
            if (result == "Password")
            {
                Preferences.Set("isLoggedIn", false);
                Preferences.Set("username", "api data");
                App.Current.MainPage = new LoginPage();
            }
            else
            {
                await DisplayAlert("Log out failed", "The password was incorrect", "OK");
            }
        }
    }
}
