using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using lab2.Exceptions;
using lab2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace lab2.Services
{
    public class VkAuthService : IVkAuthService
    {
        private const string ApiVersion = "5.126";
        private readonly string _appId;
        private readonly ILogger<VkAuthService> _logger;
        private readonly string _redirectUri;
        private readonly string _secret;

        public VkAuthService(ILogger<VkAuthService> logger, IConfiguration configuration)
        {
            _logger = logger;

            _appId = configuration["ApplicationId"];
            _secret = configuration["SecretKey"];

            string protocol = configuration["AppProtocol"];
            string domain = configuration["AppDomain"];
            int port = int.Parse(configuration["AppPort"]);
            var uriBuilder = new UriBuilder { Scheme = protocol, Host = domain, Path = "Auth", Port = port };
            _redirectUri = uriBuilder.ToString();

            AuthorizeUrl = new UriBuilder
            {
                Scheme = "https",
                Host = "oauth.vk.com",
                Path = "authorize",
                Query = $"client_id={_appId}&display=page&redirect_uri={_redirectUri}&response_type=code&v={ApiVersion}"
            }.ToString();
        }

        public string AuthorizeUrl { get; }

        public async Task<AccessTokenData> Auth(string code)
        {
            UriBuilder builder = new()
            {
                Scheme = "https",
                Host = "oauth.vk.com",
                Path = "access_token",
                Query = $"client_id={_appId}&client_secret={_secret}&redirect_uri={_redirectUri}&code={code}"
            };

            WebRequest request = WebRequest.CreateHttp(builder.ToString());
            using WebResponse response = await request.GetResponseAsync();
            await using Stream stream = response.GetResponseStream();
            var accessTokenModel = await JsonSerializer.DeserializeAsync<AccessTokenModel>(stream);
            if (accessTokenModel is null)
            {
                _logger.LogError("Cannot get access token data");
                throw new UnexpectedErrorException("Error while reading API response body.");
            }

            if (accessTokenModel.Error is not null)
            {
                _logger.LogWarning("Got error while authorization.");
                throw new VkApiException($"Cannot authorize: got error '{accessTokenModel.Error}' with description '{accessTokenModel.ErrorDescription}'");
            }

            _logger.LogInformation(accessTokenModel.ToString());
            var accessTokenData = new AccessTokenData
            {
                AccessToken = accessTokenModel.AccessToken!,
                ExpiresIn = accessTokenModel.ExpiresIn!.Value,
                UserId = accessTokenModel.UserId!.Value
            };
            return accessTokenData;
        }
    }
}
