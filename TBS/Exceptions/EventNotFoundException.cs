using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Bean;
using TBS.Models;
using TBS.Utility;

namespace TBS.Exceptions
{
    internal class EventNotFoundException:Exception
    {
        public EventNotFoundException(string message) : base(message)
        {
            
        }

        public static int EventNotFound(string eventName)
        {
            BookingSystemRepositoryImpl book = new BookingSystemRepositoryImpl();
            int eventId = book.GetEventIdByName(eventName);

            if (eventId == -1)
            {
                throw new EventNotFoundException($"Event Not Found!");
            }
            return eventId;
        }
    }
}
