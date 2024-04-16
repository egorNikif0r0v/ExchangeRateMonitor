using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CryptoExchangeRate.Intarfaces
{
    internal interface IExchanges
    {
        public static abstract Task<decimal?> GetCurentPriceAsync(string CurrencyPair);
    }
}
