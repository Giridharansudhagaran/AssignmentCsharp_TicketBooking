using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class ConcertSubClass:Event
    {
        public string Artist { get; set; }
        public string Type { get; set; }

        public ConcertSubClass(int eventId, string eventname, DateTime eventdate, TimeSpan eventtime,int venueId,  int totalseats, int availableseats, decimal ticketprice, event_type eventtype,string artist, string type) 
            : base(eventId, eventname, eventdate, eventtime,venueId, totalseats, availableseats, ticketprice, eventtype)
        {
            Artist = artist;
            Type = type;
        }
    }
}
