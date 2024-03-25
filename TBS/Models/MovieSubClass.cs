using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class MovieSubClass:Event
    {
        public string Genre { get; set; }
        public string ActorName { get; set; }
        public string ActressName { get; set; }

        public MovieSubClass(int eventId, string eventname, DateTime eventdate, TimeSpan eventtime,int venueId, int totalseats, int availableseats, decimal ticketprice, event_type eventtype,string genre, string actorName, string actressName) 
            : base(eventId,eventname,eventdate,eventtime,venueId, totalseats, availableseats,ticketprice,eventtype)
        {
            Genre = genre;
            ActorName = actorName;
            ActressName = actressName;
        }
    }
}
