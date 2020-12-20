using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using lab2.Entities;
using lab2.Exceptions;
using lab2.Models;
using Microsoft.Extensions.Logging;

namespace lab2.Services
{
    public class VkApiService : IVkApiService
    {
        private const string ApiVersion = "5.126";
        private readonly ILogger<VkApiService> _logger;

        public VkApiService(ILogger<VkApiService> logger)
        {
            _logger = logger;
        }

        public async Task<User> GetUser(int id, string accessCode)
        {
            if (accessCode is null)
                throw new ArgumentNullException(nameof(accessCode));

            const string methodName = "users.get";
            var parameters = new NameValueCollection
            {
                { "user_ids", id.ToString() },
                { "fields", "photo_100" }
            };
            string jsonString = await InvokeMethod(methodName, parameters, accessCode);
            var profileResponseInfoModel = JsonSerializer.Deserialize<ApiResponseModel<UserInfoModel>>(jsonString);

            if (profileResponseInfoModel is null)
            {
                _logger.LogError("Cannot get profile info");
                throw new UnexpectedErrorException("Error while reading API response body.");
            }

            if (profileResponseInfoModel.Response is null)
            {
                _logger.LogWarning($"Got error {profileResponseInfoModel.Error}");
                throw new VkApiException($"VK API returned error with code {profileResponseInfoModel.Error?.ErrorCode} and description: '{profileResponseInfoModel.Error?.ErrorMessage}'.");
            }

            UserInfoModel? userInfoModel = profileResponseInfoModel.Response.FirstOrDefault();
            if (userInfoModel is null)
            {
                _logger.LogError("Couldn't get user from result set");
                throw new UnexpectedErrorException("VK API returned empty users list.");
            }

            _logger.LogInformation(userInfoModel.ToString());
            return new User(userInfoModel.FirstName, userInfoModel.LastName) { ProfilePic = userInfoModel.ProfilePic100 };
        }

        private async Task<string> InvokeMethod(string method, NameValueCollection parameters, string accessToken)
        {
            parameters.Add("access_token", accessToken);
            parameters.Add("v", ApiVersion);
            IEnumerable<string> parametersCollection = parameters.AllKeys.Select(key =>
                $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(parameters[key])}");
            string queryString = string.Join('&', parametersCollection);

            UriBuilder builder = new()
            {
                Scheme = "https",
                Host = "api.vk.com",
                Path = $"method/{method}",
                Query = queryString
            };

            var uriString = builder.ToString();
            _logger.LogInformation($"Invoking VK API method: '{uriString}'");
            WebRequest request = WebRequest.CreateHttp(uriString);
            using WebResponse response = await request.GetResponseAsync();
            await using Stream stream = response.GetResponseStream();
            using var streamReader = new StreamReader(stream);
            string responseString = await streamReader.ReadToEndAsync();
            _logger.LogInformation($"Got response: {responseString}");
            return responseString;
        }
    }
}
