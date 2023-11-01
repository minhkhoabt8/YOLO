using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using SharedLib.Infrastructure.Services.Interfaces;


namespace SharedLib.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        private readonly string _type;
        private readonly string _projectTypeId;
        private readonly string _privateKeyId;
        private readonly string _privateKey;
        private readonly string _clientEmail;
        private readonly string _clientId;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _type = _configuration["FireBaseCredentials:type"]!;
            _projectTypeId = _configuration["FireBaseCredentials:project_id"]!;
            _privateKeyId = _configuration["FireBaseCredentials:private_key_id"]!;
            _privateKey = _configuration["FireBaseCredentials:private_key"]!;
            _clientEmail = _configuration["FireBaseCredentials:client_email"]!;
            _clientId = _configuration["FireBaseCredentials:client_id"]!;
        }

        public async Task<string> PushNotificationAsync(string userId, string title, string content)
        {
            var message = new Message()
            {
                Data = new Dictionary<string, string>
                {
                    { "user_id", userId },
                    { "title", title },
                    { "body", content }
                },
                Token = _privateKey, // Use the user's device token here
            };
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
