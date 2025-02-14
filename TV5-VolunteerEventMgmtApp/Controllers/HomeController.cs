using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly VolunteerEventMgmtAppDbContext _context;
		private readonly ILogger<HomeController> _logger;

		public HomeController(VolunteerEventMgmtAppDbContext context, ILogger<HomeController> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var homeImage = await _context.HomeImages.FirstOrDefaultAsync();
			return View(homeImage);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateImage(IFormFile imageFile, string welcomeMessage, string buttonText)
		{
			var homeImage = await _context.HomeImages.FirstOrDefaultAsync();
			if (homeImage == null)
			{
				homeImage = new HomeImage();
				_context.HomeImages.Add(homeImage);
			}

			if (imageFile != null && imageFile.Length > 0)
			{
				using var memoryStream = new MemoryStream();
				await imageFile.CopyToAsync(memoryStream);
				homeImage.Content = memoryStream.ToArray();
				homeImage.MimeType = imageFile.ContentType;
			}

			homeImage.WelcomeMessage = welcomeMessage;
			homeImage.ButtonText = buttonText;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
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
