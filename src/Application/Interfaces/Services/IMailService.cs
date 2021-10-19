using RuS.Application.Requests.Mail;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
        Task SendWelcomeEmailAsync(MailRequest request);
    }
}