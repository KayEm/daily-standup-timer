using System.Windows;

namespace DailyStandupTimerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;

            DataContext = new TimerViewModel();
        }
    }
}
