using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Vjezba.Web.Mock;
using Vjezba.Web.Models;
using Vjezba.Model;
using Vjezba.DAL;
using Microsoft.EntityFrameworkCore;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {

        private ClientManagerDbContext _dbContext;

        public ClientController(ClientManagerDbContext context)
        {
            this._dbContext = context;
        }

        public IActionResult Index(string query = null)
        {
            List<Client> model = this._dbContext.Clients.
                                 Include(p => p.City).
                                 ToList();

            if (!string.IsNullOrWhiteSpace(query))
                model = model.Where(p => p.FullName.ToLower().Contains(query)).ToList();

            ViewBag.ActiveTab = 1;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string queryName, string queryAddress)
        {
            List<Client> model = this._dbContext.Clients.
                     Include(p => p.City).
                     ToList();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(queryName))
                model = model.Where(p => p.FullName.ToLower().Contains(queryName)).ToList();

            if (!string.IsNullOrWhiteSpace(queryAddress))
                model = model.Where(p => p.Address.ToLower().Contains(queryAddress)).ToList();

            ViewBag.ActiveTab = 2;
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClientFilterModel filter)
        {

            List<Client> model = this._dbContext.Clients.
                     Include(p => p.City).
                     ToList();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(filter.FullName))
                model = model.Where(p => p.FullName.ToLower().Contains(filter.FullName)).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Address))
                model = model.Where(p => p.Address.ToLower().Contains(filter.Address.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Email))
                model = model.Where(p => p.Email.ToLower().Contains(filter.Email.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.City))
                model = model.Where(p => p.City != null && p.City.Name.ToLower().Contains(filter.City.ToLower())).ToList();

            ViewBag.ActiveTab = 4;

            return View("Index", model);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Client unos)
        {
            Random random = new Random();
            unos.CityID = random.Next(1, 4);
            this._dbContext.Clients.Add(unos);
            this._dbContext.SaveChanges();

            return View();
        }

        public IActionResult Details(int? id = null)
        {
            Client result = this._dbContext.Clients.
                            Include(p => p.City).
                            FirstOrDefault(p => p.ID == id);
            return View(result);
        }
    }
}
