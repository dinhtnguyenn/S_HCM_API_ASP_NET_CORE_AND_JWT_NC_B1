using Microsoft.AspNetCore.Mvc;
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
            if (!ModelState.IsValid)
            {
                return Ok(new Message(0, "Đăng ký tài khoản không thành công. Vui lòng kiểm tra và thử lại"));
            }

            fpt_login check = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(account.username));
            if (check != null)
            {
                return Ok(new Message(2, "MSSV đã được đăng ký. Vui lòng kiểm tra và thử lại"));
            }

            db.Fpt_Logins.Add(account);
            db.SaveChanges();

            return Ok(new Message(1, "Bạn đã đăng ký tài khoản thành công"));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(fpt_login loginModel)
        {
            fpt_login account = db.Fpt_Logins.SingleOrDefault(p => p.username.Equals(loginModel.username) && p.password.Equals(loginModel.password));

            if (account == null)
            {
                return Ok(new DetailLogin(0, "Đăng nhập thất bại. Vui lòng kiểm tra và thử lại", null, null));
            }
            return Ok(new DetailLogin(1, "Đăng nhập thành công", account.username));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }
    }
}
