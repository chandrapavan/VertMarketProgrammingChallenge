using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenge.ResponseModels
{
    public class CategoryResponse
    {
        public List<string> Data { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }

    }
}
