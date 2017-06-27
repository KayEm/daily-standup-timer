using System;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace DailyStandupTimer
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private DispatcherTimer _totalTimer;
        private bool _canExecute;
        private ICommand _actionCommand;
        private int _defaultCountdownTik = 60;
        private int _defaultTotalTik = 0;
        private int _tik;
        private int _totalTik;
        private string _countdownText;
        private string _totalText;
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
                return String.IsNullOrEmpty(_countdownText) ? Convert(_defaultCountdownTik) : _countdownText;
            }
            set
            {
                _countdownText = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CountdownText"));
            }
        }

        public string TotalTime
        {
            get
            {
                return String.IsNullOrEmpty(_totalText) ? Convert(_defaultTotalTik) : _totalText;
            }
            set
            {
                _totalText = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalTime"));
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
                SystemSounds.Beep.Play();
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
            _tik = _defaultCountdownTik;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            StartTotalTimer();

            BackColor = new SolidColorBrush(Colors.Green);
        }

        private void StartStopwatch()
        {
            _tik = 0;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(StopWatch_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            StartTotalTimer();
        }

        private void Stop()
        {
            _timer.Stop();
            _timer = null;

            StopTotalTimer();
        }

        protected void TotalTimer_Tick(object sender, EventArgs e)
        {
            TotalTime = Convert(_totalTik);
            _totalTik++;
        }

        private void StartTotalTimer()
        {
            if (_totalTimer == null)
            {
                _totalTimer = new DispatcherTimer();
                _totalTimer.Tick += new EventHandler(TotalTimer_Tick);
                _totalTimer.Interval = new TimeSpan(0, 0, 1);
            }

            _totalTimer.Start();
        }

        private void StopTotalTimer()
        {
            _totalTimer.Stop();
        }

        private string Convert(int value)
        {
            TimeSpan ts = TimeSpan.FromSeconds(value);
            return String.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds);
        }
    }
}
