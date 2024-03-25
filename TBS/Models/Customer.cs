using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBS.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer(int customerid, string name, string emailAddress, string phoneNumber)
        {
            CustomerId = customerid;
            CustomerName = name;
            Email = emailAddress;
            PhoneNumber = phoneNumber;
        }
    }
}
