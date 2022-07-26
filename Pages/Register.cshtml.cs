using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MITT_ROMMY_ZAMANUDDIN.Models;
using Newtonsoft.Json;

namespace MITT_ROMMY_ZAMANUDDIN.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserProfile UserProfile { get; set; } = new UserProfile();

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string BaseURL { get; set; } = "https://development.inhealth.co.id/samplebackend/api/";
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                string url = BaseURL + "UserProfile/Register";
                //Reservation receivedReservation = new Reservation();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(UserProfile), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<UserProfile>(apiResponse);
                        if (user.ERRORCODE > 0 || !string.Equals(user.ERRORDESC, "success", StringComparison.InvariantCultureIgnoreCase))
                        {
                            ErrorMessage = user.ERRORDESC;
                            return Page();
                        }
                        else
                        {
                            if (user.TAG != null && user.TAG.Length > 0)
                            {
                                SuccessMessage = user.TAG[0];
                            }
                            return Page();
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
