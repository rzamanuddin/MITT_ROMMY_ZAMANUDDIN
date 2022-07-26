using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MITT_ROMMY_ZAMANUDDIN.Models;
using Newtonsoft.Json;

namespace MITT_ROMMY_ZAMANUDDIN.Pages
{
    public class LoginModel : PageModel
    {
        public string ErrorMessage { get; set; }

        [BindProperty]
        public User UserLogin { get; set; } = new User();
        public string BaseURL { get; set; } = "https://development.inhealth.co.id/samplebackend/api/";

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {               
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                string url = BaseURL + "Users/Login";
                //Reservation receivedReservation = new Reservation();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(UserLogin), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(apiResponse);
                        if (!string.IsNullOrEmpty(user.message))
                        {
                            ErrorMessage = user.message;
                            return Page();
                        }
                        else
                        {
                            return RedirectToPage("/UserProfile", new { username = user.username });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
