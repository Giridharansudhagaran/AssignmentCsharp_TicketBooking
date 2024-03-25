using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int EventId { get; set; }
        public int NumTickets { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime BookingDate { get; set; }

        // Default constructor
        public Booking()
        {

        }
        public Booking(int bookingId, int customerId, int eventId, int numTickets, decimal totalCost, DateTime bookingDate)
        {
            BookingId = bookingId;
            CustomerId = customerId;
            EventId = eventId;
            NumTickets = numTickets;
            TotalCost = totalCost;
            BookingDate = bookingDate;
        }

        public override string ToString()
        {
            return $"BookingID : {BookingId} CustomerID : {CustomerId} EventId : {EventId} NumTickets : {NumTickets} TotalCost : {TotalCost} BookingDate : {BookingDate}";

        }
    }
}
