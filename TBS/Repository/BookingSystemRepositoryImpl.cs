using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Models;
using TBS.Services;
using TBS.Utility;
using System.Data.SqlClient;
using TBS.Exceptions;

namespace TBS.Bean
{
    public class BookingSystemRepositoryImpl:IEventServiceProvider,IBookingSystemServiceProvider
    {

        SqlConnection connect = null;
        SqlCommand cmd = null;

        public BookingSystemRepositoryImpl()
        {
            connect = new SqlConnection(DataConnectionUtility.GetConnectionString());
            cmd = new SqlCommand();
        }

        public void CreateEvent(int eventId, string eventName, DateTime eventDate, TimeSpan eventTime, int venueId, int totalSeats, int availableSeats, decimal ticketPrice, event_type eventType)
        {
            try
            {
                Event eventobj = new Event(eventId, eventName, eventDate, eventTime, venueId, totalSeats, availableSeats, ticketPrice, eventType);

                string eventTypeString = eventType.ToString();

                if (!Enum.IsDefined(typeof(event_type), eventType))
                {
                    Console.WriteLine("Invalid eventType");
                    return;
                }

                using (SqlConnection connect = new SqlConnection(DataConnectionUtility.GetConnectionString()))
                {
                    connect.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connect;
                        string query = @"INSERT INTO Event (event_id, event_name, event_date, event_time, venue_id, total_seats, available_seats, ticket_price, event_type)
                                VALUES (@EventId, @EventName, @EventDate, @EventTime, @VenueId, @TotalSeats, @AvailableSeats, @TicketPrice, @EventType);";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@EventId", eventobj.EventId);
                        cmd.Parameters.AddWithValue("@EventName", eventobj.EventName);
                        cmd.Parameters.AddWithValue("@EventDate", eventobj.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", eventobj.EventTime);
                        cmd.Parameters.AddWithValue("@VenueId", eventobj.VenueId);
                        cmd.Parameters.AddWithValue("@TotalSeats", eventobj.TotalSeats);
                        cmd.Parameters.AddWithValue("@AvailableSeats", eventobj.AvailableSeats);
                        cmd.Parameters.AddWithValue("@TicketPrice", eventobj.TicketPrice);
                        cmd.Parameters.AddWithValue("@EventType", eventTypeString);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " rows inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void BookTickets(string eventName, int numTickets)
        {

            int eventId = EventNotFoundException.EventNotFound(eventName);
            int availableSeats = GetAvailableNoOfTickets(eventName);
            if (numTickets > availableSeats)
            {
                Console.WriteLine($"Not enough available seats for event '{eventName}'.");
            }

            Console.WriteLine("Enter Customer Id:");
            int customerid = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name:");
            string? name = Console.ReadLine();
            Console.WriteLine("Enter the Email:");
            string? email = Console.ReadLine();
            Console.WriteLine("Enter Phone Number:");
            string? phoneNo = Console.ReadLine();
            int customerId = InsertCustomerDetails(customerid, name, email, phoneNo);

            List<Booking> bookings = new List<Booking>();

            Booking booking = new Booking();
            booking.BookingId = customerid;
            booking.EventId = eventId;
            booking.CustomerId = customerid;
            booking.NumTickets = numTickets;
            booking.TotalCost = Convert.ToDecimal(numTickets) * GetTicketPrice(eventName);
            booking.BookingDate = DateTime.Now;
            Console.WriteLine($"Total cost for {numTickets} is {booking.TotalCost}");

            bookings.Add(booking);

            int bookingId = AddBookingDetails(bookings);

            bool addTickets = false;

            UpdateAvailableTickets(eventId, numTickets, addTickets);

            Console.WriteLine($"{numTickets}Tickets booked successfully for '{eventName}' and your Booking ID is {bookingId}\nTh'Please Note Your Booking ID for Future reference'");
        }

        public int GetEventIdByName(string eventName)
        {

            int eventId = -1;

            string query = "SELECT event_id FROM Event WHERE event_name = @EventName";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventName", eventName);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        eventId = Convert.ToInt32(result);
                    }
                }
            }
            return eventId;
        }

        public int InsertCustomerDetails(int customerId , string customerName, string email, string phoneNumber)
        {
            string query = "INSERT INTO Customer (customer_id , customer_name, email, phone_number) VALUES (@CustomerId , @CustomerName, @Email, @PhoneNumber);";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                connection.Open();

                //Create a command
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    customerId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return customerId;
        }

        public decimal GetTicketPrice(string eventName)
        {
            decimal ticketPrice = 0;

            string query = "SELECT ticket_price FROM Event WHERE event_name = @EventName;";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventName", eventName);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        ticketPrice = Convert.ToDecimal(result);
                    }
                }
            }

            return ticketPrice;
        }

        public int AddBookingDetails(List<Booking> bookings)
        {
            int bookingId = -1;

            string query = "INSERT INTO Booking (booking_id, customer_id, event_id, num_tickets, total_cost, booking_date) " +
                           "VALUES (@BookingId, @CustomerId, @EventId, @NumTickets, @TotalCost, @BookingDate);";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {

                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var booking in bookings)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@BookingId", booking.BookingId);
                        command.Parameters.AddWithValue("@CustomerId", booking.CustomerId);
                        command.Parameters.AddWithValue("@EventId", booking.EventId);
                        command.Parameters.AddWithValue("@NumTickets", booking.NumTickets);
                        command.Parameters.AddWithValue("@TotalCost", booking.TotalCost);
                        command.Parameters.AddWithValue("@BookingDate", booking.BookingDate);

                        bookingId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            return bookingId;
        }

        public void UpdateAvailableTickets(int eventId, int numTickets, bool addTickets)
        {
            string operatorSymbol = addTickets ? "+" : "-";

            string query = $"UPDATE Event SET available_seats = available_seats {operatorSymbol} @NumTickets WHERE event_id = @EventId";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumTickets", numTickets);
                    command.Parameters.AddWithValue("@EventId", eventId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void CancelBooking(int bookingId)
        {
            Booking booking = GetBookingDetails(bookingId);

            int eventId = booking.EventId;
            int numTickets = booking.NumTickets;
            bool subTickets = true;

            UpdateAvailableTickets(eventId, numTickets, subTickets);

            DeleteBookingDetails(bookingId);
        }


        private void DeleteBookingDetails(int bookingId)
        {
            string query = "DELETE FROM Booking WHERE booking_id = @BookingId";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingId", bookingId);

                    int rowsaffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Number of rows ({rowsaffected}) Affected");
                    Console.WriteLine($"Booking ID {bookingId} has been cancelled.");
                }
            }
        }

        public int GetAvailableNoOfTickets(string eventName)
        {
            int eventId = GetEventIdByName(eventName);
            int availableSeats = 0;

            string query = "SELECT available_seats FROM Event WHERE event_id = @EventId;";

            using (SqlConnection connection = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@EventId", eventId);

                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        availableSeats = Convert.ToInt32(result);
                    }
                }
            }

            return availableSeats;

        }

        public void GetEventDetails()
        {
            List<Event> events = new List<Event>();
            cmd.CommandText = "Select * From Event ";
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Event eventobj = new Event();
                eventobj.EventId = (int)reader["event_id"];
                eventobj.EventName = (string)reader["event_name"];
                eventobj.EventDate = (DateTime)reader["event_date"];
                eventobj.EventTime = (TimeSpan)reader["event_time"];
                eventobj.VenueId = (int)reader["venue_id"];
                eventobj.TotalSeats = (int)reader["total_seats"];
                eventobj.AvailableSeats = (int)reader["available_seats"];
                eventobj.TicketPrice = (decimal)reader["ticket_price"];
                if (Enum.TryParse(reader["event_type"].ToString(), out event_type eventType))
                {
                    eventobj.EventType = eventType;
                }
                eventobj.BookingId = Convert.IsDBNull(reader["booking_id"]) ? null : (int)reader["booking_id"];
                events.Add(eventobj);
            }
            foreach (Event eventobject in events)
            {
                Console.WriteLine(eventobject);
            }
            connect.Close();
        }

        public void GetEventDetailsByEventName(string eventName)
        {
            List<Event> events = new List<Event>();
            cmd.CommandText = "Select * From Event where event_name = @eventname1";
            cmd.Parameters.AddWithValue("@eventname1", eventName);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Event eventobj = new Event();
                eventobj.EventId = (int)reader["event_id"];
                eventobj.EventName = (string)reader["event_name"];
                eventobj.EventDate = (DateTime)reader["event_date"];
                eventobj.EventTime = (TimeSpan)reader["event_time"];
                eventobj.VenueId = (int)reader["venue_id"];
                eventobj.TotalSeats = (int)reader["total_seats"];
                eventobj.AvailableSeats = (int)reader["available_seats"];
                eventobj.TicketPrice = (decimal)reader["ticket_price"];
                if (Enum.TryParse(reader["event_type"].ToString(), out event_type eventType))
                {
                    eventobj.EventType = eventType;
                }
                eventobj.BookingId = Convert.IsDBNull(reader["booking_id"]) ? null : (int)reader["booking_id"];
                events.Add(eventobj);
            }
            foreach (Event eventobject in events)
            {
                Console.WriteLine(eventobject);
            }
            connect.Close();
        }

        public Booking GetBookingDetails(int bookingId)
        {
            Booking booking = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(DataConnectionUtility.GetConnectionString()))
                {
                    connect.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Booking WHERE booking_id = @BookingId", connect))
                    {
                        cmd.Parameters.AddWithValue("@BookingId", bookingId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                booking = new Booking
                                {
                                    BookingId = (int)reader["booking_id"],
                                    CustomerId = (int)reader["customer_id"],
                                    EventId = (int)reader["event_id"],
                                    NumTickets = (int)reader["num_tickets"],
                                    TotalCost = (decimal)reader["total_cost"],
                                    BookingDate = (DateTime)reader["booking_date"]
                                };
                            }
                            else
                            {
                                throw new InvalidBookingIdException("Booking ID not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return booking;
        }

    }
}
