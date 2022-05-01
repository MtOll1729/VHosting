using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBVideoHostingContext _context;
        public ChartController(DBVideoHostingContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var users = _context.Users.Include(x => x.Subsribers).ToList();
            List<object> usersInfo = new List<object>();

            usersInfo.Add(new[] {"User", "Videos"});
            foreach(var user in users)
            {
                usersInfo.Add(new object[] { user.Nickname, user.Subsribers.Count });
            }

            return new JsonResult(usersInfo);
        }
    }
}
