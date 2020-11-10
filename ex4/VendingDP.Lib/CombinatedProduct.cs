using System.Collections.Generic;

namespace VendingDP.Lib
{
    public enum CombinatedProductStatus { InMaking, Made }
    public class CombinatedProduct
    {
        public BasicProduct basicProduct;
        public Dictionary<string, int> toppingsCounts;
        public CombinatedProductStatus CombinatedProductStatus { get; set; }
        public float Price { get; set; }
        public CombinatedProduct(BasicProduct basicProduct, CombinatedProductStatus combinatedProductStatus)
        {
            this.basicProduct = basicProduct;
            CombinatedProductStatus = combinatedProductStatus;
            toppingsCounts = new Dictionary<string, int>();
            foreach (string toppingName in basicProduct.toppingNames)
            {
                toppingsCounts.Add(toppingName, 0);
            }
            Price = basicProduct.Price;
        }
        public void AddTopping(Topping topping, int quantity = 1)
        {
            toppingsCounts[topping.name] += quantity;
            Price += quantity * topping.Price;
        }
        public override string ToString()
        {
            string details = basicProduct.name;
            foreach (KeyValuePair<string, int> toppingAndCount in toppingsCounts)
            {
                if (toppingAndCount.Value > 0)
                {
                    details += $"\n{toppingAndCount.Value} x {toppingAndCount.Key}";
                }
            }
            details += $"\n\t{Price} $";
            return details;
        }
    }
}
