using CommunityToolkit.Mvvm.Messaging;
using NeedHelp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static SliderDemo1.LoginResponse;

namespace NeedHelp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentView
    {
        Stopwatch timer;
        TimeSpan watchMeTime;
        bool isWatchMeStart = false;
        bool isSOSActivated = false;
        bool timerValue;
        int seconds = 30;
        bool windowsTimer = true;
        int watchmetimeSeconds;
        int watchMeInSeconds;
        string pendingTime = "";
        double value;
        bool isStart = true;
        string emergencyTriggeredType = "false";
        //Indicates whether the value of the slider is increasing or decreasing
        bool IsIncrease = false;
        HomePageViewModel hpvm;
        bool isWatchMeCanceled = false;
        public HomePage()
        {
            InitializeComponent();
            hpvm = new HomePageViewModel();
            BindingContext = hpvm;

            WeakReferenceMessenger.Default.Register<DragCompletedMessage>(this, (r, m) =>
            {
                StartSlidingActivities();
            });
        }
        public async void SliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            try
            {
                if (isWatchMeCanceled)
                {
                    watchme_slider.Value = 0;
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    isWatchMeCanceled = false;
                    hpvm.preValue = 0;
                }
                else
                {
                    Slider slider = (Slider)sender;
                    IsIncrease = args.NewValue > args.OldValue;
                    if (!IsIncrease)
                    {
                        hpvm.InputTransparent = true;
                        slider.Value = args.OldValue;
                        value = args.OldValue;
                    }
                    else
                    {
                        hpvm.InputTransparent = false;
                        slider.Value = args.NewValue;
                        value = args.NewValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:>>" + ex);
            }
        }

        private void StartSlidingActivities()
        {
            if (!isWatchMeStart)
            {
                watchMeTime = TimeSpan.FromMinutes(value * 60);
                string[] time = watchMeTime.ToString().Split('.');
                string result = time[0].Substring(3);
                string timeInHHMMSS = time[0];
                watchMeInSeconds = Convert.ToInt32(TimeSpan.Parse(timeInHHMMSS).TotalSeconds);
                //Debug.WriteLine("result:>>" + result);
                //Debug.WriteLine("timeInHHMMSS:>>" + timeInHHMMSS);
                //Debug.WriteLine("watchMeInSeconds:>>" + watchMeInSeconds);
                watchme_timer_label.Text = "Watch Me emergency will be triggered in " + result + " minutes";
                slider_timer_label.Text = result;
                cancel_watchme_button.IsEnabled = true;
                cancel_watchme_button.BackgroundColor = Color.FromArgb("#1c98d7");
                cancel_watchme_button.TextColor = Colors.White;
                watchme_timer_label.IsVisible = true;
                slider_timer_label.IsVisible = true;
                watchme_timer_label.TextColor = Colors.Black;
                slider_timer_label.TextColor = Colors.Black;
                slider_timer_label.BackgroundColor = Colors.White;
                StartWatchMe();
            }
            else
            {
                watchMeTime = TimeSpan.FromMinutes(value * 60);
                string[] time = watchMeTime.ToString().Split('.');
                string result = time[0].Substring(3);
                Debug.WriteLine("result:>>" + result);
                DateTime endTime = DateTime.ParseExact(result, "mm:ss", null);
                DateTime startTime = DateTime.ParseExact(pendingTime, "mm:ss", null);
                TimeSpan difference = endTime - startTime;
                Debug.WriteLine("startTime:>>" + startTime);
                Debug.WriteLine("endTime:>>" + endTime);
                Debug.WriteLine("timedifference:>>" + difference);
                timer.Restart();
                if (difference.ToString() != "00:00:00")
                {
                    watchMeInSeconds = Convert.ToInt32(difference.TotalSeconds);
                    Debug.WriteLine("watchMeInSeconds:>>" + watchMeInSeconds);
                    //Start the API call from here.
                    string instanceId = Preferences.Default.Get("watchmeinstanceId", "");
                }
            }

            if (value != 0)
            {
                slider_timer_label.TranslateTo(watchme_slider.Value * ((watchme_slider.Width - 40) / watchme_slider.Maximum), 0, 100);
                //uncomment this section to activate the advanced slider
                //no need to uncomment the advanced_slider in xaml
                //if (value >= 0.166666667)
                //{
                //    watchme_slider.Maximum = 0.333333333;
                //}
                //else if (value < 0.165)
                //{
                //    watchme_slider.Maximum = 0.166666667;
                //}
                //watchme_slider.Value = value;

                //if (value >= 0.166666667)
                //{
                //    watchme_slider.IsVisible = false;
                //    advanced_slider.IsVisible = true;
                //    advanced_slider.Value = value;
                //    slider_timer_label.TranslateTo(advanced_slider.Value * ((advanced_slider.Width - 40) / advanced_slider.Maximum), 0, 100);
                //}
                //else if (value < 0.166666667)
                //{
                //    watchme_slider.IsVisible = true;
                //    advanced_slider.IsVisible = false;
                //    watchme_slider.Value = value;
                //    slider_timer_label.TranslateTo(watchme_slider.Value * ((watchme_slider.Width - 40) / watchme_slider.Maximum), 0, 100);
                //}
            }
            else
            {
                cancel_watchme_button.BackgroundColor = Colors.White;
                cancel_watchme_button.TextColor = Colors.Black;
                cancel_watchme_button.IsEnabled = false;
                watchme_timer_label.TextColor = Color.FromArgb("#ededed");
                slider_timer_label.TextColor = Color.FromArgb("#ededed");
                slider_timer_label.BackgroundColor = Color.FromArgb("#ededed");
            }
        }

        public async void StartWatchMe()
        {
            try
            {
                //start service for watch me starting
                isWatchMeStart = true;
                timer = new Stopwatch();
                timer.Start();
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    hpvm.InputTransparent = false;
                    string slidertime = slider_timer_label.Text;
                    string[] time1 = watchme_timer_label.Text.Split('.');
                    //string result1 = time1[0].Substring(3);
                    //Debug.WriteLine("result1:>>" + result1);

                    if (slidertime == ":00:00")
                    {
                        //CancelWatchMeFeature();
                        //call service for triggering the watch me 
                        timerValue = false;
                        watchme_slider.Value = 0;
                        isWatchMeStart = false;
                        cancel_watchme_button.BackgroundColor = Colors.White;
                        cancel_watchme_button.TextColor = Colors.Black;
                        cancel_watchme_button.IsEnabled = false;
                        slider_timer_label.TextColor = Color.FromArgb("#ededed");
                        slider_timer_label.BackgroundColor = Color.FromArgb("#ededed");
                    }
                    else
                    {
                        TimeSpan ts = timer.Elapsed;
                        string newTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                        var remainingTime = watchMeTime - TimeSpan.Parse(newTime);
                        string[] time = remainingTime.ToString().Split('.');
                        string result = time[0].Substring(3);
                        Debug.WriteLine("pending time:>>" + result);
                        pendingTime = result;
                        watchme_slider.Value = remainingTime.TotalMinutes / 60;
                        watchme_timer_label.Text = "Watch Me emergency will be triggered in " + result + " minutes";
                        slider_timer_label.Text = result;
                        timerValue = isWatchMeStart;
                        if (isSOSActivated && !isWatchMeStart)
                        {
                            watchme_timer_label.TextColor = Colors.Black;
                            watchme_timer_label.Text = "You can't activate the WatchMe now since the SOS is already activated.";
                        }
                    }
                    return timerValue;
                });
                string instanceId = "";
                isStart = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:>>" + ex);
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static long MillisecondsTimestamp(this DateTime date)
        {
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(date.ToUniversalTime() - baseDate).TotalMilliseconds;
        }
    }
}