using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class Venue
    {
        public string VenueName { get; set; }
        public string Address { get; set; }

        public Venue(string venueName, string venueAddress)
        {
            VenueName = venueName;
            Address = venueAddress;
        }
    }
}
