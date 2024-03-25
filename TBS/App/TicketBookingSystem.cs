using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Bean;
using TBS.Models;
using TBS.Services;

namespace TBS.App
{
    internal class TicketBookingSystem
    {

        public TicketBookingSystem()
        {
            
        }
        public void App()
        {
            BookingSystemRepositoryImpl bookingSystemRepositoryImpl = new BookingSystemRepositoryImpl();


            Console.WriteLine("Welcome to the Ticket Booking System!");

            int userInput;
            do
            {
                Console.WriteLine("1: Create a event \n2: Book tickets \n3: Cancel tickets \n4: Get Booking Details \n5: Get event details \n6: Exit\n");
                userInput = int.Parse(Console.ReadLine());

                switch (userInput)
                {
                    case 1:
                        Console.WriteLine("Enter event details:");
                        Console.Write("Event Event ID: ");
                        int eventId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Event Name: ");
                        string eventName = Console.ReadLine();
                        Console.Write("Event Date (YYYY-MM-DD): ");
                        DateTime eventDate = Convert.ToDateTime(Console.ReadLine());
                        Console.Write("Event Time (HH:MM): ");
                        TimeSpan eventTime = TimeSpan.Parse(Console.ReadLine());
                        Console.Write("Venue Id: ");
                        int venueId = int.Parse(Console.ReadLine());
                        Console.Write("Total Seats: ");
                        int totalSeats = int.Parse(Console.ReadLine());
                        Console.Write("Available Seats: ");
                        int availableSeats = int.Parse(Console.ReadLine());
                        Console.Write("Ticket Price: ");
                        decimal ticketPrice = decimal.Parse(Console.ReadLine());
                        Console.Write("Event Type (Movie, Sports, Concert): ");
                        event_type eventType = Enum.Parse<event_type>(Console.ReadLine());

                        bookingSystemRepositoryImpl.CreateEvent(eventId,eventName, eventDate, eventTime,venueId,totalSeats, availableSeats, ticketPrice, eventType);
                        break;

                    case 2:
                        Console.WriteLine("Enter the Event Name for Booking:");
                        string eventname = Console.ReadLine();
                        Console.WriteLine("Enter the number of Tickets to Book:");
                        int numTickets = Convert.ToInt32(Console.ReadLine());
                       
                        bookingSystemRepositoryImpl.BookTickets(eventname, numTickets);
                        break;

                    case 3:
                        Console.WriteLine("Enter booking Id to cancel");
                        int id = Convert.ToInt32(Console.ReadLine());
                        bookingSystemRepositoryImpl.CancelBooking(id);
                        break;

                    case 4:
                        Console.WriteLine("Enter the Booking Id:");
                        int bookingid = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Booking Details:");
                        Booking booking = bookingSystemRepositoryImpl.GetBookingDetails(bookingid);
                        Console.WriteLine(booking);
                        break;

                    case 5:
                        Console.WriteLine("Enter the event name");
                        string EventName = Console.ReadLine();
                        bookingSystemRepositoryImpl.GetEventDetailsByEventName(EventName);
                        //bookingSystemRepositoryImpl.GetEventDetails();
                        break;

                    case 6:
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            } while (userInput != 6);
        }
    }
}
