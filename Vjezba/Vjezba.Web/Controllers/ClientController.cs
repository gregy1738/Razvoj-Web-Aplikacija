using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
	public class ClientController(
        ClientManagerDbContext _dbContext) : Controller
    {
        public IActionResult Index()
        {
			var clients = _dbContext.Clients.Include(p => p.City).ToList();
			return View(clients);
        }

		[HttpPost]
		public IActionResult IndexAjax(ClientFilterModel filter = null)
		{
			filter ??= new ClientFilterModel();

			var clientQuery = _dbContext.Clients.Include(p => p.City).AsQueryable();

			//Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
			//To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
			if (!string.IsNullOrWhiteSpace(filter.FullName))
				clientQuery = clientQuery.Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(filter.FullName.ToLower()));

			if (!string.IsNullOrWhiteSpace(filter.Address))
				clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(filter.Address.ToLower()));

			if (!string.IsNullOrWhiteSpace(filter.Email))
				clientQuery = clientQuery.Where(p => p.Email.ToLower().Contains(filter.Email.ToLower()));

			if (!string.IsNullOrWhiteSpace(filter.City))
				clientQuery = clientQuery.Where(p => p.CityID != null && p.City.Name.ToLower().Contains(filter.City.ToLower()));

			var model = clientQuery.ToList();
			return PartialView("_IndexTable", model);
		}

        public IActionResult Details(int? id = null)
        {
			var client = _dbContext.Clients
				.Include(p => p.City)
				.Where(p => p.ID == id)
				.FirstOrDefault();

			return View(client);
		}

		public IActionResult Create()
		{
			this.FillDropdownValues();
			return View();
		}

		[HttpPost]
		public IActionResult Create(Client model)
		{
			if (ModelState.IsValid)
			{
				_dbContext.Clients.Add(model);
				_dbContext.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			else
			{
				this.FillDropdownValues();
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> UploadAttachment(IFormFile file, int clientId)
		{
			if (file == null)
				return BadRequest("No file uploaded.");

			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			var fileName = Path.GetFileName(file.FileName);
			var filePath = Path.Combine(uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var attachment = new Attachment
			{
				FileName = fileName,
				FilePath = filePath,
				UploadDate = DateTime.Now,
				ClientID = clientId
			};

			_dbContext.Attachments.Add(attachment);
			await _dbContext.SaveChangesAsync();

			return Ok(new { message = "File successfully uploaded." });
		}

		[HttpGet]
		public IActionResult GetAttachments(int clientId)
		{
			var attachments = _dbContext.Attachments.Where(p => p.ClientID == clientId).ToList();

            return PartialView("_AttachmentList", attachments);
		}

		[HttpPost]
		public IActionResult DeleteAttachment(int id)
		{
			var attachment = _dbContext.Attachments.FirstOrDefault(p=> p.Id == id);

			string fullpath = attachment.FilePath;
			if (System.IO.File.Exists(fullpath))
			{
				System.IO.File.Delete(fullpath);
			}

			if(attachment != null)
			{
                _dbContext.Attachments.Remove(attachment);
                _dbContext.SaveChanges();
            }

			return RedirectToAction(nameof(Index));
		}


		[ActionName(nameof(Edit))]
		public IActionResult Edit(int id)
		{
			var model = _dbContext.Clients.FirstOrDefault(c => c.ID == id);
			this.FillDropdownValues();
			return View(model);
		}

		[HttpPost]
		[ActionName(nameof(Edit))]
		public async Task<IActionResult> EditPost(int id)
		{
			var client = _dbContext.Clients.Single(c => c.ID == id);
			var ok = await this.TryUpdateModelAsync(client);

			if (ok && this.ModelState.IsValid)
			{
				_dbContext.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			this.FillDropdownValues();
			return View();
		}

		private void FillDropdownValues()
		{
			var selectItems = new List<SelectListItem>();

			//Polje je opcionalno
			var listItem = new SelectListItem();
			listItem.Text = "- odaberite -";
			listItem.Value = "";
			selectItems.Add(listItem);

			foreach (var category in _dbContext.Cities)
			{
				listItem = new SelectListItem(category.Name, category.ID.ToString());
				selectItems.Add(listItem);
			}

			ViewBag.PossibleCities = selectItems;
		}
	}
}
