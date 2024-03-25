using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public enum event_type
    {
        Movie,
        Sports,
        Concert
    }

    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public int VenueId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public event_type EventType { get; set; }
        public Venue Venue { get; set; }
        public int? BookingId { get; set; }

        public Event()
        {

        }

        public Event(int eventId , string eventname, DateTime eventdate, TimeSpan eventtime,int venueId, int totalseats, int availableseats, decimal ticketprice, event_type eventtype)
        {
            EventId = eventId;
            EventName = eventname;
            EventDate = eventdate;
            EventTime = eventtime;
            VenueId = venueId;
            TotalSeats = totalseats;
            AvailableSeats = availableseats;
            TicketPrice = ticketprice;
            EventType = eventtype;
        }

        public override string ToString()
        {
            return $"Event ID : {EventId} EventName : {EventName} EventDate : {EventDate} EventTime : {EventTime}  VenueId: {VenueId} TotalSeats : {TotalSeats} AvailableSeats : {AvailableSeats} TicketPrice : {TicketPrice} {EventType} {BookingId}";
        }
    }
}
