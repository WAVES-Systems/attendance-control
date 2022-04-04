using AttendanceControl.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceControl.Repository
{
    internal class AttendanceEntryRepository
    {
        private string InternalDatabaseFileName => "database.db";
        private string FinalDatabaseFileName => "final_report.csv";
        private string Separator => ",";
        public AttendanceEntry LastAttendanceEntry { get; set; }

        /// <summary>
        /// Gets a value that represents the name of the current logged user.
        /// </summary>
        public string Name => System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;

        /// <summary>
        /// Stores a attendance entry in the database file.
        /// </summary>
        /// <param name="attendanceEntry">An object that represents a attendance entry.</param>
        /// <returns>A object that represents the resulting attendance entry.</returns>
        public AttendanceEntry Save(AttendanceEntry attendanceEntry)
        {
            try
            {
                if (attendanceEntry == null)
                {
                    throw new Exception("Input is null!");
                }
                attendanceEntry.Id = GetNextId();
                if (attendanceEntry.EntryType == EntryType.Out)
                {
                    WriteData(ConvertEntryToLine(LastAttendanceEntry, attendanceEntry),FinalDatabaseFileName);
                }
                WriteData(ConvertEntryToLine(attendanceEntry));
                LastAttendanceEntry = attendanceEntry;
                return attendanceEntry;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Retrieves the last registred attendance entry.
        /// </summary>
        /// <returns>An object that represents an attendance entry.</returns>
        public AttendanceEntry GetLastAttendanceEntry()
        {
            try
            {
                string data = TryRetrieveData().Replace("\"", String.Empty);

                if (string.IsNullOrEmpty(data))
                    return null;

                AttendanceEntry attendanceEntry = new AttendanceEntry();

                int commaPosition = data.IndexOf(Separator);
                attendanceEntry.Id = int.Parse(data.Substring(0, commaPosition));
                data = data.Remove(0, commaPosition + 1);

                commaPosition = data.IndexOf(Separator);
                double AODate = double.Parse(data.Substring(0, commaPosition));
                attendanceEntry.Timestamp = DateTime.FromOADate(AODate);
                data = data.Remove(0, commaPosition + 1);

                attendanceEntry.EntryType = (EntryType)Enum.Parse(typeof(EntryType), data);

                return attendanceEntry;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieve the next Id to be used in the new entry.
        /// </summary>
        /// <returns>A value that represents the next Id.<para/>
        /// If a new Id cannot be retrieved, 0 is returned.
        /// </returns>
        private int GetNextId()
        {
            string data = TryRetrieveData().Replace("\"", String.Empty);
            if (data.Contains(","))
            {
                int id = 0;
                int commaPosition = data.IndexOf(Separator);
                if (int.TryParse(data.Substring(0, commaPosition), out id))
                {
                    return id + 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Tries to retrieve the content from the database file. If the file does not exists, a new empty file will be created.
        /// </summary>
        /// <returns>All content from the database file.</returns>
        private string TryRetrieveData()
        {
            if (!File.Exists(InternalDatabaseFileName))
            {
                File.WriteAllText(InternalDatabaseFileName, "");
            }
            var result = File.ReadAllLines(InternalDatabaseFileName);
            if (result.Any())
                return result[result.Length-1];
            else
                return "";
        }

        /// <summary>
        /// Appends new data to the database file. If the file does not exists, a new file will be created.
        /// </summary>
        /// <param name="data">A string that represents the data to be written.</param>
        private void WriteData(string data)
        {
            if (!File.Exists(InternalDatabaseFileName))
            {
                File.WriteAllText(InternalDatabaseFileName, data);
            }
            else
            {
                File.AppendAllText(InternalDatabaseFileName, data+ Environment.NewLine);
            }
        }

        /// <summary>
        /// Appends new data to the database file. If the file does not exists, a new file will be created.
        /// </summary>
        /// <param name="data">A string that represents the data to be written.</param>
        private void WriteData(string data, string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, data);
            }
            else
            {
                File.AppendAllText(filePath, data + Environment.NewLine);
            }
        }

        /// <summary>
        /// Converts an attendance entry to a csv-line to be written in the database file.
        /// </summary>
        /// <param name="attendanceEntry">An object that represents an attendance entry.</param>
        /// <returns>A string that represents a csv-line. <para/>
        /// <see langword="null"/> if <paramref name="attendanceEntry"/> is <see langword="null"/>.</returns>
        private string ConvertEntryToLine(AttendanceEntry attendanceEntry)
        {
            if (attendanceEntry == null)
                return null;
            return $"\"{attendanceEntry.Id}\"{Separator}\"{attendanceEntry.Timestamp.ToOADate().ToString(CultureInfo.InvariantCulture)}\"{Separator}\"{Enum.GetName(typeof(EntryType), attendanceEntry.EntryType)}\"";
        }

        /// <summary>
        /// Converts two attendance entries into a csv-line to be written in a timekeeping table.
        /// </summary>
        /// <param name="inEntry">An object that represents the arrival entry</param>
        /// <param name="outEntry">An object that represents the departure entry</param>
        /// <returns>A string that represents the csv line with Date, Time In and Time Out.</returns>
        private string ConvertEntryToLine(AttendanceEntry inEntry, AttendanceEntry outEntry)
        {
            if (inEntry == null)
                return null;
            return $"\"{inEntry.Timestamp.ToOADate().ToString(CultureInfo.InvariantCulture)}\"{Separator}\"{inEntry.Timestamp.ToOADate().ToString(CultureInfo.InvariantCulture)}\"{Separator}\"{outEntry.Timestamp.ToOADate().ToString(CultureInfo.InvariantCulture)}\"";
        }
    }
}
