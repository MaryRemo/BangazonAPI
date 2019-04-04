/*
 * CREATED BY: HM
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonSprint.Models
{
    public class Order
    {
        public int id { get; set; }

        public int CustomerId { get; set; }

        public int PaymentTypeId { get; set; }

        public List <Customer> Customers {get; set;} = new List<Customer>();

        public List<Product> Products { get; set; } = new List<Product>();

    }
}
