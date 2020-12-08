using System.Text.RegularExpressions;

namespace VendingDP.Lib.Commands
{
    public class Add : Command
    {
        public override string CommandPrintingFormat() => "Add [toppingName] [quantity = 1]";

        public override void Run(ref Order currentOrder)
        {
            string toppingDetails = Regex.Match(fullCommand, @"(?i)(?<=Add).*").ToString().Trim();

            if (Regex.Match(toppingDetails, @"\d$").Success)
            {
                string toppingName = Regex.Match(toppingDetails, @".*\D(?=-?\d+$)").Value.Trim();
                string quantityAsString = Regex.Match(toppingDetails, @"-?\d+$").Value;
                int quantity = int.Parse(quantityAsString);
                currentOrder.AddTopping(toppingName, quantity);
            }

            else
            {
                currentOrder.AddTopping(toppingDetails);
            }
        }
    }
}
