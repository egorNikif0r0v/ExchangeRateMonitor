using CryptoExchangeRate.Intarfaces;
using Kucoin.Net.Clients;

namespace CryptoExchangeRateAplication.exchanges
{
    public class KucoinExchanger : IExchanges
    {
        public static async Task<decimal?> GetCurentPriceAsync(string CurrencyPair = "ETHUSDT")
        {
            try
            {
                var restClient = new KucoinRestClient();
                var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync($"{CurrencyPair}");
                var lastPrice = tickerResult.Data.LastPrice;
                return lastPrice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
