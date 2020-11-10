using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public class Menu : MenuLoader
    {
        public Menu()
        {
            Validate();
        }

        // Check whether the menu products are written correctly. If not, throw relevant exceptions.
        public static void Validate()
        {
            IEnumerable<List<object>> wrongCategoryToppings = WrongCategoryToppings();
            ErrorHandler.InvalidMenuItems(wrongCategoryToppings);

            IEnumerable<KeyValuePair<string, List<object>>> wrongTypeCombinations = WronglyWrittenPreparedCombinations();
            ErrorHandler.InvalidMenuItems(wrongTypeCombinations.Cast<object>());
        }

        // No topping should belong to a product category no basic product belongs to.
        private static IEnumerable<List<object>> WrongCategoryToppings()
        {
            IEnumerable<string> basicProductCategoryNames = basicProducts
                .Select(productDetails => (string)productDetails[(int)ItemDetailColumns.ProductCategory])
                .Distinct();
            IEnumerable<List<object>> wrongCategoryToppings = toppings
                .Where(
                productDetails => !basicProductCategoryNames.Contains((string)productDetails[(int)ItemDetailColumns.ProductCategory]));
            return wrongCategoryToppings;
        }
        /* The prepared combinations should match the following format:
             * (Known) List<object> {basic product name, topping1 name, topping1 quantity, topping2 name, topping2 quantity, ...}
             * (length is odd, first item belongs to the basic product names, then each pair of items is with first item 
             * belonging to the topping names and second item can be casted to int).
             */
        private static IEnumerable<KeyValuePair<string, List<object>>> WronglyWrittenPreparedCombinations()
        {
            IEnumerable<string> basicProductNames = basicProducts
               .Select(productDetails => (string)productDetails[(int)ItemDetailColumns.Name]);
            IEnumerable<string> toppingNames = toppings
                .Select(productDetails => (string)productDetails[(int)ItemDetailColumns.Name]);
            IEnumerable<KeyValuePair<string, List<object>>> wrongTypeCombinations = preparedCombinations
                .Where(preparedCombination =>
                !IsCombinatedProductToppingsOK(preparedCombination.Value, toppingNames, basicProductNames));
            return wrongTypeCombinations;
        }
        private static bool IsCombinatedProductToppingsOK(
            List<object> combinatedProduct, IEnumerable<string> toppingNames, IEnumerable<string> basicProductNames)
        {
            if (!(combinatedProduct.Count() % 2 == 1 &&
                basicProductNames.Contains((string)combinatedProduct[0])))
            {
                return false;
            }
            for (int instruction = 0; instruction < combinatedProduct.Count() / 2; instruction++)
            {
                if (!toppingNames.Contains((string)combinatedProduct[2 * instruction + 1]))
                {
                    return false;
                }
                if (!(combinatedProduct[2 * instruction + 2] is int))
                {
                    return false;
                }
            }
            return true;
        }

        /* ItemsToCategories: key=category, value=dictionary with keys "Basic Products" and "Toppings"
         * and values which are the lists of items belonging to the category.
         */
        private Dictionary<string, Dictionary<string, List<object>>> ItemsToCategories()
        {
            Dictionary<string, Dictionary<string, List<object>>> itemsToCategories = new Dictionary<string, Dictionary<string, List<object>>>();
            IEnumerable<string> categories = basicProducts
                .Select(basicProduct => (string)basicProduct[(int)ItemDetailColumns.ProductCategory]).Distinct();
            foreach (string category in categories)
            {
                itemsToCategories.Add(category, new Dictionary<string, List<object>>()
                {
                    ["Basic Products"] = new List<object>(),
                    ["Toppings"] = new List<object>()
                }) ; 
            }
            foreach (List<object> basicProduct in basicProducts)
            {
                itemsToCategories[(string)basicProduct[(int)ItemDetailColumns.ProductCategory]]
                    ["Basic Products"].Add(basicProduct);
            }
            foreach (List<object> topping in toppings)
            {
                itemsToCategories[(string)topping[(int)ItemDetailColumns.ProductCategory]]
                    ["Toppings"].Add(topping);
            }
            return itemsToCategories;
        }

        private int GetLongestBasicProductName() =>
            basicProducts.Concat(toppings)
                .Select(productDetails => ((string)productDetails[(int)ItemDetailColumns.Name]).Length).Max();

        private string SpecificListOfProductsDetails(List<object> productsGroup, int nameLength)
        {
            string productsDetails = "";
            foreach (List<object> productDetails in productsGroup)
            {
                productsDetails += "\n * * " 
                    + ((string)productDetails[(int)ItemDetailColumns.Name]).PadRight(nameLength) 
                + " ---------- " 
                + ((float)productDetails[(int)ItemDetailColumns.Price]).ToString() + " $";
            }
            return productsDetails;
        }
        private string PreparedCombinationPrinter(KeyValuePair<string, List<object>> preparedCombination)
        {
            string combinationDetails = $"\n{preparedCombination.Key} = {preparedCombination.Value[0]} + ";
            string[] toppings = new string[preparedCombination.Value.Count() / 2];
            for (int instruction = 0; instruction < preparedCombination.Value.Count() / 2; instruction++)
            {
                string toppingName = (string)preparedCombination.Value[2 * instruction + 1];
                int quantity = (int)preparedCombination.Value[2 * instruction + 2];
                toppings[instruction] += $"{quantity}x{toppingName}";
            }
            combinationDetails += string.Join(" + ", toppings);
            return combinationDetails;
        }
        public override string ToString()
        {
            string menuMessage = "\n----- MENU -----\n";
            Dictionary<string, Dictionary<string, List<object>>> itemsToCategories = ItemsToCategories();
            int longestBasicProductName = GetLongestBasicProductName();
            foreach (KeyValuePair<string, Dictionary<string, List<object>>> categoryAndItems in itemsToCategories)
            {
                menuMessage += categoryAndItems.Key + ":\n * Basic Products:";
                menuMessage += SpecificListOfProductsDetails(categoryAndItems.Value["Basic Products"], longestBasicProductName);
                menuMessage += "\n * Toppings:";
                menuMessage += SpecificListOfProductsDetails(categoryAndItems.Value["Toppings"], longestBasicProductName) + "\n";
            }
            menuMessage += "\nPrepared Combinations (Price - Do the Math):";
            foreach (KeyValuePair<string, List<object>> preparedCombination in preparedCombinations)
            {
                menuMessage += PreparedCombinationPrinter(preparedCombination);
            }
            return menuMessage;
        }
    }
}
