using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTextAssessment.Objects.Models
{
    public class Product
    {

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Sku { get; set; }
        public Subscription[] Subscriptions { get; set; }
        public string Category { get; set; }
    }
}
