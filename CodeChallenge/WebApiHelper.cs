using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.RequestModels;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using RestSharp.Serialization.Json;

namespace CodeChallenge
{
    public class WebApiHelper
    {
        public static readonly int MaxRetryAttempts = 2;
        public static readonly TimeSpan PauseBetweenFailures = TimeSpan.FromSeconds(2);

        public static async Task<TResult> GetWebApiResponseAsync<TResult>(string uri, HttpMethod verb, HttpClient httpClient, string request, CancellationToken cancellationToken)
        {

            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(MaxRetryAttempts, i => PauseBetweenFailures);

            var httpRequest = new HttpRequestMessage(verb, uri)
            {
                Content = content
            };

            var responseString = await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();

            });
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

    }
}
