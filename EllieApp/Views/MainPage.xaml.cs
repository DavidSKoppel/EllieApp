#if ANDROID
using Android.App;
using Android.Content;
using EllieApp.Platforms.Android;
using Android.OS;
#endif
using EllieApp.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading;
using EllieApp.CustomPopUp;
using CommunityToolkit.Maui.Views;

namespace EllieApp.Views;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Alarm> alarms = new ObservableCollection<Alarm>();
    public MainPage()
    {
        BindingContext = this;
        InitializeComponent();
        this.Loaded += MainPage_Loaded;
        Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
        collectionView.ItemsSource = alarms;
        firstNameLabel.Text = Preferences.Get("firstName", defaultValue: "null");
        lastNameLabel.Text = Preferences.Get("lastName", defaultValue: "null");
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
#if ANDROID
        if (alarms.Count == 0)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response =
            await httpClient.GetAsync("https://totally-helpful-krill.ngrok-free.app/UserAlarmRelation/GetAlarmsByUserId/id?id=" + Convert.ToInt32(Preferences.Get("id", defaultValue: 1)));
            var json = await response.Content.ReadAsStringAsync();
            var jsonAlarms = JsonSerializer.Deserialize<List<Alarm>>(json);
            alarms.Clear();
            foreach (var alarm in jsonAlarms)
            {
                alarms.Add(alarm);
            }
        }
#endif
    }

    private void ShowAlarmButton_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidServiceManager.StartMyNetworkService();
#endif
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Log out?", "Password for logging out is required");
        if (result == "Password")
        {
            AndroidServiceManager.StopMyAlarms();
            Preferences.Set("id", "");
            Preferences.Set("firstName", "");
            Preferences.Set("lastName", "");
            Preferences.Set("points", "");
            Preferences.Set("token", "");
            Preferences.Set("isLoggedIn", false);
            App.Current.MainPage = new LoginPage();
        }
        else
        {
            await DisplayAlert("Log out failed", "The password was incorrect", "OK");
        }
    }

    /*private void StartButton_OnClicked(object sender, EventArgs e)
    {
        _startTime = DateTime.Now;
        _cancellationTokenSource = new CancellationTokenSource();
        UpdateArc();
    }

    private async void UpdateArc()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            var elapsedTime = (DateTime.Now - _startTime);
            int secondsRemaining = (int)(_duration - elapsedTime.TotalMilliseconds) / 1000;

            ProgressButton.Text = $"{secondsRemaining}";

            // More stuff to come ...

            await Task.Delay(500);
        }

        // Reset the view
    }*/
}
