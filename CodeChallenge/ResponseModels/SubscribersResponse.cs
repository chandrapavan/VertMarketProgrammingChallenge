using System;
using System.Collections.Generic;
using System.Text;
using CodeChallenge.Models;

namespace CodeChallenge.ResponseModels
{
    public class SubscribersResponse
    {
        public List<Subscriber> Data { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
    }
}
