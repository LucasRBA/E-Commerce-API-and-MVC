using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Payment_API.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string DocumentFormatter(string number) {
            string formattedNumber = String.Format("{0:###.###.###-##}", number);
            return formattedNumber;
        }

        public string PhoneNumberFormatter(string number) {
            string formattedNumber = String.Format("{0:#-###-###-####}", number);
            return formattedNumber;
        }
    }
}
