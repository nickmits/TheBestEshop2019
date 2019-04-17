using Microsoft.ESportShop.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Infrastructure.Services
{
    
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
