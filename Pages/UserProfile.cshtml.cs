using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MITT_ROMMY_ZAMANUDDIN.Models;
using Newtonsoft.Json;

namespace MITT_ROMMY_ZAMANUDDIN.Pages
{
    public class UserProfileModel : PageModel
    {
        [BindProperty]
        public UserProfile UserProfile { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string BaseURL { get; set; } = "https://development.inhealth.co.id/samplebackend/api/";

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                string url = BaseURL + $"UserProfile/GetUserProfileByUsername?username={UserName}";
                //Reservation receivedReservation = new Reservation();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            UserProfile = JsonConvert.DeserializeObject<UserProfile>(apiResponse);
                        }
                        else
                        {
                            ErrorMessage = response.StatusCode.ToString();
                        }
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            
        }
    }
}
