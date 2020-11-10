using System.Collections.Generic;

namespace VendingDP.Lib
{
    public class BasicProduct
    {
        public readonly string name;
        public float Price { get; }
        public readonly List<string> toppingNames;
        public readonly string productCategory;
        public BasicProduct(string name, float price, List<string> possibleToppingNames, string productCategory)
        {
            this.name = name;
            Price = price;
            toppingNames = possibleToppingNames;
            this.productCategory = productCategory;
        }
    }
}
