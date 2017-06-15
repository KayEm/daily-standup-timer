using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace DailyStandupTimerWpfApp
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private bool _canExecute;
        private ICommand _actionCommand;
        private int _defaultTik = 60;
        private int _tik;
        private string _countdownText;
        private SolidColorBrush _backColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public TimerViewModel()
        {
            _canExecute = true;
            BackColor = new SolidColorBrush(Colors.Green);
        }

        public ICommand ActionCommand
        {
            get
            {
                return _actionCommand ?? (_actionCommand = new CommandHandler(() => DoAction(), _canExecute));
            }
        }

        public string CountdownText
        {
            get
            {
                return String.IsNullOrEmpty(_countdownText) ? Convert(_defaultTik) : _countdownText;
            }
            set
            {
                _countdownText = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CountdownText"));
            }
        }

        public SolidColorBrush BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackColor"));
            }
        }

        public void DoAction()
        {
            if (_timer == null)
            {
                StartTimer();
            }
            else
            {
                if (_timer.IsEnabled)
                {
                    Stop();
                }
                else
                {
                    StartTimer();
                }
            }
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            CountdownText = Convert(_tik);

            if (_tik > 0)
            {
                _tik--;
            }
            else
            {
                BackColor = new SolidColorBrush(Colors.Red);

                Stop();
                StartStopwatch();
            }
        }

        protected void StopWatch_Tick(object sender, EventArgs e)
        {
            CountdownText = Convert(_tik);
            _tik++;
        }

        private void StartTimer()
        {
            _tik = _defaultTik;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            BackColor = new SolidColorBrush(Colors.Green);
        }

        private void StartStopwatch()
        {
            _tik = 0;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(StopWatch_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Stop()
        {
            _timer.Stop();
            // _timer.Tick -= new EventHandler(Timer_Tick);
            _timer = null;
        }

        private string Convert(int value)
        {
            TimeSpan ts = TimeSpan.FromSeconds(value);
            return String.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds);
        }
    }
}
