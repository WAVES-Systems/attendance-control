using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceControl.Models
{
    public class AttendanceEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public EntryType EntryType { get; set; }
    }
}
