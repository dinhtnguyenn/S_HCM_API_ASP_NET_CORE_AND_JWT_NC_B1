using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly dinhntco_studywithmeContext db;

        public AccountsController(dinhntco_studywithmeContext context)
        {
            db = context;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Message(0, "Đăng ký tài khoản không thành công. Vui lòng kiểm tra và thử lại"));
            }

            Account check = db.Accounts.SingleOrDefault(p => p.MSSV.Equals(account.MSSV));
            if (check != null)
            {
                return Ok(new Message(2, "MSSV đã được đăng ký. Vui lòng kiểm tra và thử lại"));
            }

            db.Accounts.Add(account);
            db.SaveChanges();

            return Ok(new Message(1, "Bạn đã đăng ký tài khoản thành công"));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            Account account = db.Accounts.SingleOrDefault(p => p.MSSV.Equals(loginModel.MSSV) && p.Email.Equals(loginModel.Email));

            if (account == null)
            {
                return Ok(new DetailLogin(0, "Đăng nhập thất bại. Vui lòng kiểm tra và thử lại", null, null));
            }
            return Ok(new DetailLogin(1, "Đăng nhập thành công", account.FullName, account.MSSV));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpGet]
        [Route("ranking")]
        public IActionResult GetRanking()
        {
            return Ok(db.Accounts.Where(p => p.Score != null).Select(p => new { p.FullName, p.Score }).OrderByDescending(p => p.Score).ToList());
        }

        [HttpGet]
        [Route("details")]
        public IActionResult GetDetails(string id)
        {
            Account account = db.Accounts.SingleOrDefault(p => p.MSSV.Equals(id));
            if (account == null)
            {
                return Ok(new DetailAccount(0, account));
            }
            return Ok(new DetailAccount(1, account));
        }

        [HttpPost]
        [Route("change-info")]
        public IActionResult ChangeInfo(Account account)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Message(0, "Thay đổi thông tin thất bại. Vui lòng kiểm tra và thử lại"));
            }

            Account accountUpdate = db.Accounts.SingleOrDefault(p => p.MSSV.Equals(account.MSSV));

            if (accountUpdate == null)
            {
                return Ok(new Message(2, "Không tìm thấy người chơi cần thay đổi thông tin. Vui lòng kiểm tra và thử lại"));
            }

            accountUpdate.FullName = account.FullName;
            accountUpdate.Email = account.Email;
            accountUpdate.Phone = account.Phone;
            db.Accounts.Update(accountUpdate);
            db.SaveChanges();

            return Ok(new Message(1, "Thay đổi thông tin thành công"));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        [HttpPost]
        [Route("update-score")]
        public IActionResult UpdateScore(ScoreModel scoreModel)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Message(0, "Cập nhật điểm thất bại. Vui lòng kiểm tra và thử lại"));
            }

            Account accountUpdate = db.Accounts.SingleOrDefault(p => p.MSSV.Equals(scoreModel.MSSV));

            if (accountUpdate == null)
            {
                return Ok(new Message(2, "Không tìm thấy người chơi. Vui lòng kiểm tra và thử lại"));
            }

            accountUpdate.Score = scoreModel.Score;
            db.Accounts.Update(accountUpdate);
            db.SaveChanges();

            return Ok(new Message(1, "Cập nhật điểm thành công"));
            //return CreatedAtRoute("AllAccount", new { id = account.No }, account);
        }

        public JObject parseJsonObject(string data)
        {
            JObject result = JObject.Parse(data);
            return result;
        }

        [HttpGet]
        [Route("quiz")]
        public IActionResult GetQuiz()
        {
            return Ok(parseJsonObject("{\"quiz\":{\"trolls\":{\"q1\":{\"question\":\"Lịch nào dài nhất?\",\"options\":[\"Lịch sử\",\"Lịch lãm\",\"Lịch sự\",\"Lịch vạn niên\"],\"answer\":\"Lịch sử\"},\"q2\":{\"question\":\"Con gì ăn lửa với nước than?\",\"options\":[\"Con quỷ\",\"Con ma\",\"Con tàu\",\"Con người\"],\"answer\":\"Con tàu\"},\"q3\":{\"question\":\"Con gì đập thì sống, không đập thì chết?\",\"options\":[\"Com tim\",\"Con đĩa\",\"Con ngựa\",\"Con tôm\"],\"answer\":\"Con tim\"},\"q4\":{\"question\":\"2 con vịt đi trước 2 con vịt, 2 con vịt đi sau 2 con vịt, 2 con vịt đi giữa 2 con vịt. Hỏi có mấy con vịt?\",\"options\":[\"2\",\"1\",\"0\",\"3\"],\"answer\":\"2\"}},\"maths\":[{\"question\":\"5 + 7 = ?\",\"options\":[\"10\",\"11\",\"12\",\"13\"],\"answer\":\"12\"},{\"question\":\"12 - 8 = ?\",\"options\":[\"1\",\"2\",\"3\",\"4\"],\"answer\":\"4\"},{\"question\":\"7 * 9 = ?\",\"options\":[\"63\",\"64\",\"65\",\"66\"],\"answer\":\"63\"}]}}"));
        }
    }
}
