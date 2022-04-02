using AttendanceControl.Models;
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
        private AttendanceEntryRepository _attendanceEntryRepository;
        private AttendanceEntry _lastAttendanceEntry;
        public event PropertyChangedEventHandler PropertyChanged;

        public string EmployeeName => _attendanceEntryRepository.Name;

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

        public string _lastEntryDate = " ";
        public string LastEntryDate
        {
            get => _lastEntryDate;
            set
            {
                _lastEntryDate = value;
                OnPropertyChanged();
            }
        }

        public AttendanceControlViewModel()
        {
            _attendanceEntryRepository = new AttendanceEntryRepository();
        }

        public void GetLastEntry()
        {
            _lastAttendanceEntry = _attendanceEntryRepository.GetLastAttendanceEntry();
            if(_lastAttendanceEntry != null)
                LastEntryDate = $"Last Entry: {_lastAttendanceEntry.Timestamp:HH:mm D}";
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
