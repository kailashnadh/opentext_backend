using Newtonsoft.Json;
using OpenTextAssessment.BusinesLogic;
using OpenTextAssessment.Objects.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenTextAssessment
{

    /**
     * Thanks for providing the opportunity to take test.
     * Specific changes to highlight would be to make application loosely coupled
     * I have divided application into 3 projects (main, business logic, models).
     * 
     * Divided code into multiple functions and changed  logic for sorting, getting full subscription
     * price per user, for Merging products, Display users. Also added multiple null checks to aviod 
     * unexpected exceptions.
     * 
     * Created configuration file to access the url of the API.
     */
    public class Program
    {     

        static async Task Main(string[] args)
        {
            string apiUrl=Convert.ToString(ConfigurationManager.AppSettings["ApiUrl"]);
            Services service = new Services(apiUrl);
            // get user and product data
            var responseData = new ResponseData();
            try
            {
                Console.WriteLine("Fetching Product and User data...");
                string responseBody = await Task.Run(()=> service.GetUserAndProductData());
                responseData = JsonConvert.DeserializeObject<ResponseData>(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            //create generic class
            if (responseData != null)
            {
                if (responseData.Users != null)
                {
                    User[] users = (User[])responseData.Users.Clone();

                    // sort users by last name
                    users = users.OrderBy(user => user.LastName).ToArray();

                    if (responseData.ProductCategories != null)
                    {
                        // add up all users' total subscription costs
                        service.GetTotalSubscriptionCost(ref users, responseData.ProductCategories);
                    }

                    // display sorted users with their data and sub costs
                    DisplayUsers(users);
                }

                else
                {
                    Console.WriteLine("No Users");
                }

            }



            if (responseData.ProductCategories != null)
            {
                // merge consumer and smb products into a single collection
                var allProducts = service.MergeProducts(responseData.ProductCategories);
                // display products
                DisplayProducts(allProducts);
            }

            else
            {
                Console.WriteLine("No Products");
            }
           
        }

        //Method to Display Users
        static void DisplayUsers(User[] users)
        {
            Console.WriteLine();
            Console.WriteLine("Users:");
            Console.WriteLine("********************************");
            //used foreach
            foreach (var user in users)
            {
                Console.WriteLine($"Full Name:\t{user.FirstName} {user.LastName}");
                Console.WriteLine("Email:\t\t" + user.Email);
                Console.WriteLine("Total Subscription Cost: $" + user.TotalSubscriptionCost);
                Console.WriteLine();
            }
        }

        //Method to Display Products
        static void DisplayProducts(Product[] allProducts)
        {
            Console.WriteLine();
            Console.WriteLine("Products:");
            Console.WriteLine("********************************");
            foreach (var product in allProducts)
            {
                Console.WriteLine($"Name: {product.DisplayName}\t\tSKU: {product.Sku}\t\tCategory: {product.Category}");
            }
        }
    }
}
