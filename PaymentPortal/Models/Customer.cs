using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPortal.Models
{
    public class Customer
    {
        public int SSN { get; set; }
        public string FName { get; set; } = "fname";
        public string LName { get; set; } = "lname";

        public int CreditCardAmountDue { get; set; } = 0;             
        public string Address { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }


    }
}
