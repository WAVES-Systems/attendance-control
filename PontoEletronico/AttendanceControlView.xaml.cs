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
        public AttendanceControlView()
        {
            InitializeComponent();
            _viewModel = new AttendanceControlViewModel();
            DataContext = _viewModel;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            TextBlock_time.Text = $"{DateTime.Now:HH:mm:ss}";
            TextBlock_date.Text = $"{DateTime.Now:D}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void Grid_main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
