using System.Linq;
using System.Collections.Generic;

namespace VendingDP.Lib
{
    public class Order
    {
        private readonly List<CombinatedProduct> products;
        private readonly ItemDetectors itemDetectors = new ItemDetectors();
        private readonly ErrorHandler errorHandler = new ErrorHandler();
        public float Price { get; set; }
        public Menu Menu { get; set; }
        public Order(Menu menu)
        {
            Menu = menu;
            products = new List<CombinatedProduct>();
        }

        private void HandleSameCategoryExistingProduct(string productCategory)
        {
            CombinatedProduct matchingProductInMaking = products
                .Find(combinatedProduct => combinatedProduct.CombinatedProductStatus == CombinatedProductStatus.InMaking && 
                combinatedProduct.basicProduct.Category == productCategory);
            if (matchingProductInMaking != null)
            {
                matchingProductInMaking.CombinatedProductStatus = CombinatedProductStatus.Made;
            }
        }
        public void OrderProduct(string productName)
        {
            CombinatedProduct product = itemDetectors.DetectProduct(productName);
            if (product != null) 
            {
                if (product.CombinatedProductStatus == CombinatedProductStatus.InMaking)
                {
                    HandleSameCategoryExistingProduct(product.basicProduct.Category);
                }
                products.Add(product);
                Price += product.Price;
            }
            else
            {
                errorHandler.ItemNotFound(productName, "Product");
            }
        }
        public void AddTopping(string toppingName, int quantity = 1)
        {
            if (errorHandler.CheckToppingPositivity(quantity))
            {
                Item topping = itemDetectors.DetectTopping(toppingName);
                if (topping != null)
                {
                    CombinatedProduct productForTopping = products
                            .Find(product => product.CombinatedProductStatus == CombinatedProductStatus.InMaking &&
                            product.basicProduct.Category == topping.Category);
                    if (productForTopping != null)
                    {
                        productForTopping.AddTopping(topping, quantity);
                        Price += quantity * topping.Price;
                    }
                    else
                    {
                        errorHandler.NoProductToAddToppingTo(toppingName);
                    }
                }
                else
                {
                    errorHandler.ItemNotFound(toppingName, "Topping");
                }
            }
        }
        public void EndOrder()
        {
            foreach (CombinatedProduct product in products)
            {
                product.CombinatedProductStatus = CombinatedProductStatus.Made;
            }
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
