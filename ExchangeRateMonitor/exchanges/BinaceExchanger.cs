using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchangeRate.Intarfaces;
using Newtonsoft.Json.Linq;

namespace CryptoExchangeRateAplication.exchanges
{
    public class BinaceExchanger : IExchanges
    {
        public static async Task<decimal?> GetCurentPriceAsync(string CurrencyPair = "ETHUSDT")
        {
            ClientWebSocket webSocket = new ClientWebSocket();
            Uri uri = new Uri($"wss://stream.binance.com:9443/ws/{CurrencyPair.ToLower()}@trade");

            try
            {
                await webSocket.ConnectAsync(uri, CancellationToken.None);
                Console.WriteLine("Connected to Binance WebSocket");

                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var json = JObject.Parse(message);
                var price = json["p"].ToObject<decimal>();

                return price;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open)
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                webSocket.Dispose();
            }
        }
    }
}

