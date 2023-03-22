using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var list = db.Fpt_Logins.OrderByDescending(p => p.score).ToList();
            List<fpt_LeaderBoard> listLB = new List<fpt_LeaderBoard>();
            foreach(fpt_login item in list)
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
                    return Ok(new MessageLoginModel(0, "Đăng nhập không thành công", "", -1, null, null, null));
                }
                return Ok(new MessageLoginModel(1, "Đăng nhập thành công", account.username, account.score, account.positionX, account.positionY, account.positionZ));
                //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
            }
            catch (Exception e)
            {
                return Ok(new MessageLoginModel(0, "Đăng nhập không thành công - catch:err","", -1, null, null, null));
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

        [HttpPost]
        [Route("save-position")]
        public IActionResult UpdatePosition(fpt_login loginModel)
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
                return Ok(new Message(0, "Lưu position thất bại - catch:err"));
            }
        }
    }
}
