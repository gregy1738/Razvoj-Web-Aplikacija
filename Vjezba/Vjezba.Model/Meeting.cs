using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{

    public enum MeetingType
    {
       InPerson,
       VideoCall
    }

    public enum MeetingStatus
    {
        Scheduled,
        Cancelled
    }

    public class Meeting
    {
        [Key]
        public int ID { get; set; }
        public MeetingType Type { get; set; }
        public MeetingStatus Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Location { get; set; }
        public string Comment { get; set; }
    }
}
