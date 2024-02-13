using Homies.Data;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Homies.Data.DataConstants;

namespace Homies.Controllers
{
    [Authorize]
    public class Event : Controller
    {
        private readonly HomiesDbContext data;

        public Event(HomiesDbContext dbContext)
        {
            data = dbContext;
        }

        public async Task<IActionResult> All()
        {
            var allEvents = await data.Events
                .Select(e => new EventAllViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organiser.UserName
                })
                .ToListAsync();

            return View(allEvents);
        }

        public async Task<IActionResult> Add()
        {
            var types = await GetTypes();

            var model = new AddEventViewModel()
            {
                Types = types
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newEvent = new Data.Models.Event()
            {
                Name = model.Name,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                CreatedOn = DateTime.Now,
                Type = model.Type,
                TypeId = model.TypeId,
                OrganiserId = GetUserId()
                
            };


            await data.Events.AddAsync(newEvent);
            await data.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await data.Events
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model is null)
            {
                return RedirectToAction("All");
            }

            var types = await GetTypes();

            var edit = new EventEditViewModel()
            {
                Name = model.Name,
                Description = model.Description,
                Start = model.Start.ToString(DateTimeFormat),
                End = model.End.ToString(DateTimeFormat),
                TypeId = model.TypeId,
                Types = types
            };
            if (!ModelState.IsValid)
            {
                return RedirectToAction("All");
            }

            return View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editedEvent = await data.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (editedEvent == null)
            {
                ModelState.AddModelError("", "Wrong Event Id");
            }

            editedEvent.Name = model.Name;
            editedEvent.Description = model.Description;
            editedEvent.Start = DateTime.Parse(model.Start);
            editedEvent.End = DateTime.Parse(model.End);
            editedEvent.TypeId = model.TypeId;

            await data.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var e = await data.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if (e == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!e.EventsParticipants.Any(p => p.HelperId == userId))
            {
                e.EventsParticipants.Add(new EventParticipant()
                {
                    EventId = e.Id,
                    HelperId = userId
                });

                await data.SaveChangesAsync();
            }

            return RedirectToAction("Joined");

        }

        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await data.EventsParticipants
                .Where(ep => ep.HelperId == userId || ep.Event.OrganiserId == userId)
                .AsNoTracking()
                .Select(ep => new EventAllViewModel()
                {
                    Id = ep.Event.Id,
                    Name = ep.Event.Name,
                    Start = ep.Event.Start.ToString(DateTimeFormat),
                    Type = ep.Event.Type.Name,
                    Organiser = ep.Event.Organiser.UserName,
                })
                .ToListAsync();


            return View(model);
        }

        public async Task<IActionResult> Leave(int id)
        {
            var e = await data.EventsParticipants
                .FirstOrDefaultAsync(e => e.EventId == id && e.HelperId == GetUserId());

            if (e == null)
            {
                return BadRequest();
            }

            data.EventsParticipants.Remove(e);
            await data.SaveChangesAsync();

            return RedirectToAction("Joined");
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await data.Events
                .Include(e => e.Organiser) // Include Organiser navigation property
                .FirstOrDefaultAsync(e => e.Id == id);

            if (model == null)
            {
                return BadRequest();
            }

            var type = await data.Types.FirstOrDefaultAsync(t => t.Id == model.TypeId);
            string typeName = type?.Name;

            var e = new DetailsViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                CreatedOn = model.CreatedOn,
                TypeId = model.TypeId,
                Type = typeName,
                Organiser = model.Organiser?.UserName // Access UserName property of Organiser
            };

            return View(e);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<List<TypesViewModel>> GetTypes()
        {
            var types = await data.Types
                .AsNoTracking()
                .Select(t => new TypesViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToListAsync();

            return types;
        }
    }
}
