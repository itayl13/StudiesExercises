using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace VendingDP.Lib
{
    public class ErrorHandler
    {
        public void IsDataCollectionEmpty<T>(IEnumerable<T> collection, string collectionName)
        {
            if (collection == null || collection.Count() == 0)
            {
                string message = $"No {collectionName} data were loaded (Possibly due to JSON format).";
                throw new InvalidDataException(message);
            }
        }

        public void InvalidMenuItems(IEnumerable<string> wrongToppings, string whatBeingTested)
        {
            if (wrongToppings.Count() >= 1)
            {
                string message = MenuItemsMessage(wrongToppings, whatBeingTested);
                throw new InvalidDataException(message);
            }
        }
        
        private string MenuItemsMessage(IEnumerable<string> wrongItems, string whatBeingTested)
        {
            string message = string.Join("\n", wrongItems);

            if (whatBeingTested == "toppings")
            {
                
                return "The following toppings belong to an unknown product category:\n" + message;
            }

            else 
            {
                return "The following combinations have an incorrect form (wrong basic product or toppings):\n" + message;
            }
        }

        public bool CheckToppingPositivity(int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Toppings can be added only a positive number of times. Topping was not added.");
                return false;
            }
            return true;
        }

        public void ItemNotFound(string itemName, string what)
        {
            string message;

            if (what == "Product")
            {
                message = $"No product named {itemName} was found.";
            }

            else
            {
                message = $"No topping named {itemName} was found (Maybe wrong typing format?).";
            }
            Console.WriteLine(message + $" {what} was not ordered.");
        }

        public void NoProductToAddToppingTo(string toppingName)
        {
            Console.WriteLine($"Topping {toppingName} cannot be added to any product in the order. Topping was not added");
        }
    }
}
