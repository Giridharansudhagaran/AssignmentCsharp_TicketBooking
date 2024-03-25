using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class SportsSubClass:Event
    {
        public string SportsName { get; set; }
        public string TeamsName { get; set; }

        public SportsSubClass(int eventId ,string eventname, DateTime eventdate, TimeSpan eventtime , int venueId,int totalseats, int availableseats, decimal ticketprice, event_type eventtype, string sportsname, string teamsname)
            : base(eventId , eventname, eventdate, eventtime,venueId ,totalseats, availableseats, ticketprice, eventtype)
        {
            SportsName = sportsname;
            TeamsName = teamsname;
        }
    }
}
