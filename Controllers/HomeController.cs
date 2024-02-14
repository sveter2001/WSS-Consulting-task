using HierarchicalCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchicalCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<MenuItem> menuItems = await _context.MenuItems.ToListAsync();

            return View(menuItems);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int itemId)
        {
            var item = await _context.MenuItems.FindAsync(itemId);

            if (item != null)
            {
                await DeleteRec(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Не найдена запись на удаление");
                return RedirectToAction("Error");
            }
        }

        private async Task DeleteRec(MenuItem item)//функция рекурсивного удаления child элементов
        {
            var childItems = await _context.MenuItems.Where(m => m.ParentId == item.Id).ToListAsync();
            foreach (var child in childItems)
            {
                await DeleteRec(child);
            }
            _context.MenuItems.Remove(item);
        }

        [HttpPost]
        public async Task<IActionResult> Move(MenuItem item, int itemId)
        {
            var olditem = await _context.MenuItems.FindAsync(itemId);
            var itemMoveIn = await _context.MenuItems.Where(x => x.Id == item.ParentId).AnyAsync();
            if (olditem != null && itemMoveIn)
            {
                olditem.ParentId = item.ParentId;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else if (item.ParentId == 0)//Перенос в корень
            {
                olditem.ParentId = null;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Не найдена запись на перемещение или целевая запись");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(MenuItem item, int itemId)
        {
            var newitem = new MenuItem();
            try
            {
                if (await _context.MenuItems.Where(x => x.ParentId == itemId).CountAsync() > 0)
                {
                    newitem.Id = (await _context.MenuItems.OrderBy(x => x.Id).LastOrDefaultAsync()).Id + 1;
                    newitem.Header = item.Header;
                    newitem.Order = itemId == 0 ? (await _context.MenuItems.Where(x => x.ParentId == null)
                        .OrderBy(x => x.Order).LastOrDefaultAsync()).Order + 1
                        : (await _context.MenuItems.Where(x => x.ParentId == itemId)
                        .OrderBy(x => x.Order).LastOrDefaultAsync()).Order + 1;
                    newitem.ParentId = itemId == 0 ? null : itemId;
                    await _context.MenuItems.AddAsync(newitem);
                }
                else
                {
                    newitem.Id = await _context.MenuItems.Where(x => x.ParentId == null).CountAsync() == 0 ?
                        1 : (await _context.MenuItems.OrderBy(x => x.Id).LastOrDefaultAsync()).Id + 1;
                    newitem.Header = item.Header;
                    newitem.Order = 1;
                    newitem.ParentId = itemId == 0 ? null : itemId;
                    await _context.MenuItems.AddAsync(newitem);
                }
            }
            catch
            {
                Console.WriteLine("Ошибка добавления записи");
                return RedirectToAction("Error");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}