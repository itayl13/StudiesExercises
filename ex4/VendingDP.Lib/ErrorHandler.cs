using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace VendingDP.Lib
{
    public static class ErrorHandler
    {
        // Menu loading issues:
        public static void TabDelimitingIssue()
        {
            string tabIssue = "Something is wrong with the tab delimiting";
            throw new InvalidDataException(tabIssue);
        }
        public static void ParsingIssue(string item)
        {
            string wrongProductsMessage = "The following item has an incorrect form:\n" + item;
            throw new InvalidDataException(wrongProductsMessage);
        }

        public static void InvalidMenuItems(IEnumerable<object> wrongItems)
        {
            if (wrongItems.Count() >= 1)
            {
                string message = MenuItemsMessage(wrongItems);
                throw new InvalidDataException(message);
            }
        }
        private static string MenuItemsMessage(IEnumerable<object> wrongItems)
        {
            // If the items are mismatching toppings:
            if (wrongItems as IEnumerable<List<object>> != null)
            {
                string message = string.Join("\n", ((IEnumerable<List<object>>)wrongItems)
                    .Select(productDetails => string.Join(", ", productDetails)));
                return "The following toppings belong to an unknown product category:\n" + message;
            }
            // If the items are wrongly typed prepared combinations:
            else // wrongItems is of type IEnumerable<KeyValuePair<string, List<object>>>.
            {
                string message = string.Join("\n", ((IEnumerable<KeyValuePair<string, List<object>>>)wrongItems)
                    .Select(combinationKeyValue => combinationKeyValue.Key + ": " +
                        string.Join(", ", combinationKeyValue.Value)));
                return "The following combinations have an incorrect form (missing value or wrong writing):\n" + message;
            }
        }

        // Ordering Issues:
        public static void CheckToppingPositivity(int quantity)
        {
            if (quantity <= 0)
            {
                // Removing a topping is possible, but is not for use.
                throw new ArgumentOutOfRangeException("Toppings can be added only a positive number of times. Topping was not added");
            }
        }
        public static void ItemNotFound(string message, string what)
        {
            Console.WriteLine(message + $" {what} was not ordered.");
        }
        public static void NonPositiveQuantity(string message)
        {
            Console.WriteLine(message);
        }
        public static void NoProductToAddToppingTo(string toppingName)
        {
            Console.WriteLine($"Topping {toppingName} cannot be added to any product in the order. Topping was not added");
        }
    }
}
