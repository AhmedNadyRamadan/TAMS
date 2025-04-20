using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TASM.ViewModels;

namespace TASM.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly IConfiguration _configuration;

        public MeetingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            var model = new MeetingViewModel
            {
                Timezone = "GMT+2",
                StartTime = DateTime.Now
            };

            return View(model);
        }

        public async Task<ActionResult> CreateMeeting(MeetingViewModel model)
        {
            string token = await GetAccessTokenAsync();

            string apiUrl = "https://api.zoom.us/v2/users/me/meetings";
            DateTime cairoStartTime = model.StartTime.AddHours(-2);

            if (model.StartTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("StartTime", "Start time cannot be in the past.");
                return View("Index", model);
            }
            var meeting = new
            {
                topic = model.Topic,
                type = 2, // Scheduled Meeting
                start_time = cairoStartTime.ToString("yyyy-MM-ddTHH:mm:ssZ"), 
                duration = model.Duration,
                timezone = "Africa/Cairo",
                settings = new
                {
                    host_video = model.HostVideo,
                    participant_video = model.ParticipantVideo
                }
            };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(meeting), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var meetingResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string joinUrl = meetingResponse.join_url;
                TempData["Message"] = "Zoom meeting created successfully.";
                TempData["JoinUrl"] = joinUrl;
            }
            else
            {
                TempData["Message"] = "Failed to create Zoom meeting.";
                TempData["JoinUrl"] = null;
            }

            return Redirect("/");
        }


        private async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();

            var clientId = _configuration["ZoomSettings:ZoomClientId"];
            var clientSecret = _configuration["ZoomSettings:ZoomClientSecret"];
            var accountId = _configuration["ZoomSettings:ZoomAccountId"];

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://zoom.us/oauth/token");
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))
            );

            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "account_credentials"),
        new KeyValuePair<string, string>("account_id", accountId)
    });

            var response = await client.SendAsync(tokenRequest);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get token: {json}");
            }

            var token = JsonConvert.DeserializeObject<dynamic>(json);
            return token.access_token;
        }

    }
}
