using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.RequestModels;
using CodeChallenge.ResponseModels;
using Newtonsoft.Json;

namespace CodeChallenge
{
    public class ApiService
    {
        protected static HttpClient HttpClient = new HttpClient();
        public  async Task<string> GetTokenByApiAsync(CancellationToken cancelToken)
        {
            var url = WebApiUrl.GetConnectionStringForToken();
            var tokenResponse = await WebApiHelper.GetWebApiResponseAsync<TokenResponse>(url, HttpMethod.Get, HttpClient, string.Empty, cancelToken);
            if (!tokenResponse.Success) throw new Exception("Token request was not successful");
            return tokenResponse.Token;
        }

        public async Task<CategoryResponse> GetCategoriesByApiAsync(string token,CancellationToken cancelToken)
        {
            
            var url = WebApiUrl.GetConnectionStringForCategories(token);
            var response = await WebApiHelper.GetWebApiResponseAsync<CategoryResponse>(url, HttpMethod.Get, HttpClient, string.Empty, cancelToken);
            if (!response.Success) throw new Exception("Web Api  category request was not successful");
            return response;
        }
        public async Task<MagazineResponse> GetMagazineByCategoryByApiAsync(string token,string category, CancellationToken cancelToken)
        {

            var url = WebApiUrl.GetConnectionStringForMagazines(token, category);
            var response = await WebApiHelper.GetWebApiResponseAsync<MagazineResponse>(url, HttpMethod.Get, HttpClient, string.Empty, cancelToken);
            if (!response.Success) throw new Exception("Web Api  magazine request was not successful");
            return response;
        }

        public  async Task<SubscribersResponse> GetSubscribersByApiAsync(string token, CancellationToken cancelToken)
        {
            
            var url = WebApiUrl.GetConnectionStringForSubscribers(token);
            var response = await WebApiHelper.GetWebApiResponseAsync<SubscribersResponse>(url, HttpMethod.Get, HttpClient, string.Empty, cancelToken);
            if (!response.Success) throw new Exception("Web Api  subscriber request was not successful");
            return response;

        }

        public  async Task<AnswerResponse> PostAnswerToApiAsync(string token,AnswerRequest answerRequest, CancellationToken cancelToken)
        {
           
            var url = WebApiUrl.GetConnectionStringForAnswer(token);
            var body = JsonConvert.SerializeObject(answerRequest);
            var response = await WebApiHelper.GetWebApiResponseAsync<AnswerResponse>(url, HttpMethod.Post, HttpClient, body, cancelToken);
            return response;
        }



        



    }
}