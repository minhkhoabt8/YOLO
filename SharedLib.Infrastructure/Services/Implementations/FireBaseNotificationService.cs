using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using SharedLib.Infrastructure.Services.Interfaces;


namespace SharedLib.Infrastructure.Services.Implementations
{
    public class FireBaseNotificationService : IFireBaseNotificationService
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public FireBaseNotificationService(IConfiguration configuration)
        {
            var googleCredentialPath = configuration.GetSection("GoogleFirebase")["firebase-credentials.json"];
            var credential = GoogleCredential.FromFile("firebase-credentials.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });

            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }

        public async Task<string> SendNotificationToUserAsync(string userId, string title, string body)
        {
            //UserId is treated as token

            if (userId != null)
            {
                var message = new Message
                {
                    Token = userId,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    }
                };

                var response = await _firebaseMessaging.SendAsync(message);

                return $"Successfully sent message to user {userId}: {response}";
            }
            else
            {
                return $"User {userId} does not have a valid registration token.";
            }
        }
    }
}
