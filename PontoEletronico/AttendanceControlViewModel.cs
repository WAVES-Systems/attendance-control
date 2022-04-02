using AttendanceControl.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceControl
{
    public class AttendanceControlViewModel : INotifyPropertyChanged
    {
        private AttendanceEntryRepository attendanceEntryRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        public string EmployeeName => attendanceEntryRepository.Name;

        public Visibility _homeScreenVisibility = Visibility.Visible;
        public Visibility HomeScreenVisibility
        {
            get => _homeScreenVisibility;
            set
            {
                _homeScreenVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility _entryScreenVisibility = Visibility.Collapsed;
        public Visibility EntryScreenVisibility
        {
            get => _entryScreenVisibility;
            set
            {
                _entryScreenVisibility = value;
                OnPropertyChanged();
            }
        }

        public AttendanceControlViewModel()
        {
            attendanceEntryRepository = new AttendanceEntryRepository();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
