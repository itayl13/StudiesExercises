
namespace VendingDP.Lib
{
    public class Topping
    {
        public readonly string name;
        public float Price { get; }
        public readonly string toppingFor;
        public Topping(string name, float price, string toppingFor)
        {
            this.name = name;
            Price = price;
            this.toppingFor = toppingFor;
        }
    }
}
