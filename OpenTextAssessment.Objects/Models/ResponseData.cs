using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTextAssessment.Objects.Models
{
    public class ResponseData
    {
        public ProductCategory[] ProductCategories { get; set; }
        public User[] Users { get; set; }
    }
}
