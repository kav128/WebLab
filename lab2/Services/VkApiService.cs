using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using lab2.Entities;
using lab2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace lab2.Services
{
    class VkApiService : IVkApiService
    {
        private readonly ILogger<VkApiService> _logger;
        private readonly string _appId;
        private readonly string _secret;
        private AccessTokenModel? _accessTokenModel;
        private const string RedirectUri = "https://localhost:5001/Auth";
        private const string ApiVersion = "5.126";

        public VkApiService(ILogger<VkApiService> logger, IConfiguration configuration)
        {
            _logger = logger;

            _appId = configuration["ApplicationId"];
            _secret = configuration["SecretKey"];
        }

        public async Task<User?> Auth(string code)
        {
            WebRequest request = WebRequest.CreateHttp($"https://oauth.vk.com/access_token?client_id={_appId}&client_secret={_secret}&redirect_uri={RedirectUri}&code={code}");
            string json;
            using WebResponse response = await request.GetResponseAsync();
            await using Stream stream = response.GetResponseStream();
            var accessTokenModel = await JsonSerializer.DeserializeAsync<AccessTokenModel>(stream);
            _accessTokenModel = accessTokenModel;
            _logger.LogInformation(_accessTokenModel?.ToString());

            return await GetUser(accessTokenModel!.UserId!.Value);
        }

        private async Task<User?> GetUser(int id)
        {
            if (_accessTokenModel is null) return null;
            
            const string methodName = "users.get";
            var uriString = $"https://api.vk.com/method/{methodName}?user_ids={id}&access_token={_accessTokenModel.AccessToken}&v={ApiVersion}";
            _logger.LogInformation(uriString);
            WebRequest request = WebRequest.CreateHttp(uriString);
            using WebResponse response = await request.GetResponseAsync();
            await using Stream stream = response.GetResponseStream();
            var profileResponseInfoModel = await JsonSerializer.DeserializeAsync<ApiResponseModel<UserInfoModel>>(stream);
            
            if (profileResponseInfoModel is null)
            {
                _logger.LogError("Cannot get profile info");
                return null;
            }

            if (profileResponseInfoModel.Response is null)
            {
                _logger.LogWarning($"Got error {profileResponseInfoModel.Error}");
                return null;
            }

            UserInfoModel? userInfoModel = profileResponseInfoModel.Response.FirstOrDefault();
            if (userInfoModel is null)
            {
                _logger.LogError("Couldn't get user from result set");
                return null;
            }

            _logger.LogInformation(userInfoModel.ToString());
            return new User(userInfoModel.FirstName, userInfoModel.LastName);
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        public string GetAuthorizeUrl() => $"https://oauth.vk.com/authorize?client_id={_appId}&display=page&redirect_uri={RedirectUri}&response_type=code&v={ApiVersion}";
    }
}
