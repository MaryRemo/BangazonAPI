using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonSprint.Models
{
    public class Product
    {
        public int id { get; set; }
        public int price { get; set; }

        //[Required]
        //[StringLength(25, MinimumLength = 2)]
        public string title { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }

        //NOTE: The product_type_id and customer_id props are not yet defined; MR is working on the customer controller and the ProductType  controller has not been established yet.
        public string product_type_id {get; set; }

        public string customer_id { get; set; }
    }
}
