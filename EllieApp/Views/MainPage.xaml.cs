#if ANDROID
using Android.App;
using Android.Content;
using EllieApp.Platforms.Android;
using Android.OS;
#endif
using EllieApp.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text.Json;

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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync("https://deep-wealthy-roughy.ngrok-free.app/alarm");
        var json = await response.Content.ReadAsStringAsync();
        var jsonAlarms = JsonSerializer.Deserialize<List<Alarm>>(json);
        alarms.Clear();
        foreach (var alarm in jsonAlarms)
        {
            alarms.Add(alarm);
        }
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
#if ANDROID
        if(!AndroidServiceManager.IsRunning)
        {
            AndroidServiceManager.StartMyService();
        }
        else
        {
        }
#endif
    }

    private void StopServiceButton_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidServiceManager.StopMyService();
#endif
    }

    private void StartServiceButton_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        AndroidServiceManager.StartMyService();
#endif
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

    private void Button_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var intent = new Android.Content.Intent(Android.App.Application.Context, typeof(CustomReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, intent, PendingIntentFlags.Immutable);
        intent.SetAction("AlarmReceived");
        intent.PutExtra("alarmIntent", pendingIntent);
        var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
        long interval = 60 * 10;//AlarmManager.IntervalDay;  60 *
        alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + interval, pendingIntent);
        // alarmManager.SetRepeating(AlarmType.RtcWakeup, startTime.Ticks, interval, pendingIntent);*/
#endif
    }
}
