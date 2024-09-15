using System.Text.Json;
using TradingApp.Helpers;

namespace TradingApp.Contexts
{
    public class AlpacaMDContext(HttpClient client)
    {
        private readonly HttpClient _client = client;

        // HttpClient comes in through DI with preset key, secret, and base address

        private async Task<HttpResponseMessage?> SendRequest(AlpacaMDRequest request)
        {
            // add query parameters to request
            var requestUri = $"?symbols={request.Symbol}&timeframe={request.TimeFrame}";

            // add optional query parameters to request if they exist
            if (request.Start != null) { requestUri += $"&start={request.Start}"; }
            if (request.End != null) { requestUri += $"&end={request.End}"; }
            if (request.Limit != null) { requestUri += $"&limit={request.Limit}"; }
            if (request.Adjustment != null) { requestUri += $"&adjustment={request.Adjustment}"; }
            if (request.AsOf != null) { requestUri += $"&as_of={request.AsOf}"; }
            if (request.Feed != null) { requestUri += $"&feed={request.Feed}"; }
            if (request.Format != null) { requestUri += $"&format={request.Format}"; }
            if (request.PageToken != null) { requestUri += $"&page_token={request.PageToken}"; }
            if (request.Sort != null) { requestUri += $"&sort={request.Sort}"; }

            // send request to Alpaca API
            var response = await _client.GetAsync(requestUri);

            // return response
            return response;
        }

        public async Task<AlpacaMDResponse?> GetBars(AlpacaMDRequest request)
        {
            // send request to Alpaca API
            var response = await SendRequest(request);

            // if response is successful, return deserialized response
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AlpacaMDResponse>(content);
                return result;
            }

            // if response is not successful, return null
            return null;
        }
    }
}