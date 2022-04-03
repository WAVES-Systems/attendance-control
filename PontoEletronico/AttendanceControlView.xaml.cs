using AttendanceControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AttendanceControl
{
    public partial class AttendanceControlView : Window
    {
        private AttendanceControlViewModel _viewModel;

        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer _entryDisplayCount = new DispatcherTimer();
        public AttendanceControlView()
        {
            InitializeComponent();
            _viewModel = new AttendanceControlViewModel();
            DataContext = _viewModel;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += _timer_Tick;
            _entryDisplayCount.Interval = TimeSpan.FromSeconds(5);
            _entryDisplayCount.Tick += _entryDisplayCount_Tick;
        }

        private void _entryDisplayCount_Tick(object sender, EventArgs e)
        {
            _entryDisplayCount.Stop();
            _viewModel.EntryScreenVisibility = Visibility.Collapsed;
            _viewModel.HomeScreenVisibility = Visibility.Visible;
            Retangle_Fingerprint.Visibility = Visibility.Visible;
            _entryDisplayCount.IsEnabled = true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            TextBlock_time.Text = $"{DateTime.Now:HH:mm:ss}";
            TextBlock_date.Text = $"{DateTime.Now:D}";
            if (_viewModel.LastAttendanceEntry != null)
            {
                var currentTimePassed = DateTime.Now.Subtract(_viewModel.LastAttendanceEntry.Timestamp);
                TextBlock_timeSinceLast.Text = $"Time span: {currentTimePassed.TotalHours.ToString("00")}:{currentTimePassed.Minutes.ToString("00")}:{currentTimePassed.Seconds.ToString("00")}";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _viewModel.GetLastEntry();
            _viewModel.EntryScreenVisibility = Visibility.Collapsed;
            _viewModel.HomeScreenVisibility = Visibility.Visible;
            Retangle_Fingerprint.Focusable = true;
            Retangle_Fingerprint.Focus();
        }

        private void Retangle_Fingerprint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _entryDisplayCount.IsEnabled = false;
            _viewModel.EntryScreenVisibility = Visibility.Collapsed;
            _viewModel.HomeScreenVisibility = Visibility.Collapsed;
            _viewModel.OnFingerprintReceived();
            _viewModel.EntryScreenVisibility = Visibility.Visible;
            _entryDisplayCount.Start();
            Retangle_Fingerprint.Visibility = Visibility.Collapsed;

        }

        private void Grid_main_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
