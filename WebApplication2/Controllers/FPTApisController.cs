using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

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

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount(fpt_login account)
        {
            try
            {
                if (account.username == null || account.password == null || account.username.Equals("") || account.password.Equals("") || string.IsNullOrEmpty(account.username) || string.IsNullOrEmpty(account.password))
                {
                    return Ok(new Message(0, "Nhập đầy đủ thông tin"));
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new Message(0, "Đăng ký tài khoản không thành công"));
                }

                fpt_login check = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(account.username));
                if (check != null)
                {
                    return Ok(new Message(0, "Username đã được đăng ký"));
                }

                account.positionX = "";
                account.positionY = "";
                account.positionZ = "";
                account.score = 0;
                db.Fpt_Logins.Add(account);
                db.SaveChanges();
                return Ok(new Message(1, "Đăng ký tài khoản thành công"));
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Đăng ký tài khoản không thành công"));
            }
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(fpt_login loginModel)
        {
            try
            {
                if (loginModel.username == null || loginModel.password == null || loginModel.username.Equals("") || loginModel.password.Equals("") || string.IsNullOrEmpty(loginModel.username) || string.IsNullOrEmpty(loginModel.password))
                {
                    return Ok(new Message(0, "Nhập đầy đủ thông tin"));
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username) && p.password.Equals(loginModel.password));

                if (account == null)
                {
                    return Ok(new MessageLoginModel(0, "Đăng nhập không thành công", "", -1, "", "", ""));
                }
                return Ok(new MessageLoginModel(1, "Đăng nhập thành công", account.username, account.score, account.positionX, account.positionY, account.positionZ));
                //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
            }
            catch (Exception e)
            {
                return Ok(new MessageLoginModel(0, "Đăng nhập không thành công", "", -1, "", "", ""));
                //return Ok(new MessageLoginModel(0, e.ToString(), "", -1, null, null, null));
            }

        }

        [HttpPost]
        [Route("save-score")]
        public IActionResult UpdateScore(fpt_login loginModel)
        {
            try
            {
                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username));

                if (account == null)
                {
                    return Ok(new Message(0, "Không tìm thấy tài khoản"));
                }
                else
                {
                    account.score = loginModel.score;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();
                    return Ok(new Message(1, "Lưu score thành công"));
                }
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Lưu score thất bại"));
            }
        }

        [HttpPost]
        [Route("save-position")]
        public IActionResult UpdatePosition(fpt_login loginModel)
        {
            try
            {
                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username));

                if (account == null)
                {
                    return Ok(new Message(0, "Không tìm thấy tài khoản"));
                }
                else
                {
                    account.positionX = loginModel.positionX;
                    account.positionY = loginModel.positionY;
                    account.positionZ = loginModel.positionZ;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();
                    return Ok(new Message(1, "Lưu position thành công"));
                }
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Lưu position thất bại"));
            }
        }

        [HttpPost]
        [Route("change-password")]
        public IActionResult ChangePassword(fpt_changepassword changepassword)
        {
            try
            {
                if (changepassword.username == null || changepassword.oldpassword == null || changepassword.newpassword == null || changepassword.username.Equals("") || changepassword.oldpassword.Equals("") || changepassword.newpassword.Equals("") || string.IsNullOrEmpty(changepassword.username) || string.IsNullOrEmpty(changepassword.oldpassword) || string.IsNullOrEmpty(changepassword.newpassword))
                {
                    return Ok(new Message(0, "Nhập đầy đủ thông tin"));
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(changepassword.username) && p.password.Equals(changepassword.oldpassword));

                if (account == null)
                {
                    return Ok(new Message(0, "Không tìm thấy tài khoản"));
                }
                else
                {
                    account.password = changepassword.newpassword;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();
                    return Ok(new Message(1, "Đổi mật khẩu thành công"));
                }
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Đổi mật khẩu thất bại"));
            }
        }

        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> SendOTPAsync(fpt_sendotp sendotp)
        {
            try
            {
                if (sendotp.username == null || sendotp.username.Equals("") || string.IsNullOrEmpty(sendotp.username))
                {
                    return Ok(new Message(0, "Nhập đầy đủ thông tin"));
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

                    return Ok(new Message(1, "Gửi OTP thành công"));
                }
                else
                {
                    return Ok(new Message(0, "Không tìm thấy tài khoản"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new Message(0, "Gửi OTP thất bại"));
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword(fpt_resetpassword resetpassword)
        {
            try
            {
                if (resetpassword.username == null || resetpassword.newpassword == null || resetpassword.username.Equals("") || resetpassword.otp.Equals("") || resetpassword.newpassword.Equals("") || string.IsNullOrEmpty(resetpassword.username) || string.IsNullOrEmpty(resetpassword.newpassword))
                {
                    return Ok(new Message(0, "Nhập đầy đủ thông tin"));
                }

                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(resetpassword.username) && p.otp.Equals(resetpassword.otp));

                if (account == null)
                {
                    return Ok(new Message(0, "Không tìm thấy tài khoản hoặc nhập mã OTP sai"));
                }
                else
                {
                    account.password = resetpassword.newpassword;
                    account.otp = null;
                    db.Fpt_Logins.Update(account);
                    db.SaveChanges();
                    return Ok(new Message(1, "Reset mật khẩu thành công"));
                }
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Reset mật khẩu thất bại"));
            }
        }
    }
}
