using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public static class ItemDetectors
    {
        public static CombinatedProduct DetectProduct(string productName) 
        {
            // If everything goes as planned, a prepared combination product is requested:
            try
            {
                List<object> preparedCombinationInstructions = MenuLoader.preparedCombinations[productName];
                string basicProductName = (string)preparedCombinationInstructions[0];
                CombinatedProduct combinatedProduct = new CombinatedProduct(DetectBasicProduct(basicProductName), CombinatedProductStatus.Made);
                MakeCombinatedProduct(combinatedProduct, preparedCombinationInstructions);
                return combinatedProduct;
            }
            // Product name not in the prepared combination products:
            catch (KeyNotFoundException)
            {
                BasicProduct basicProduct = DetectBasicProduct(productName);
                CombinatedProduct combinatedProduct = new CombinatedProduct(basicProduct, CombinatedProductStatus.InMaking);
                return combinatedProduct;
            }
            // Basic product not detected (Other exceptions are covered in menu validation):
            catch (ArgumentException)
            {
                throw new KeyNotFoundException($"No basic product named {productName} was found.");
            }
        }

        public static Topping DetectTopping(string toppingName) 
        {
            try
            {
                List<object> matchingToppingLine = MenuLoader.toppings
                    .Find(toppingDetailLine => (string)toppingDetailLine[(int)ItemDetailColumns.Name] == toppingName);
                return new Topping(
                    name: (string)matchingToppingLine[(int)ItemDetailColumns.Name],
                    price: (float)matchingToppingLine[(int)ItemDetailColumns.Price],
                    toppingFor: (string)matchingToppingLine[(int)ItemDetailColumns.ProductCategory]);
            }
            catch (NullReferenceException)
            {
                throw new KeyNotFoundException($"No topping named {toppingName} was found (Maybe wrong typing format?).");
            }
        }

        private static BasicProduct DetectBasicProduct(string productName)
        {
            try
            {
                List<object> matchingProductLine = MenuLoader.basicProducts
                    .Find(productDetailLine => (string)productDetailLine[(int)ItemDetailColumns.Name] == productName);
                List<string> matchingToppingsToProduct = FindMatchingToppings((string)matchingProductLine[(int)ItemDetailColumns.ProductCategory]);
                return new BasicProduct(
                    name: (string)matchingProductLine[(int)ItemDetailColumns.Name],
                    price: (float)matchingProductLine[(int)ItemDetailColumns.Price],
                    possibleToppingNames: matchingToppingsToProduct,
                    productCategory: (string)matchingProductLine[(int)ItemDetailColumns.ProductCategory]);
            }
            // basic product of the input name not found in the menu:
            catch (NullReferenceException)
            {
                throw new KeyNotFoundException($"No basic product named {productName} was found.");
            }
        }

        private static void MakeCombinatedProduct(
            CombinatedProduct combinatedProduct, List<object> preparedCombinationInstructions)
        {
            for (int instruction = 0; instruction < preparedCombinationInstructions.Count() / 2; instruction++)
            {
                string toppingName = (string)preparedCombinationInstructions[2 * instruction + 1];
                int quantity = (int)preparedCombinationInstructions[2 * instruction + 2];
                Topping topping = DetectTopping(toppingName);
                combinatedProduct.AddTopping(topping, quantity);
            }
        }

        private static List<string> FindMatchingToppings(string productCategory)
        {
            return MenuLoader.toppings
                .Where(toppingDetailLine => (string)toppingDetailLine[(int)ItemDetailColumns.ProductCategory] == productCategory)
                .Select(toppingDetailLine => (string)toppingDetailLine[(int)ItemDetailColumns.Name]).ToList();
        }
    }
}
