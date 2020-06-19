using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTextAssessment.Objects.Models
{
     public class ProductCategory
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public Product[] Products { get; set; }
    }
}
