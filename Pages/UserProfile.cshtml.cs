using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MITT_ROMMY_ZAMANUDDIN.Models;
using Newtonsoft.Json;

namespace MITT_ROMMY_ZAMANUDDIN.Pages
{
    public class UserProfileModel : PageModel
    {
        [BindProperty]
        public UserProfileResult UserProfile { get; set; } = new UserProfileResult();
        public UserSkillResult UserSkillResult { get; set; } = new UserSkillResult();
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string BaseURL { get; set; } = "https://development.inhealth.co.id/samplebackend/api/";
        public string Token { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                UserName = HttpContext.Session.GetString("username");
                Token = HttpContext.Session.GetString("jwtToken");
                string url = $"{BaseURL}UserProfile/GetUserProfileByUsername?username={UserName}";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    using (var response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = "";
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                            UserProfile = JsonConvert.DeserializeObject<UserProfileResult>(apiResponse);
                            if (UserProfile.ERRORCODE == 0)
                            {
                                url = $"{BaseURL}UserSkills/GetUserSkillByUsername?username={UserName}";
                                using (var client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                                    using (var clientResponse = await httpClient.GetAsync(url))
                                    {
                                        var userSkillResponse = await clientResponse.Content.ReadAsStringAsync();
                                        UserSkillResult = JsonConvert.DeserializeObject<UserSkillResult>(userSkillResponse);

                                        if (UserSkillResult.ERRORCODE != 0)
                                        {
                                            ErrorMessage = UserSkillResult.ERRORDESC;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                ErrorMessage = UserProfile.ERRORDESC;
                            }
                        }
                        else
                        {
                            ErrorMessage = response.StatusCode.ToString();
                            ErrorMessage = Token;
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
