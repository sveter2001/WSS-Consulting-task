using HierarchicalCatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchicalCatalog.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationContext _context;
        private SerializerToXml _Serializer;

        public FilesController(ApplicationContext context)
        {
            _context = context;
            _Serializer = new SerializerToXml();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ItemsToXml()
        {
            List<MenuItem> menuItems = await _context.MenuItems.ToListAsync();
            using (var memoryStream = new MemoryStream())
            {
                _Serializer.SerializeToXml(menuItems, memoryStream);
                memoryStream.Position = 0;
                var contentType = "application/xml";

                return File(memoryStream.ToArray(), contentType, "example.xml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                List<MenuItem> list = new List<MenuItem>();
                using (var memoryStream = new MemoryStream())
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    list = _Serializer.DeserializeFromXml(memoryStream);
                }
                _context.MenuItems.RemoveRange(_context.MenuItems);
                await _context.MenuItems.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}