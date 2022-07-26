using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MITT_ROMMY_ZAMANUDDIN.Models;
using Newtonsoft.Json;
using System.Text;

namespace MITT_ROMMY_ZAMANUDDIN.Pages
{
    public class UserProfileModel : PageModel
    {
        [BindProperty]
        public UserProfileResult UserProfile { get; set; } = new UserProfileResult();
        public UserSkillResult UserSkillResult { get; set; } = new UserSkillResult();
        public SkillResult SkillResult { get; set; } = new SkillResult();
        public SelectList SkillList { get; set; }
        public SkillLevelResult SkillLevelResult { get; set; } = new SkillLevelResult();
        public SelectList SkillLevelList { get; set; }

        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string BaseURL { get; set; } = "https://development.inhealth.co.id/samplebackend/api/";
        public string Token { get; set; }

        public async Task LoadSkillAsync()
        {
            string url = $"{BaseURL}Skill/GetSkills";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = "";
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        SkillResult = JsonConvert.DeserializeObject<SkillResult>(apiResponse);
                        if (UserProfile.ERRORCODE != 0)
                        {
                            ErrorMessage = UserProfile.ERRORDESC;
                        }
                        else
                        {
                            SkillList = new SelectList(SkillResult.TAG, nameof(Skill.SkillID), nameof(Skill.SkillName));
                        }
                    }
                    else
                    {
                        ErrorMessage = response.StatusCode.ToString();
                        ErrorMessage = Token;
                    }
                }
            }
        }

        public async Task LoadSkillLevelAsync()
        {
            string url = $"{BaseURL}SkillLevel/GetSkillLevels";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = "";
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        SkillLevelResult = JsonConvert.DeserializeObject<SkillLevelResult>(apiResponse);
                        if (UserProfile.ERRORCODE != 0)
                        {
                            ErrorMessage = UserProfile.ERRORDESC;
                        }
                        else
                        {
                            SkillLevelList = new SelectList(SkillLevelResult.TAG, nameof(SkillLevel.SkillLevelID), nameof(SkillLevel.SkillLevelName));
                        }
                    }
                    else
                    {
                        ErrorMessage = response.StatusCode.ToString();
                        ErrorMessage = Token;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostCreateSkillAsync()
        {
            try
            {
                string url = BaseURL + "UserSkills/Create";
                using (var httpClient = new HttpClient())
                {
                    var userSkillCreate = new UserSkillCreate();
                    userSkillCreate.username = UserName;
                    userSkillCreate.skillID = Convert.ToInt32(SkillList.SelectedValue);
                    userSkillCreate.skillLevelID = Convert.ToInt32(SkillLevelList.SelectedValue);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(userSkillCreate), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<UserSkillCreate>(apiResponse);
                        if (user.ERRORCODE > 0 || !string.Equals(user.ERRORDESC, "success", StringComparison.InvariantCultureIgnoreCase))
                        {
                            ErrorMessage = user.ERRORDESC;
                            return Page();
                        }
                        else
                        {
                            await OnGetAsync();
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
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                UserName = HttpContext.Session.GetString("username");
                Token = HttpContext.Session.GetString("jwtToken");

                await LoadSkillAsync();
                await LoadSkillLevelAsync();

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
