using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTextAssessment.Objects.Models
{
     public class Subscription
    {
        public string Id { get; set; }
        public int Months { get; set; }
        public decimal Price { get; set; }
    }
}
