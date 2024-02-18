using Microsoft.Extensions.Logging;


namespace Jewelry.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }
        public void SendMessage(string to, string subject, string body)
        {
            _logger.LogInformation($"to: {to} Subject: {subject} Body: {body}");
        }
    }
}
