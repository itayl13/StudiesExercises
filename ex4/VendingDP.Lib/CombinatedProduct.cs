using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public enum CombinatedProductStatus { InMaking, Made }
    public class CombinatedProduct
    {
        public Item basicProduct;
        public List<Item> toppings;
        public CombinatedProductStatus CombinatedProductStatus { get; set; }
        public float Price { get; set; }
        public CombinatedProduct(Item basicProduct, CombinatedProductStatus combinatedProductStatus)
        {
            this.basicProduct = basicProduct;
            CombinatedProductStatus = combinatedProductStatus;
            toppings = new List<Item>();
            Price = basicProduct.Price;
        }
        public void AddTopping(Item topping, int quantity = 1)
        {
            for (int toppingCount = 0; toppingCount < quantity; toppingCount++)
            {
                toppings.Add(topping);
            }
            Price += quantity * topping.Price;
        }
        public List<string> GroupToppings()
        {
            IEnumerable<IGrouping<string, Item>> toppingsByName = toppings.GroupBy(topping => topping.Name);
            List<string> toppingsWithCounts = new List<string>();
            foreach (IGrouping<string, Item> toppingGroup in toppingsByName)
            {
                toppingsWithCounts.Add($"{toppingGroup.Count()} x {toppingGroup.Key}");
            }
            return toppingsWithCounts;
        }
        public override string ToString()
        {
            string details = basicProduct.Name + "\n" + string.Join("\n", GroupToppings());
            details += $"\n\t{Price} $";
            return details;
        }
    }
}
