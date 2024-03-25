using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBS.Bean;

namespace TBS.Exceptions
{
    public class InvalidBookingIdException:Exception
    {
        public InvalidBookingIdException(string message) : base(message) 
        {
            
        }
    }
}
