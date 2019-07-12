using Microsoft.AspNetCore.Mvc;
using NewsRank.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NewsRank.Controllers
{
    public class RankController : Controller
    {
        private readonly NewsDBContext _context;

        public RankController(NewsDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sport()
        {
            // Query sport news.
                var list = _context.TblNews.Where(b => b.NewsType == "Sport")
                .ToList();
                return View(list);
        }

        public IActionResult Ent()
        {
            // Query entertainment news.
            var list = _context.TblNews.Where(b => b.NewsType == "Ent")
                .ToList();
            return View(list);
        }

        /// <summary>
        /// Get detial of news.
        /// </summary>
        /// <param name="id">news id</param>
        /// <returns></returns>
        public IActionResult Detial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var list = _context.TblNews.First(b => b.Id == id);
                return View(list);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}