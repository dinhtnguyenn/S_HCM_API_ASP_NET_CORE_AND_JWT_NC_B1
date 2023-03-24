namespace WebApplication2.Models
{
    public class MailRequest
    {
        public MailRequest(string toEmail, int oTP)
        {
            ToEmail = toEmail;
            OTP = oTP;
        }

        public string ToEmail { get; set; }
        public int OTP { get; set; }
    }
}
