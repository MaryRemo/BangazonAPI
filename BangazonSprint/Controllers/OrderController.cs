﻿/*
* CREATED BY: HM
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonSprint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonSprint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/Order?q=socks&include=paymentType
        [HttpGet]
        public IEnumerable<Order> Get(string include, string q, string completed)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = @"
                                    select 
	                                [Order].Id as oId, [Order].CustomerId, [Order].PaymentTypeId,
	                                PaymentType.Id, PaymentType.[Name], PaymentType.AcctNumber, PaymentType.CustomerId,
	                                Customer.Id as CustomerID, Customer.FirstName as CustomerFirstName, Customer.LastName as CustomerLastName,

		                            Product.Id as ProductId, Product.Title as ProductTitle, Product.ProductTypeId, Product.CustomerId, Product.[Description] as ProductD, Product.Quantity as ProductQ, Product.Price as Price,


	                                OrderProduct.Id, OrderProduct.OrderId, OrderProduct.ProductId

	                                from [order]
	                                left join paymentType
	                                on PaymentType.id =[order].paymentTypeId
	                                left join customer
	                                on paymentType.CustomerId = customer.id
                                    left join [OrderProduct]
	                                on [OrderProduct].OrderId = [Order].id
		                            left join [Product]
		                            on OrderProduct.ProductId = Product.Id";
                    }

                    else if (include == "customers")
                    {
                        cmd.CommandText = @"
                                    select 
	                                [Order].Id as oId, [Order].CustomerId as oCID, [Order].PaymentTypeId,
	                                PaymentType.Id, PaymentType.[Name], PaymentType.AcctNumber, PaymentType.CustomerId,
	                                Customer.Id as CustomerID, Customer.FirstName as CustomerFirstName, Customer.LastName as CustomerLastName,
	                                OrderProduct.Id, OrderProduct.OrderId, OrderProduct.ProductId
	                                from [order]
	                                left join paymentType
	                                on PaymentType.id =[order].paymentTypeId
	                                left join customer
	                                on paymentType.CustomerId = customer.id
                                    left join [OrderProduct]
	                                on [OrderProduct].OrderId = [Order].id
                                    ";
                    }

                    else if (completed == "false")
                    {
                        cmd.CommandText = $@"select o.Id as oId, op.Id as opId, p.Id as pId, c.Id as cId, o.CustomerId, o.PaymentTypeId, 
                                            op.OrderId, op.productId, pt.[Name], pt.AcctNumber, p.Title, p.Price,
                                            p.[Description], c.FirstName, c.LastName
                                            from [order] o
                                            left join paymentType pt on o.PaymentTypeId = pt.Id
                                            left join OrderProduct op on o.Id = op.OrderId
                                            left join product p on op.ProductId = p.Id
                                            left join customer c on o.CustomerId = c.Id
                                            where o.paymentTypeId is null";
                    }

                    else
                    {
                        cmd.CommandText = @"
                                         select id as oId, customerId, paymenttypeid
                                            From [Order]
                                         WHERE 1 = 1";
                    }

                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd.CommandText += @" AND 
                                             ([order].CustomerId LIKE @q OR
                                              [order].PaymentTypeId LIKE @q)";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();


                    Dictionary<int, Order> orders = new Dictionary<int, Order>();
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(reader.GetOrdinal("oId"));




                        if (completed == "false")
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                            {
                                if (!orders.ContainsKey(orderId))
                                {
                                    Order order = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                                    };
                                    orders.Add(orderId, order);
                                }
                            }
                        }

                        if (include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("productId")))
                            {
                                if (!orders.ContainsKey(orderId))
                                {
                                    Order order = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                                    };
                                    orders.Add(orderId, order);
                                }

                                Order currentOrder = orders[orderId];
                                currentOrder.Products.Add(
                                    new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("ProductTitle")),
                                        Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                        Description = reader.GetString(reader.GetOrdinal("ProductD")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("ProductQ"))
                                    }
                                );
                            }
                        }

                        else if (include == "customers")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                            {

                                if (!orders.ContainsKey(orderId))
                                {
                                    Order order = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                                    };
                                    orders.Add(orderId, order);
                                }

                                Order currentOrder = orders[orderId];
                                currentOrder.Customers.Add(
                                    new Customer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("CustomerLastName")),
                                    }
                                );
                            }

                            if (reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                            {

                                if (!orders.ContainsKey(orderId))
                                {
                                    Order order = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("oCID"))

                                    };
                                    orders.Add(orderId, order);
                                }
                            }
                        }

                        else
                        {
                            if  (reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                            {
                                if (!orders.ContainsKey(orderId))
                                {
                                    Order newOrder = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                                    };

                                    orders.Add(orderId, newOrder);
                                }

                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                              {
                                if (!orders.ContainsKey(orderId))
                                {
                                    Order newOrder = new Order
                                    {
                                        id = orderId,
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                    };

                                    orders.Add(orderId, newOrder);
                                }

                              }


                        }
                    }
                    reader.Close();

                    return orders.Values.ToList();
                }
            }
        }






        // GET: api/Order/5
        [HttpGet("{id}", Name = "GetSingleOrder")]
        public Order Get(int id, string include, string completed)

        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = @"
                                    select p.id, p.[name], 
                                    product.id as ProductId, product.ProductTypeId, product.CustomerId, Product.price as ProductPrice, Product.Title as ProductTitle, Product.[Description] as ProductD, Product.Quantity as ProductQ,
                                    OrderProduct.Id, OrderProduct.OrderId, OrderProduct.ProductId,
                                    [Order].Id as orderId, [Order].CustomerId, [Order].PaymentTypeId,
                                    PaymentType.Id, PaymentType.[Name], PaymentType.AcctNumber, PaymentType.CustomerId,
                                    Customer.Id as CustomerID, Customer.FirstName as CustomerFirstName, Customer.LastName as CustomerLastName
                                    from productType as p
                                    left join product 
                                    on p.id = product.productTypeId
                                    left join orderProduct 
                                    on product.id = orderProduct.productid
                                    left join [order]
                                    on orderProduct.orderId = [order].id 
                                    left join PaymentType
                                    on [order].paymentTypeId = paymentType.id
                                    left join customer
                                    on paymentType.CustomerId = customer.id
                                    ";
                    }
                    else if (include == "customers")
                    {
                        cmd.CommandText = @"
                                    select p.id, p.[name], 
                                    product.id as ProductId, product.ProductTypeId, product.CustomerId, Product.price as ProductPrice, Product.Title as ProductTitle, Product.[Description] as ProductD, Product.Quantity as ProductQ,
                                    OrderProduct.Id, OrderProduct.OrderId, OrderProduct.ProductId,
                                    [Order].Id as orderId, [Order].CustomerId, [Order].PaymentTypeId,
                                    PaymentType.Id, PaymentType.[Name], PaymentType.AcctNumber, PaymentType.CustomerId,
                                    Customer.Id as CustomerID, Customer.FirstName as CustomerFirstName, Customer.LastName as CustomerLastName
                                    from productType as p
                                    left join product 
                                    on p.id = product.productTypeId
                                    left join orderProduct 
                                    on product.id = orderProduct.productid
                                    left join [order]
                                    on orderProduct.orderId = [order].id 
                                    left join PaymentType
                                    on [order].paymentTypeId = paymentType.id
                                    left join customer
                                    on paymentType.CustomerId = customer.id
                                   ";
                    }

                    else if (completed == "false")
                    {
                        cmd.CommandText = @"
                                    select p.id, p.[name], 
                                    product.id as ProductId, product.ProductTypeId, product.CustomerId, Product.price as ProductPrice, Product.Title as ProductTitle, Product.[Description] as ProductD, Product.Quantity as ProductQ,
                                    OrderProduct.Id, OrderProduct.OrderId, OrderProduct.ProductId,
                                    [Order].Id as orderId, [Order].CustomerId, [Order].PaymentTypeId,
                                    PaymentType.Id, PaymentType.[Name], PaymentType.AcctNumber, PaymentType.CustomerId,
                                    Customer.Id as CustomerID, Customer.FirstName as CustomerFirstName, Customer.LastName as CustomerLastName
                                    from productType as p
                                    left join product 
                                    on p.id = product.productTypeId
                                    left join orderProduct 
                                    on product.id = orderProduct.productid
                                    left join [order]
                                    on orderProduct.orderId = [order].id 
                                    left join PaymentType
                                    on [order].paymentTypeId = paymentType.id
                                    left join customer
                                    on paymentType.CustomerId = customer.id
                                    
                                    WHERE [Order].PaymentTypeId = null
                                   ";
                    }

                    else
                    {
                        cmd.CommandText = @"
                                         select * from [order]
                                         ";
                    }

                    cmd.CommandText += " WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Order order = null;
                    while (reader.Read())
                    {
                        if (order == null)
                        {
                            order = new Order
                            {
                                id = id,
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                            };


                        }

                        if (include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("productId")))
                            {
                                if (!order.Products.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("ProductId"))))
                                {
                                    order.Products.Add(
                                        new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                            Title = reader.GetString(reader.GetOrdinal("ProductTitle")),
                                            Price = reader.GetInt32(reader.GetOrdinal("ProductPrice")),
                                            Description = reader.GetString(reader.GetOrdinal("ProductD")),
                                            Quantity = reader.GetInt32(reader.GetOrdinal("ProductQ"))
                                        }
                                    );
                                }
                            }
                        }

                        if (include == "customers")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                            {
                                if (!order.Products.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("ProductId"))))
                                {

                                    order.Customers.Add(
                                    new Customer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("CustomerLastName")),
                                    }
                                 );
                                }
                            }
                        }
                    }

                    reader.Close();

                    return order;
                }
            }
        }



        
            [HttpPost]
            public IActionResult Post([FromBody] Order order)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO [order] (CustomerId, PaymentTypeId)
                                            OUTPUT INSERTED.Id
                                            VALUES (@CustomerId, @PaymentTypeId)";
                        cmd.Parameters.Add(new SqlParameter("@CustomerId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@PaymentTypeId", order.PaymentTypeId));
                       
                        int newId = (int)cmd.ExecuteScalar();
                        order.id = newId;
                        return CreatedAtRoute("GetSingleOrder", new { id = newId }, order);
                    }
                }
            }
            

            [HttpPut("{id}")]
            public IActionResult Put([FromRoute] int id, [FromBody] Order order)
            {                             
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"UPDATE [Order]
                                                SET 
                                                PaymentTypeId = @PaymentTypeId                                             
                                                WHERE [order].id = @id";
                            cmd.Parameters.Add(new SqlParameter("@PaymentTypeId", order.PaymentTypeId));
                            cmd.Parameters.Add(new SqlParameter("@id", id));

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return new StatusCodeResult(StatusCodes.Status204NoContent);
                            }
                            throw new Exception("No rows affected");
                        }
                    }                         
            }

        

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM OrderProduct WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();
                }


                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [order] WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return NoContent();
                    }
                    throw new Exception("No rows affected");
                }
                
            }
        }



    }
}