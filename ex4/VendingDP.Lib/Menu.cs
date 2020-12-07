using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib
{
    public class Menu : MenuLoader
    {
        private readonly ErrorHandler errorHandler = new ErrorHandler();

        public Menu()
        {
            Validate();
        }

        public void Validate()
        {
            CheckDataCollections();
            IEnumerable<string> wrongCategoryToppings = WrongCategoryToppings();
            errorHandler.InvalidMenuItems(wrongCategoryToppings, "toppings");

            IEnumerable<string> wrongTypeCombinations = WronglyWrittenPreparedCombinations();
            errorHandler.InvalidMenuItems(wrongTypeCombinations, "prepared combinations");
        }

        private void CheckDataCollections()
        {
            errorHandler.IsDataCollectionEmpty(BasicProducts, "basic products");
            errorHandler.IsDataCollectionEmpty(Toppings, "toppings");
            errorHandler.IsDataCollectionEmpty(PreparedCombinations, "prepared combinations");
        }

        private IEnumerable<string> WrongCategoryToppings()
        {
            IEnumerable<string> validCategories = BasicProducts.Select(basicProduct => basicProduct.Category).Distinct(); 
            IEnumerable<string> wrongCategoryToppings = Toppings
                .Where(topping => !validCategories.Contains(topping.Category)).Select(topping => topping.Name);
            return wrongCategoryToppings;
        }

        private IEnumerable<string> WronglyWrittenPreparedCombinations() => PreparedCombinations
            .Where(rawPreparedCombination => !ValidateCombinatedProduct(rawPreparedCombination))
            .Select(combination => combination.Name);

        private bool ValidateCombinatedProduct(RawPreparedCombination combinatedProduct)
        {
            bool isFirstBasicProduct = BasicProducts.Select(basicProduct => basicProduct.Name).Contains(combinatedProduct.BasicProductName);
            IEnumerable<string> toppingNames = Toppings.Select(topping => topping.Name);
            bool areRestToppings = combinatedProduct.Toppings.All(toppingName => toppingNames.Contains(toppingName));
            return isFirstBasicProduct && areRestToppings;
        }

        public override string ToString()
        {
            string menuMessage = "\n----- MENU -----\n";
            IEnumerable<DataInSpecificCategory> itemsToCategories = ItemsToCategories();
            int longestBasicProductName = GetLongestBasicProductName();

            foreach (DataInSpecificCategory categoryAndData in itemsToCategories)
            {
                menuMessage += categoryAndData.categoryName + ":\n * Basic Products:";
                menuMessage += SpecificListOfProductsDetails(categoryAndData.basicProducts, longestBasicProductName);
                menuMessage += "\n * Toppings:";
                menuMessage += SpecificListOfProductsDetails(categoryAndData.toppings, longestBasicProductName) + "\n";
            }
            menuMessage += "\nPrepared Combinations (Price - Do the Math):";

            foreach (RawPreparedCombination rawPreparedCombination in PreparedCombinations)
            {
                menuMessage += PreparedCombinationPrinter(rawPreparedCombination);
            }
            return menuMessage;
        }

        private IEnumerable<DataInSpecificCategory> ItemsToCategories()
        {
            IEnumerable<DataInSpecificCategory> itemsToCategories =
                from basicProductsInCategory in BasicProducts.GroupBy(basicProduct => basicProduct.Category)
                join toppingsInCategory in Toppings.GroupBy(topping => topping.Category)
                on basicProductsInCategory.Key equals toppingsInCategory.Key
                select new DataInSpecificCategory(basicProductsInCategory, toppingsInCategory);
            return itemsToCategories;
        }

        private int GetLongestBasicProductName() =>
            BasicProducts.Concat(Toppings).Select(item => item.Name.Length).Max();

        private string SpecificListOfProductsDetails(IEnumerable<Item> productsGroup, int nameLength)
        {
            string productsDetails = "";

            foreach (Item productDetails in productsGroup)
            {
                productsDetails += "\n * * " + productDetails.Name.PadRight(nameLength)
                + " ---------- " + productDetails.Price.ToString() + " $";
            }
            return productsDetails;
        }

        private string PreparedCombinationPrinter(RawPreparedCombination rawPreparedCombination)
        {
            string combinationDetails = $"\n{rawPreparedCombination.Name} = {rawPreparedCombination.BasicProductName}";
            IEnumerable<IGrouping<string, string>> toppingsByName = rawPreparedCombination.Toppings.GroupBy(topping => topping);

            foreach (IGrouping<string, string> toppingGroup in toppingsByName)
            {
                combinationDetails += $" + {toppingGroup.Count()} x {toppingGroup.Key}";
            }
            return combinationDetails;
        }
    }
}
