using Xamarin.Forms;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bitcoin
{
    public partial class BitcoinPage : ContentPage
    {
        static Dictionary<String, String> currencySymbols = new Dictionary<String, String>
        {
            ["USD"] = "$",
            ["GBP"] = "£",
            ["EUR"] = "€"
        };
        string selectedCurrency = "USD";

        public BitcoinPage()
        {
            InitializeComponent();

            UpdateBitcoinLabel();
        }

        async Task UpdateBitcoinLabel(bool startTimer = true)
        {
            const string bitcoinAddress = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var client = new HttpClient();
            var bitcoinJSON = await client.GetStringAsync(bitcoinAddress);
            var bitcoinObject = JObject.Parse(bitcoinJSON);
            var bitcoinPrice = (double)bitcoinObject["bpi"][selectedCurrency]["rate_float"];
            BitcoinPriceLabel.Text = String.Format("{0}{1:.00}", currencySymbols[selectedCurrency], bitcoinPrice);
            if (startTimer)
                Device.StartTimer(TimeSpan.FromMinutes(1), () =>
                {
                    UpdateBitcoinLabel();
                    return false;
                });
        }

        void ChangeCurrency(object sender, EventArgs e)
        {
            selectedCurrency = ((Button)sender).Text;
            foreach (var child in BitcoinGrid.Children)
            {
                if (child.GetType().ToString() == "Xamarin.Forms.Button")
                {
                    if (selectedCurrency == ((Button)child).Text)
                    {
                        ((Button)child).BackgroundColor = Color.White;
                        ((Button)child).TextColor = Color.FromHex("#202020");
                    }
                    else
                    {
                        ((Button)child).BackgroundColor = Color.FromHex("#202020");
                        ((Button)child).TextColor = Color.White;
                    }
                }
            }
            UpdateBitcoinLabel(false);
        }
    }
}
