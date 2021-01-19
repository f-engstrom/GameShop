using System.Collections.Generic;

namespace GameShop.Models
{
    internal class Product
    {

        public int Id { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductDescription { get; internal set; }
        public decimal ProductPrice { get; internal set; }

        public Product(string productCode, string productName, string productDescription, decimal productPrice)
        {
            this.ProductCode = productCode;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.ProductPrice = productPrice;
        }

        public List<ProductOrder> Orders { get; protected set; } = new List<ProductOrder>();
    }
}