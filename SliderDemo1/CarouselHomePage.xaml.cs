using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using NeedHelp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

#if ANDROID
using Android.Views;
#endif


namespace NeedHelp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselHomePage : ContentPage
    {
        private bool AcceptBack;
        public static Action EmulateBackPressed;
        public CarouselHomePage()
        {
            InitializeComponent();
            ModifySlider();
        }

        void ModifySlider()
        {
            Microsoft.Maui.Handlers.SliderHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetOnTouchListener(new SliderListener());
#endif
            });
        }

#if ANDROID
        public class SliderListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            public bool OnTouch(global::Android.Views.View v, MotionEvent e)
            {
                if (e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Move)
                {
                    v.Parent.RequestDisallowInterceptTouchEvent(true);
                }
                else
                {
                    v.Parent.RequestDisallowInterceptTouchEvent(false);
                }
                return false;
            }
        }
#endif
    }
}