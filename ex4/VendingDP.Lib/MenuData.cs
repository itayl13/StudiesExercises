using System.Collections.Generic;

namespace VendingDP.Lib
{
    public class MenuData
    {
        public IEnumerable<Item> BasicProducts { get; set; }
        public IEnumerable<Item> Toppings { get; set; }
        public IEnumerable<RawPreparedCombination> PreparedCombinations { get; set; }
    }
}
