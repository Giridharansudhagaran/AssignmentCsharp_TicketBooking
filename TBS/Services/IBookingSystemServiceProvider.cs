using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Models;

namespace TBS.Services
{
    public interface IBookingSystemServiceProvider
    {
        void BookTickets(string eventName, int numTickets);
        void CancelBooking(int bookingId);
        Booking GetBookingDetails(int bookingId);
    }    
}
