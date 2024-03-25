using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Models;

namespace TBS.Services
{
    public interface  IEventServiceProvider
    {
        void CreateEvent(int eventId , string eventName, DateTime date, TimeSpan time, int venueId , int totalSeats, int availableSeats, decimal ticketPrice, event_type eventType);
        void GetEventDetailsByEventName(string eventName);
        int GetAvailableNoOfTickets(string eventName);
    }
}
