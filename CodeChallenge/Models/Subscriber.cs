using System;
using System.Collections.Generic;
using System.Text;

namespace CodeChallenge.Models
{
    public class Subscriber
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> MagazineIds { get; set; }
    }
}
