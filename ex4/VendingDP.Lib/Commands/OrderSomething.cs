using System.Text.RegularExpressions;

namespace VendingDP.Lib.Commands
{
    public class OrderSomething : Command
    {
        public override string CommandPrintingFormat() => "Order [productName]";

        public override void Run(ref Order currentOrder)
        {
            string productName = Regex.Match(fullCommand, @"(?i)(?<=Order).*").ToString().Trim();
            currentOrder.OrderProduct(productName);
        }
    }
}
