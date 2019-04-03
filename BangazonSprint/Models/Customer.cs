using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonSprint.Models
{
    public class Customer
    {
<<<<<<< HEAD
        //-------------------------------------------------------------------------------
        //HN: Added props to test functionality of ProductController
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //-------------------------------------------------------------------------------

=======
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PaymentType> payments { get; set; }
        public List<Product> products { get; set; }
>>>>>>> master
    }
}
