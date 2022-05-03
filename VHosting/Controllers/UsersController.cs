#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VHosting;

namespace VHosting.Controllers
{
    public class UsersController : Controller
    {
        private readonly DBVideoHostingContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(DBVideoHostingContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var id = GetCurrentUserId();
            return RedirectToAction("Details", "Users", new { id = id.Result });
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.AccountTypeNavigation)
                .Include(u => u.Videos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["AccountType"] = new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Nickname,PaymentCard,AccountType,IsVerified,UserName,NormalizedUserName,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountType"] = new SelectList(_context.Accounts, "Id", "Id", user.AccountType);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["AccountType"] = new SelectList(_context.Accounts, "Id", "Id", user.AccountType);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Nickname,PaymentCard,AccountType,IsVerified,UserName,NormalizedUserName,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountType"] = new SelectList(_context.Accounts, "Id", "Id", user.AccountType);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.AccountTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<int?> GetCurrentUserId()
        {
            User usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Playlist newPlaylist;
                                var playlists = (from pl in _context.Playlists
                                         where pl.Name.Contains(worksheet.Name)
                                         select pl).ToList();
                                if (playlists.Count > 0)
                                {
                                    newPlaylist = playlists[0];
                                }
                                else
                                {
                                    newPlaylist = new Playlist();
                                    var info = worksheet.Name.Split("_");
                                    newPlaylist.Name = info[0];
                                    newPlaylist.UserId = int.Parse(info[1]);
                                    _context.Playlists.Add(newPlaylist);
                                }
                                
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Video vid = new Video();
                                        vid.Name = row.Cell(1).Value.ToString();
                                        vid.Description = row.Cell(2).Value.ToString();
                                        vid.Likes = int.Parse(row.Cell(3).Value.ToString());
                                        vid.Dislikes = int.Parse(row.Cell(4).Value.ToString());
                                        vid.UserId = int.Parse(row.Cell(5).Value.ToString());
                                        vid.Link = row.Cell(6).Value.ToString();
                                        vid.Playlists.Add(newPlaylist);


                                        var vidUser = (from user in _context.Users
                                                 where user.Id == vid.UserId
                                                 select user).ToList();
                                        if (vidUser.Count > 0)
                                        {
                                            _context.Videos.Add(vid);
                                        }

                                        
                                    }
                                    catch (Exception e)
                                    {
                                        

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var currentUserId = GetCurrentUserId().Result;
                var myPlaylists = _context.Playlists.Include(x => x.Videos).Where(x => x.UserId == currentUserId).ToList();
                foreach (var pl in myPlaylists)
                {
                    var worksheet = workbook.Worksheets.Add(pl.Name);

                    worksheet.Cell("A1").Value = "Video name";
                    worksheet.Cell("B1").Value = "Video description";
                    worksheet.Cell("C1").Value = "Likes";
                    worksheet.Cell("D1").Value = "Dislikes";
                    worksheet.Cell("E1").Value = "AuthorId";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var videos = pl.Videos.ToList();

                    for (int i = 0; i < videos.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = videos[i].Name;
                        worksheet.Cell(i + 2, 2).Value = videos[i].Description;
                        worksheet.Cell(i + 2, 3).Value = videos[i].Likes;
                        worksheet.Cell(i + 2, 4).Value = videos[i].Dislikes;
                        worksheet.Cell(i + 2, 5).Value = videos[i].UserId;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Playlists_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }


}
