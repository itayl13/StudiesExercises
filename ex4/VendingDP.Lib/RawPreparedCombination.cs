using System.Collections.Generic;

namespace VendingDP.Lib
{
    public class RawPreparedCombination
    {
        public string Name { get; set; }
        public string BasicProductName { get; set; }
        public IEnumerable<string> Toppings { get; set; }
    }
}
