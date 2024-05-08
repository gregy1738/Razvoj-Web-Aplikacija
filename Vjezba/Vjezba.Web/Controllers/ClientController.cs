using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class ClientController(
        ClientManagerDbContext _dbContext) : Controller
    {
        public IActionResult Index(ClientFilterModel filter = null)
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
            return View(model);
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
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                Random rand = new Random();
                model.CityID = rand.Next(1, 4);
                _dbContext.Clients.Add(model);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(p => p.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var client = _dbContext.Clients.First(p => p.ID == id);
            var ok = await this.TryUpdateModelAsync(client);

            if (ok)
            {
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Pregledajte ModelState kako biste vidjeli informacije o grešci
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        // Ovdje možete prilagoditi kako prikazujete ili obrađujete greške
                        // Na primjer, možete ih dodati u neku listu kako biste ih prikazali u pogledu
                        Console.WriteLine("Errori: ");
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

                return View();
            }
        }
    }
}
