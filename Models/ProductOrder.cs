using System;
using System.Collections.Generic;
using System.Text;

namespace GameShop.Models
{
    class ProductOrder
    {
        public int OrderId { get; protected set; }
        public Order Order { get; protected set; }
        public Product Product { get; protected set; }
        public int ProductId { get; protected set; }
        public int Amount { get; protected set; }

        public ProductOrder(Product product, int amount)
        {
            Product = product;
            Amount = amount;

        }

        public ProductOrder()
        {

        }
    }
}
