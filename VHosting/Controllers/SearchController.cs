using Microsoft.AspNetCore.Mvc;

namespace VHosting.Controllers
{
    public class SearchController : Controller
    {
        private readonly DBVideoHostingContext _context;
        public SearchController(DBVideoHostingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string searched)
        {
            var list = new List<object>();

            foreach(var user in _context.Users)
            {
                if (user.Nickname.Contains(searched, System.StringComparison.CurrentCultureIgnoreCase)) list.Add(user);
            }

            foreach (var video in _context.Videos)
            {
                if (video.Name.Contains(searched, System.StringComparison.CurrentCultureIgnoreCase)) list.Add(video);
            }

            return View(list);
        }
    }
}
