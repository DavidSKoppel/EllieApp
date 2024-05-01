﻿#if ANDROID
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
                MainActivity.globalAlarms.Add(alarm);
            }
            StartAlarm(jsonAlarms);
        }
#endif
    }

    private void ShowAlarmButton_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var button = (Button)sender;
        var alarm = (Alarm)button.BindingContext;
        AlarmPopUp popup = new AlarmPopUp(alarm);
        this.ShowPopup(popup);
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

    private void StartAlarm(List<Alarm> alarms)
    {
#if ANDROID
        foreach (var alarm in alarms) { 
            var intent = new Android.Content.Intent(Android.App.Application.Context, typeof(CustomReceiver));
            var json = JsonSerializer.Serialize(alarm);
            intent.SetAction("AlarmReceived");
            intent.PutExtra("Alarm", json);
            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, alarm.id, intent, PendingIntentFlags.Immutable);
            var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);

            Java.Util.Calendar calendar = new Java.Util.Calendar.Builder().SetCalendarType("iso8601").Build();
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();

            // Set the alarm to trigger at 12:30 PM
            calendar.Set(Java.Util.CalendarField.HourOfDay, alarm.activatingTime.Hour);
            calendar.Set(Java.Util.CalendarField.Minute, alarm.activatingTime.Minute);
            calendar.Set(Java.Util.CalendarField.Second, alarm.activatingTime.Second);
            //long interval = alarm.id * 10000;
            if (calendar.TimeInMillis < Java.Lang.JavaSystem.CurrentTimeMillis())
            {
                // If the time has passed, add one day to the calendar to schedule the alarm for tomorrow
                calendar.Add(Java.Util.CalendarField.DayOfMonth, 1);
            }

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            //alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + interval, pendingIntent);
        }
#endif
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
