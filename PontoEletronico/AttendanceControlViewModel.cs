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
        public event PropertyChangedEventHandler PropertyChanged;

        private AttendanceEntry _lastAttendanceEntry;
        public AttendanceEntry LastAttendanceEntry
        {
            get { return _lastAttendanceEntry; }
            set { _lastAttendanceEntry = value; }
        }

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

        public string _lastEntryDate = "WAVES Systems";
        public string LastEntryDate
        {
            get => _lastEntryDate;
            set
            {
                _lastEntryDate = value;
                OnPropertyChanged();
            }
        }

        public string _entryTypeText= "WAVES Systems";
        public string EntryTypeText
        {
            get => _entryTypeText;
            set
            {
                _entryTypeText = value;
                OnPropertyChanged();
            }
        }

        public string _timeSinceLastEntry = "Attendance Control 1.0";
        public string TimeSinceLastEntry
        {
            get => _timeSinceLastEntry;
            set
            {
                _timeSinceLastEntry = value;
                OnPropertyChanged();
            }
        }

        public string _entryMessage = "Welcome!";
        public string EntryMessage
        {
            get => _entryMessage;
            set
            {
                _entryMessage = value;
                OnPropertyChanged();
            }
        }

        public string _entryTimeSpan = "Attendance Control";
        public string EntryTimeSpan
        {
            get => _entryTimeSpan;
            set
            {
                _entryTimeSpan = value;
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
            if (_lastAttendanceEntry != null)
            {
                LastEntryDate = $"{(_lastAttendanceEntry.EntryType == EntryType.In ? "Arrival" : "Departure")} at {_lastAttendanceEntry.Timestamp:HH:mm}, {_lastAttendanceEntry.Timestamp:d}";
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnFingerprintReceived()
        {
            AttendanceEntry attendanceEntry = new AttendanceEntry();
            attendanceEntry.Timestamp = DateTime.Now;
            attendanceEntry.EntryType = EntryType.In;
            if (_lastAttendanceEntry != null)
            {
                attendanceEntry.EntryType = _lastAttendanceEntry.EntryType == EntryType.Out ? EntryType.In : EntryType.Out;
                var currentTimePassed = attendanceEntry.Timestamp.Subtract(LastAttendanceEntry.Timestamp);
                EntryTimeSpan = $"Time span: {currentTimePassed.TotalHours.ToString("00")}:{currentTimePassed.Minutes.ToString("00")}:{currentTimePassed.Seconds.ToString("00")}";
            }
            _lastAttendanceEntry = _attendanceEntryRepository.Save(attendanceEntry);
            EntryTypeText = $"{(attendanceEntry.EntryType == EntryType.In ? "Arrival" : "Departure")} registred!{Environment.NewLine}{attendanceEntry.Timestamp:HH:mm}, {attendanceEntry.Timestamp:D}";
            EntryMessage = $"{(attendanceEntry.EntryType == EntryType.In ? "Welcome!" : "See you!")}";
            GetLastEntry();
        }
    }
}
