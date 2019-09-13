using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.RequestModels;
using CodeChallenge.ResponseModels;

namespace CodeChallenge
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var cancelToken = tokenSource.Token;
            await ExecuteChallengeAsync(cancelToken);
        }


        public static async Task ExecuteChallengeAsync(CancellationToken cancelToken)
        {
            var service = new ApiService();
            var token = await service.GetTokenByApiAsync(cancelToken); // Get token from api call

            var magazineIdsByCategoriesTask = GetMagazineIdsByCategoriesAsync
                (token, (await service.GetCategoriesByApiAsync(token, cancelToken)).Data,service, cancelToken);  

            var subscribersTask = service.GetSubscribersByApiAsync(token, cancelToken); // Get subscribers by api call

            var magazineIdsByCategory = await magazineIdsByCategoriesTask;
            var subscribers = await subscribersTask;   // both subscribers and magazine are called async as they are not dependent on each other.

            var subscribersWithMagazineIds = GetSubscribersWithMagazineIds(subscribers);

            var subscriberSubscribedEachCategory = GetSubscriberSubscribedEachCategory(subscribersWithMagazineIds, magazineIdsByCategory);

            var answerRequest = new AnswerRequest() { Subscribers = subscriberSubscribedEachCategory };

            var result = await service.PostAnswerToApiAsync(token, answerRequest, cancelToken);

            PrintPropertiesOfAnswerObject(result);

        }


        //  get magazine Id by categories after getting categories from categories api call
        public static async Task<List<MagazineIdsByCategory>> GetMagazineIdsByCategoriesAsync(string token, List<string> categories, ApiService service, CancellationToken cancelToken)
        {
            var magazineIdsByCategories = categories.Select(async s => await GetMagazineIdByCategoryAsync(token, s,service, cancelToken)).ToList();
            return (await Task.WhenAll(magazineIdsByCategories)).ToList();
        }


        // get magazineIds by each category
        public static async Task<MagazineIdsByCategory> GetMagazineIdByCategoryAsync(string token, string category, ApiService service, CancellationToken cancelToken)
        {
            
            var magazineResponse = await service.GetMagazineByCategoryByApiAsync(token, category, cancelToken);
            return magazineResponse.Data.GroupBy(x => x.Category).Select(x =>
                new MagazineIdsByCategory
                {
                    Name = x.Key,
                    Ids = x.Select(z => z.Id).ToList()
                }).FirstOrDefault();
        }




        public static List<string> GetSubscriberSubscribedEachCategory(List<Subscribers> subscribers, List<MagazineIdsByCategory> magazineIdsByCategories)
        {
            return subscribers.Where(x => magazineIdsByCategories.All(y => y.Ids.Any(x.MagazineIds.Contains))).Select(x => x.Id).ToList();
            
        }

        public static List<Subscribers> GetSubscribersWithMagazineIds(SubscribersResponse response)
        {
            return response.Data.Select(x => new Subscribers { Id = x.Id, MagazineIds = x.MagazineIds }).ToList();
        }


        public static void PrintPropertiesOfAnswerObject(AnswerResponse answerResponse)
        {
            Console.WriteLine("answerCorrect: " + answerResponse.Data.AnswerCorrect);
            Console.WriteLine("totalTime: " + answerResponse.Data.TotalTime);
            Console.WriteLine("shouldBe: " + answerResponse.Data.ShouldBe);
            Console.WriteLine("success: " + answerResponse.Success);
            Console.WriteLine("token: " + answerResponse.Token);
            Console.WriteLine("message: " + answerResponse.Message);
            Console.ReadLine();
        }

    }


}
