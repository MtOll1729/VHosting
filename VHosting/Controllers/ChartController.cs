using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBVideoHostingContext _context;
        private readonly UserManager<User> _userManager;
        public ChartController(DBVideoHostingContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var videos = _context.Videos
                .Include(x => x.WatchedVideos)
                .Where(x => x.UserId == GetCurrentUserId().Result).ToList();
            List<object> videosInfo = new List<object>();

            videosInfo.Add(new[] {"Video", "Watched times"});
            foreach(var video in videos)
            {
                videosInfo.Add(new object[] { video.Name, video.WatchedVideos.Count });
            }

            return new JsonResult(videosInfo);
        }

        [HttpGet]
        public async Task<int?> GetCurrentUserId()
        {
            User usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
