using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly CvManager _cvManager;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _cvManager = new CvManager("cv/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadPage()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> Upload(UploadVM uploadVM)
        {
            // Check if the provided file is valid
            if (uploadVM.File == null)
                throw new NotImplementedException("No file was provided");

            if (!uploadVM.File.FileName.EndsWith(".pdf"))
                throw new NotImplementedException("The provided file was not a .pdf file");

            // Store the file
            Guid id = _cvManager.GenerateNewId();
            var filePath = _cvManager.GetFilePath(id);
            using (var fileSteam = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, filePath), FileMode.Create))
                await uploadVM.File.CopyToAsync(fileSteam);

            // TODO: Remove previous cv

            return View(
                new PreviewVM
                {
                    URL = new List<string>
                    {
                        $"\\{filePath}",
                        $"\\{_cvManager.GetFilePath(Guid.Parse("86451e00-00df-496d-af11-1e39ad149144"))}",
                        $"\\{_cvManager.GetFilePath(Guid.Parse("0bc76ddd-0184-41a8-b0a4-12b1b51300dc"))}"
                    }
                }
            );
        }
    }
}
