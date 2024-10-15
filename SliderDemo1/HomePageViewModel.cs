using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SliderDemo1;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using static SliderDemo1.LoginResponse;

namespace NeedHelp.Model
{
    public partial class HomePageViewModel : INotifyPropertyChanged
    {
        private int _days;
        private int _hours;
        private int _minutes;
        private string _seconds;
        private double _progress;
        public double preValue;
        public HomePageViewModel()
        {
            
        }

        private bool _inputTransparent;
        public bool InputTransparent
        {
            set
            {
                SetProperty(ref _inputTransparent, value);
            }
            get { return _inputTransparent; }
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;


        public int Days
        {
            get => _days;
            set => SetProperty(ref _days, value);
        }

        public int Hours
        {
            get => _hours;
            set => SetProperty(ref _hours, value);
        }

        public int Minutes
        {
            get => _minutes;
            set => SetProperty(ref _minutes, value);
        }

        public string Seconds
        {
            get => _seconds;
            set => SetProperty(ref _seconds, value);
        }

        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public ICommand RestartCommand => new Command(Restart);

        //public ICommand DragCompletedCommand => new Command(OnDragCompleted);

        [RelayCommand]
        public void DragCompleted(object obj)
        {
            var slider = obj as Slider;
            Debug.WriteLine("slider.Value:>>" + slider.Value);
            Debug.WriteLine("preValue:>>" + preValue);
            if (slider.Value > preValue)
            {
                Debug.WriteLine("do some work");
                WeakReferenceMessenger.Default.Send(new DragCompletedMessage("DragCompleted"));
                preValue = slider.Value;
            }
            else
            {
                Utility.ShowToast("Only time extension is allowed!", ToastDuration.Short, 14);
            }
        }
        void Restart()
        {
            try
            {
                Debug.WriteLine("Restart");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:>>" + ex);
            }
        }
    }
}
