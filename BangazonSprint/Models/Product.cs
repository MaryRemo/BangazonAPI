using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonSprintStartUp.Models;

namespace BangazonSprint.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Price { get; set; }

        //[Required]
        //[StringLength(25, MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }


        //NOTE: The ProductType and customer_id props are not defined on this branch; MR is working on the customer controller and the ProductType  controller has not been established yet.

        public int ProductTypeId { get; set; }
        public int CustomerId { get; set; }

        public ProductType ProductType { get; set; }
        public Customer Customer { get; set; }
    }
}
