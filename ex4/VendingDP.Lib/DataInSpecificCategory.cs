using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public class DataInSpecificCategory
    {
        public string categoryName;
        public List<Item> basicProducts;
        public List<Item> toppings;

        public DataInSpecificCategory(IGrouping<string, Item> basicProductsInCategory, IGrouping<string, Item> toppingsInCategory)
        {
            basicProducts = new List<Item>();
            toppings = new List<Item>();
            categoryName = basicProductsInCategory.Key;

            foreach (Item basicProduct in basicProductsInCategory)
            {
                basicProducts.Add(basicProduct);
            }
                
            foreach (Item topping in toppingsInCategory)
            {
                toppings.Add(topping);
            }

        }
    }
}
