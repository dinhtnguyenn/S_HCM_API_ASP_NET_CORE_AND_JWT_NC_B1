using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("/fpt/")]
    [ApiController]
    public class FPTApisController : ControllerBase
    {
        private readonly dinhntco_studywithmeContext db;

        public FPTApisController(dinhntco_studywithmeContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult DanhSach()
        {
            return Ok(db.Fpt_Logins.ToList());
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount(fpt_login account)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new Message(0, "Đăng ký tài khoản không thành công"));
                }

                fpt_login check = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(account.username));
                if (check != null)
                {
                    return Ok(new Message(0, "Username đã được đăng ký"));
                }

                db.Fpt_Logins.Add(account);
                db.SaveChanges();
                return Ok(new Message(1, "Đăng ký tài khoản thành công"));
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Đăng ký tài khoản không thành công - catch:err"));
            }
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(fpt_login loginModel)
        {
            try
            {
                fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username) && p.password.Equals(loginModel.password));

                if (account == null)
                {
                    return Ok(new DetailLogin(0, "Đăng nhập không thành công", null, null));
                }
                return Ok(new DetailLogin(1, "Đăng nhập thành công", account.username));
                //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
            }
            catch (Exception e)
            {
                return Ok(new Message(0, "Đăng nhập không thành công - catch:err"));
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
                    return Ok(new Message(0, "Không tìm thấy tài khoản - catch: null"));
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
                return Ok(new Message(0, "Lưu score thất bại - catch:err"));
            }
        }
    }
}
