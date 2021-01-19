using System;
using System.Collections.Generic;

namespace GameShop.Models
{
    internal class Order
    {
        public int Id { get; internal set; }
        public Customer Customer { get; internal set; }
        public int CustomerId { get; internal set; }
        public DateTime OrderDate { get; internal set; }

        public DateTime? OrderFinishedDate { get; internal set; }

        public Order()
        {
            OrderDate = DateTime.Now;

        }

        public void OrderDone()
        {

            OrderFinishedDate= DateTime.Now;
            

        }


        public List<ProductOrder> Products { get; protected set; } = new List<ProductOrder>();
    }
}