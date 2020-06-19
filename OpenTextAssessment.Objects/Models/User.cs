using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTextAssessment.Objects.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] SubscriptionIds { get; set; }
        public decimal TotalSubscriptionCost { get; set; }
    }
}
