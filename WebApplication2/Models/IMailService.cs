using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public interface IMailService
    {
        Task SendMailNe(MailRequest mailRequest);
    }
}
