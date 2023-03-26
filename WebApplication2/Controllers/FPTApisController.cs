using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using System.Text;
using System.Globalization;

namespace WebApplication2.Controllers
{
    [Route("/fpt/")]
    [ApiController]
    public class FPTApisController : ControllerBase
    {
        private readonly dinhntco_studywithmeContext db;
        private readonly IMailService mailService;

        public FPTApisController(dinhntco_studywithmeContext context, IMailService mailService)
        {
            db = context;
            this.mailService = mailService;
        }

        //clear accents
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult DanhSach()
        {
            var list = db.Fpt_Logins.OrderByDescending(p => p.score).ToList();
            List<fpt_LeaderBoard> listLB = new List<fpt_LeaderBoard>();
            foreach (fpt_login item in list)
            {
                listLB.Add(new fpt_LeaderBoard(item.username, item.score));
            }
            return Ok(listLB);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetInfoAll()
        {
            return Ok(db.Fpt_Logins.ToList());
        }

        [HttpGet]
        [Route("details")]
        public IActionResult GetDetailInfo(string id)
        {
            return Ok(db.Fpt_Logins.SingleOrDefault(p=>p.username == id));
        }

        [HttpGet]
        [Route("logs")]
        public IActionResult GetLogs()
        {
            return Ok(db.Fpt_Datalogs.OrderByDescending(p=>p.id).ToList());
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount(fpt_login account)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                if (account.username == null || account.password == null || account.username.Equals("") || account.password.Equals("") || string.IsNullOrEmpty(account.username) || string.IsNullOrEmpty(account.password))
                {
                    Message message = new Message(0, "Nhập đầy đủ thông tin");
                    fpt_Datalog.logData += ">>>Register>>" + DateTime.Now + ">" 
                        + JsonSerializer.Serialize(account) + ">" 
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                if (!ModelState.IsValid)
                {
                    Message message = new Message(0, "Đăng ký tài khoản không thành công");
                    fpt_Datalog.logData += ">>>Register>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                fpt_login check = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(account.username));
                if (check != null)
                {
                    Message message = new Message(0, "Username đã được đăng ký");
                    fpt_Datalog.logData += ">>>Register>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                account.positionX = "";
                account.positionY = "";
                account.positionZ = "";
                account.score = 0;
                
                db.Fpt_Logins.Add(account);
                db.SaveChanges();

                Message message2 = new Message(1, "Đăng ký tài khoản thành công");
                fpt_Datalog.logData += ">>>Register>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(account) + ">"
                    + JsonSerializer.Serialize(message2) + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();

                return Ok(message2);
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Đăng ký tài khoản không thành công");
                fpt_Datalog.logData += ">>>Register>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(account) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();

                return Ok(message);
            }
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(fpt_login loginModel)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                if (loginModel.username == null || loginModel.password == null || loginModel.username.Equals("") || loginModel.password.Equals("") || string.IsNullOrEmpty(loginModel.username) || string.IsNullOrEmpty(loginModel.password))
                {
                    Message message = new Message(0, "Nhập đầy đủ thông tin");
                    fpt_Datalog.logData += ">>>Login>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(message);
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username) && p.password.Equals(loginModel.password));

                if (account == null)
                {
                    MessageLoginModel messageLoginModel = new MessageLoginModel(0, "Đăng nhập không thành công", "", -1, "", "", "");
                    fpt_Datalog.logData += ">>>Login>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(messageLoginModel) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(messageLoginModel);
                }

                MessageLoginModel messageLoginModel2 = new MessageLoginModel(1, "Đăng nhập thành công", account.username, account.score, account.positionX, account.positionY, account.positionZ);
                fpt_Datalog.logData += ">>>Login>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(loginModel) + ">"
                    + JsonSerializer.Serialize(account) + ">"
                    + JsonSerializer.Serialize(messageLoginModel2) + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();

                return Ok(messageLoginModel2);
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                MessageLoginModel message = new MessageLoginModel(0, "Đăng nhập không thành công", "", -1, "", "", "");
                fpt_Datalog.logData += ">>>Login>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(loginModel) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();

                return Ok(message);
            }

        }

        [HttpPost]
        [Route("save-score")]
        public IActionResult UpdateScore(fpt_login loginModel)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username));

                if (account == null)
                {
                    Message message = new Message(0, "Không tìm thấy tài khoản");
                    fpt_Datalog.logData += ">>>SaveScore>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
                else
                {
                    account.score = loginModel.score;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();

                    Message message = new Message(1, "Lưu score thành công");
                    fpt_Datalog.logData += ">>>SaveScore>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Lưu score thất bại");
                fpt_Datalog.logData += ">>>SaveScore>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(loginModel) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();
                return Ok(message);
            }
        }

        [HttpPost]
        [Route("save-position")]
        public IActionResult UpdatePosition(fpt_login loginModel)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username));

                if (account == null)
                {
                    Message message = new Message(0, "Không tìm thấy tài khoản");
                    fpt_Datalog.logData += ">>>SavePosition>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
                else
                {
                    account.positionX = loginModel.positionX;
                    account.positionY = loginModel.positionY;
                    account.positionZ = loginModel.positionZ;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();

                    Message message = new Message(1, "Lưu position thành công");
                    fpt_Datalog.logData += ">>>SavePosition>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(loginModel) + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Lưu position thất bại");
                fpt_Datalog.logData += ">>>SavePosition>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(loginModel) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();
                return Ok(message);
            }
        }

        [HttpPost]
        [Route("change-password")]
        public IActionResult ChangePassword(fpt_changepassword changepassword)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                if (changepassword.username == null || changepassword.oldpassword == null || changepassword.newpassword == null || changepassword.username.Equals("") || changepassword.oldpassword.Equals("") || changepassword.newpassword.Equals("") || string.IsNullOrEmpty(changepassword.username) || string.IsNullOrEmpty(changepassword.oldpassword) || string.IsNullOrEmpty(changepassword.newpassword))
                {
                    Message message = new Message(0, "Nhập đầy đủ thông tin");
                    fpt_Datalog.logData += ">>>ChangePassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(changepassword) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(changepassword.username) && p.password.Equals(changepassword.oldpassword));

                if (account == null)
                {
                    Message message = new Message(0, "Không tìm thấy tài khoản");
                    fpt_Datalog.logData += ">>>ChangePassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(changepassword) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
                else
                {
                    account.password = changepassword.newpassword;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();

                    Message message = new Message(1, "Đổi mật khẩu thành công");
                    fpt_Datalog.logData += ">>>ChangePassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(changepassword) + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Đổi mật khẩu thất bại");
                fpt_Datalog.logData += ">>>ChangePassword>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(changepassword) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();
                return Ok(message);
            }
        }

        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> SendOTPAsync(fpt_sendotp sendotp)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                if (sendotp.username == null || sendotp.username.Equals("") || string.IsNullOrEmpty(sendotp.username))
                {
                    Message message = new Message(0, "Nhập đầy đủ thông tin");
                    fpt_Datalog.logData += ">>>SendOTP>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(sendotp) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(sendotp.username));
                if(account != null)
                {
                    Random random = new Random();
                    var otp = random.Next(1000, 9999);
                    MailRequest request = new MailRequest(sendotp.username, otp);
                    await mailService.SendMailNe(request);


                    account.otp = otp;
                    db.SaveChanges();

                    Message message = new Message(1, "Gửi OTP thành công");
                    fpt_Datalog.logData += ">>>SendOTP>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(sendotp) + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();

                    return Ok(message);
                }
                else
                {
                    Message message = new Message(0, "Không tìm thấy tài khoản");
                    fpt_Datalog.logData += ">>>SendOTP>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(sendotp) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Gửi OTP thất bại");
                fpt_Datalog.logData += ">>>SendOTP>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(sendotp) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();
                return Ok(message);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword(fpt_resetpassword resetpassword)
        {
            try
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();

                if (resetpassword.username == null || resetpassword.newpassword == null || resetpassword.username.Equals("") || resetpassword.otp.Equals("") || resetpassword.newpassword.Equals("") || string.IsNullOrEmpty(resetpassword.username) || string.IsNullOrEmpty(resetpassword.newpassword))
                {
                    Message message = new Message(0, "Nhập đầy đủ thông tin");
                    fpt_Datalog.logData += ">>>ResetPassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(resetpassword) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(resetpassword.username) && p.otp.Equals(resetpassword.otp));

                if (account == null)
                {
                    Message message = new Message(0, "Không tìm thấy tài khoản hoặc nhập mã OTP sai");
                    fpt_Datalog.logData += ">>>ResetPassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(resetpassword) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
                else
                {
                    account.password = resetpassword.newpassword;
                    account.otp = null;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();

                    Message message = new Message(1, "Reset mật khẩu thành công");
                    fpt_Datalog.logData += ">>>ResetPassword>>" + DateTime.Now + ">"
                        + JsonSerializer.Serialize(resetpassword) + ">"
                        + JsonSerializer.Serialize(account) + ">"
                        + JsonSerializer.Serialize(message) + ">>END;";
                    db.Fpt_Datalogs.Add(fpt_Datalog);
                    db.SaveChanges();
                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                fpt_datalog fpt_Datalog = new fpt_datalog();
                Message message = new Message(0, "Reset mật khẩu thất bại");
                fpt_Datalog.logData += ">>>ResetPassword>>" + DateTime.Now + ">"
                    + JsonSerializer.Serialize(resetpassword) + ">"
                    + JsonSerializer.Serialize(message) + ">"
                    + e.ToString() + ">>END;";
                db.Fpt_Datalogs.Add(fpt_Datalog);
                db.SaveChanges();
                return Ok(message);
            }
        }
    }
}
