using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VendingDP.Lib
{
    public class MenuLoader
    {
        /* The logic of basic products and toppings is as follows: 
            * they are held in a list of lists, each item of the outer list represents a product/topping.
            * The inner lists are lists of [name, price, category], where the name will help identifying the ordered 
            * item and the category will help associating toppings to a given basic product.
            */
        public static List<List<object>> basicProducts = new List<List<object>>();
        public static List<List<object>> toppings = new List<List<object>>();
        /* The logic of the prepared combinations is different:
            * they are held in a dictionary where the keys are the combinations' names and the value is a list 
            * of "instructions" how to make them: the first item is always the basic product, 
            * then the next two items are a topping name and its quantity in the combination.
            */
        public static Dictionary<string, List<object>> preparedCombinations = new Dictionary<string, List<object>>();

        public MenuLoader()
        {
            string[] lines = ReadFile();
            int lineIndex = 0;
            while (lineIndex < lines.Length)
            {
                if (lines[lineIndex].StartsWith("Basic Products:"))
                {
                    BuildProductsOrToppings(ref lineIndex, lines, basicProducts);
                }
                else if (lines[lineIndex].StartsWith("Toppings:"))
                {
                    BuildProductsOrToppings(ref lineIndex, lines, toppings);
                }
                else if (lines[lineIndex].StartsWith("Prepared Combinations:"))
                {
                    BuildPreparedCombinations(ref lineIndex, lines);
                }
                lineIndex += 1;
            }
        }
        private string[] ReadFile()
        {
            string menuPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName,
                "VendingDP.Lib", "Menu.txt");
            return File.ReadAllLines(menuPath);
        }
        private void BuildProductsOrToppings(ref int lineIndex, string[] lines, List<List<object>> itemsCollection)
        {
            try
            {
                lineIndex += 1;
                while (lines[lineIndex] != "")
                {
                    string[] productDetails = lines[lineIndex].Split("\t".ToCharArray(),
                        options: StringSplitOptions.RemoveEmptyEntries);
                    itemsCollection.Add(
                        new List<object>()
                        {
                                productDetails[(int)ItemDetailColumns.Name],
                                float.Parse(productDetails[(int)ItemDetailColumns.Price]),
                                productDetails[(int)ItemDetailColumns.ProductCategory],
                        }
                        );
                    lineIndex += 1;
                }
            }
            catch (IndexOutOfRangeException)
            {
                ErrorHandler.TabDelimitingIssue();
            }
            catch (FormatException)
            {
                ErrorHandler.ParsingIssue(lines[lineIndex]);
            }
        }

        private void BuildPreparedCombinations(ref int lineIndex, string[] lines)
        {
            lineIndex += 1;
            while (lineIndex < lines.Length && lines[lineIndex] != "")
            {
                string[] combinationDetails = lines[lineIndex].Split("\t".ToCharArray(),
                    options: StringSplitOptions.RemoveEmptyEntries);
                AddPreparedCombination(combinationDetails);
                lineIndex += 1;
            }
        }

        private void AddPreparedCombination(string[] combinationDetails)
        {
            try
            {
                List<object> combinationProductsAndToppings = new List<object>()
                { combinationDetails[(int)PreparedCombinationColumns.BasicProductName] };
                string[] toppingsDetails = combinationDetails[(int)PreparedCombinationColumns.Toppings]
                    .Split("+".ToCharArray());
                foreach (string countAndTopping in toppingsDetails)
                {
                    string[] countToppingDetails = countAndTopping.Split(" ".ToCharArray(),
                        options: StringSplitOptions.RemoveEmptyEntries);
                    int quantity = int.Parse(countToppingDetails[0]);
                    string toppingName = string.Join(" ", countToppingDetails.Skip(1));
                    combinationProductsAndToppings.Add(toppingName);
                    combinationProductsAndToppings.Add(quantity);
                }
                preparedCombinations.Add(
                combinationDetails[(int)PreparedCombinationColumns.Name],
                combinationProductsAndToppings);
            }
            catch (IndexOutOfRangeException)
            {
                ErrorHandler.TabDelimitingIssue();
            }
            catch (FormatException)
            {
                ErrorHandler.ParsingIssue(string.Join("\t", combinationDetails));
            }
        }
    }
    public enum ItemDetailColumns { Name, Price, ProductCategory }
    public enum PreparedCombinationColumns { Name, BasicProductName, Toppings }
}
