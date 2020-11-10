using System;
using System.Linq;
using System.Collections.Generic;

namespace VendingDP.Lib
{
    public class Order
    {
        private readonly List<CombinatedProduct> products;
        public float Price { get; set; }
        public string Menu { get; set; }
        public Order()
        {
            products = new List<CombinatedProduct>();
        }

        private void HandleSameCategoryExistingProduct(CombinatedProduct product)
        {
            // Find if there's another product in making that is of the same category as `product`. 
            // If so, move it to the finished product. Otherwise, no problem and continue as usual:
            try
            {
                CombinatedProduct matchingProductInMaking = products
                .Find(combinatedProduct => combinatedProduct.CombinatedProductStatus == CombinatedProductStatus.InMaking &&
                combinatedProduct.basicProduct.productCategory == product.basicProduct.productCategory);
                matchingProductInMaking.CombinatedProductStatus = CombinatedProductStatus.Made;
            }
            catch (NullReferenceException) { }
        }
        public void OrderProduct(string productName)
        {
            try
            {
                CombinatedProduct product = ItemDetectors.DetectProduct(productName);
                if (product.CombinatedProductStatus == CombinatedProductStatus.InMaking)
                {
                    HandleSameCategoryExistingProduct(product);
                }
                products.Add(product);
                Price += product.Price;
            }
            catch (KeyNotFoundException ex)
            {
                ErrorHandler.ItemNotFound(ex.Message, "Product");
            }
        }
        public void AddTopping(string toppingName, int quantity = 1)
        {
            {
                try
                {
                    ErrorHandler.CheckToppingPositivity(quantity);
                    Topping topping = ItemDetectors.DetectTopping(toppingName);
                    CombinatedProduct productForTopping = products
                        .First(product => product.CombinatedProductStatus == CombinatedProductStatus.InMaking && 
                        product.basicProduct.productCategory == topping.toppingFor);
                    productForTopping.AddTopping(topping, quantity);
                    Price += quantity * topping.Price;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    ErrorHandler.NonPositiveQuantity(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    ErrorHandler.ItemNotFound(ex.Message, "Topping");
                }
                catch (InvalidOperationException)
                {
                    ErrorHandler.NoProductToAddToppingTo(toppingName);
                }
            }
        }
        public void EndOrder()
        {
            foreach (CombinatedProduct product in products)
            {
                product.CombinatedProductStatus = CombinatedProductStatus.Made;
            }
            // I guess in real life a RequestPayment method would've been here before cleaning. Here I just clear.
            products.Clear();
        }
        public override string ToString() 
        {
            string message = "Current Order:\n";
            IEnumerable<string> currentProductsDetails = products
                .Select(product => product?.ToString());
            message += string.Join("\n", currentProductsDetails);
            message += $"\nTotal: {Price} $";
            return message;
        }
    }
}
