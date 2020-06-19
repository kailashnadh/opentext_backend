using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OpenTextAssessment.Objects.Models;

namespace OpenTextAssessment.BusinesLogic
{
    public class Services
    {
        public static readonly HttpClient httpClient = new HttpClient();
        private string url;

        public Services(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Method to get total subscription cost per each user
        /// </summary>
        /// <param name="users"></param>
        /// <param name="productCategories"></param>
        public void GetTotalSubscriptionCost(ref User[] users, ProductCategory[] productCategories)
        {
            Dictionary<string, float> subscriptionPrices = GetSubscriptionPrices(productCategories);
            if (users != null)
            {
                for (int i = 0; i < users.Length; i++)
                {
                    float totalPrice = 0f;
                    foreach (var subId in users[i].SubscriptionIds)
                    {
                        totalPrice += subscriptionPrices.GetValueOrDefault(subId);
                    }
                    users[i].TotalSubscriptionCost = (decimal)totalPrice;
                }
            }                        
                   
        }

        
        /// <summary>
        /// Method to get subscription price for each product
        /// </summary>
        /// <param name="productCategories"></param>
        /// <returns></returns>
        public static Dictionary<string,float> GetSubscriptionPrices(ProductCategory[] productCategories)
        {
            Dictionary<string, float> subscriptionPrices = new Dictionary<string, float>();
            foreach (var productCategory in productCategories)
            {
                if (productCategory.Products != null)
                {
                    foreach (var product in productCategory.Products)
                    {
                        if (product.Subscriptions != null)
                        {
                            foreach (var subscription in product.Subscriptions)
                            {
                                subscriptionPrices.Add(subscription.Id, (float)subscription.Price);
                            }
                        }                     
                    }
                }
                

            }
            return subscriptionPrices;
        }

        /// <summary>
        /// Calls Api
        /// </summary>
        /// <returns> String response</returns>
        public async Task<String> GetUserAndProductData()
        {
            string responseBody;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Could not get User and Product Data"+ex.Message);
            }


        }

        /// <summary>
        /// Merges all products sorted by product categories
        /// </summary>
        /// <param name="productCategories"></param>
        /// <returns></returns>
        public Product[] MergeProducts(ProductCategory[] productCategories)
        {
            Product[] consumerProducts = productCategories.First(p => p.Type == "Consumer").Products;
            Product[] smbProducts = productCategories.First(p => p.Type == "SMB").Products;
            Product[] allProducts = new Product[consumerProducts.Length + smbProducts.Length];
            int index = 0;
            GetProductsByCategory("Consumer", ref allProducts,consumerProducts,index);
            index = consumerProducts.Length ;
            GetProductsByCategory("SMB", ref allProducts, smbProducts, index);
            return allProducts;
        }

        /// <summary>
        /// Gets the products info by category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="allProducts"></param>
        /// <param name="productsPerType"></param>
        /// <param name="index"></param>
        public void GetProductsByCategory(string categoryName,ref Product[] allProducts,Product[] productsPerType,int index)
        {
            if(allProducts!=null && productsPerType != null)
            {
                for (int i = 0; i < productsPerType.Length; i++)
                {
                    allProducts[index] = productsPerType[i];
                    allProducts[index].Category = categoryName;
                    index++;
                }
            }
           
        }


    }
}
