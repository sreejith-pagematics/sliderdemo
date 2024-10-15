using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliderDemo1
{
    class Utility
    {
        public static void ShowToast(string text, ToastDuration duration, double fontSize)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make(text, duration, fontSize);
                await toast.Show(cancellationTokenSource.Token);
            });
        }
    }
}
