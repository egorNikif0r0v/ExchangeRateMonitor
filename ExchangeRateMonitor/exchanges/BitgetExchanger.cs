using Bitget.Net.Clients;
using CryptoExchangeRate.Intarfaces;
using System;

namespace CryptoExchangeRateAplication.exchanges
{
    public class BitgetExchanger : IExchanges
    {
        public static async Task<decimal?> GetCurentPriceAsync(string CurrencyPair = "ETHUSDT")
        {
            try
            {
                var restClient = new BitgetRestClient();
                var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync($"{CurrencyPair}_SPBL");
                var lastPrice = tickerResult.Data.ClosePrice;
                return (lastPrice);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}