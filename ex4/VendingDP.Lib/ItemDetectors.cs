using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public class ItemDetectors : MenuLoader
    {
        public CombinatedProduct DetectProduct(string productName)
        {
            productName = productName.Trim();
            RawPreparedCombination rawPreparedCombination = GetRawPreparedCombination(productName);

            if (rawPreparedCombination != null)
            {
                return MakeCombinatedProduct(rawPreparedCombination);
            }
            Item basicProduct = GetItem(productName, BasicProducts);

            if (basicProduct != null)
            {
                CombinatedProduct combinatedProduct = new CombinatedProduct(basicProduct, CombinatedProductStatus.InMaking);
                return combinatedProduct;
            }
            return null;
        }

        private CombinatedProduct MakeCombinatedProduct(RawPreparedCombination rawPreparedCombination)
        {
            Item basicProduct = GetItem(rawPreparedCombination.BasicProductName, BasicProducts);
            CombinatedProduct combinatedProduct = new CombinatedProduct(basicProduct, CombinatedProductStatus.Made);
            AddToppingsToCombinatedProduct(combinatedProduct, rawPreparedCombination.Toppings);
            return combinatedProduct;
        }

        private void AddToppingsToCombinatedProduct(
            CombinatedProduct combinatedProduct, IEnumerable<string> preparedCombinationToppings)
        {
            foreach (string toppingName in preparedCombinationToppings)
            {
                Item topping = DetectTopping(toppingName);
                combinatedProduct.AddTopping(topping);
            }
        }

        public Item DetectTopping(string toppingName) => GetItem(toppingName, Toppings);

        private Item GetItem(string name, IEnumerable<Item> items) => items.FirstOrDefault(
            item => item.Name.Equals(name.Trim(), StringComparison.InvariantCultureIgnoreCase));
        private RawPreparedCombination GetRawPreparedCombination(string name) => PreparedCombinations.FirstOrDefault(
            item => item.Name.Equals(name.Trim(), StringComparison.InvariantCultureIgnoreCase));
    }
}
