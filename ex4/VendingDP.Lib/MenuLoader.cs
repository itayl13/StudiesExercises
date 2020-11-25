using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace VendingDP.Lib
{
    public class MenuLoader : MenuData
    {
        public MenuLoader()
        {
            StreamReader menuFile = ReadFile();
            string json = menuFile.ReadToEnd();
            MenuData menuData = JsonConvert.DeserializeObject<MenuData>(json);
            BasicProducts = menuData.BasicProducts;
            Toppings = menuData.Toppings;
            PreparedCombinations = menuData.PreparedCombinations;
        }
        private StreamReader ReadFile()
        {
            AppSettingsReader reader = new AppSettingsReader();
            string filePath = reader.GetValue("File path", typeof(string)).ToString();
            string fileName = reader.GetValue("File Name", typeof(string)).ToString();
            string menuPath = Path.Combine(filePath, fileName);
            return new StreamReader(menuPath);
        }
    }
}
