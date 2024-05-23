using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientApiController(
        ClientManagerDbContext _dbContext) : Controller
    {
        public IActionResult Get()
        {
            var clients = _dbContext.Clients.Include(p => p.City).ToList();
            var clientDTOs = clients
                .Select(client => new ClientDTO
                {
                    ID = client.ID,
                    FullName = client.FullName,
                    Address = client.Address,
                    City = new CityDTO
                    {
                        ID = client.City.ID,
                        Name = client.City.Name
                    },
                    Email = client.Email
                })
                .ToList();

            return Ok(clientDTOs);
        }

        //[Route("api/client/{id}")]
        [HttpGet("{id}")]
        public IActionResult Get(int? id = null)
        {
            var client = _dbContext.Clients
                .Include(p => p.City)
                .Where(c => c.ID == id)
                .FirstOrDefault();

            var clientDTO = new ClientDTO
            {
                ID = client.ID,
                FullName = client.FullName,
                Address = client.Address,
                City = new CityDTO
                {
                    ID = client.City.ID,
                    Name = client.City.Name
                },
                Email = client.Email
            };

            return Ok(clientDTO);

        }

        [HttpGet("pretraga/{q}")]
        public IActionResult Get(string? q)
        {
            var clients = _dbContext.Clients
                .Include(p => p.City)
                .Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(q.ToLower()))
                .ToList();

            var clientDTOs = clients
                .Select(client => new ClientDTO
                {
                    ID = client.ID,
                    FullName = client.FullName,
                    Address = client.Address,
                    City = new CityDTO
                    {
                        ID = client.City.ID,
                        Name = client.City.Name
                    },
                    Email = client.Email
                })
                .ToList();

            return Ok(clientDTOs);
        }

        [HttpPost]
        public IActionResult Create(Client model)
        {
            _dbContext.Clients.Add(model);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = model.ID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Client model)
        {
            var client = _dbContext.Clients.Single(c => c.ID == id);

            client.FirstName = model.FirstName;
            client.LastName = model.LastName;
            client.Email = model.Email;
            client.DateOfBirth = model.DateOfBirth;
            client.WorkingExperience = model.WorkingExperience;
            client.Gender = model.Gender;
            client.Address = model.Address;
            client.PhoneNumber = model.PhoneNumber;
            client.CityID = model.CityID;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Get), new {id = client.ID});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var client = _dbContext.Clients.Find(id);
            _dbContext.Remove(client);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Get));
        }
    }
}
