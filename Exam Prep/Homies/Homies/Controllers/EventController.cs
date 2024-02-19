using System.Security.Claims;
using Homies.Data;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static Homies.Data.DataConstants;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext dbContext;

        public EventController(HomiesDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<IActionResult> All()
        {
            var events = await dbContext.Events
                .AsNoTracking()
                .Select(e => new AllEventsViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organiser.UserName,
                }).ToListAsync();

            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var types = await GetAllTypes();

            var form = new AddEventViewModel()
            {
                Types = types,
            };

            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Types = await GetAllTypes();

                return View(model);
            }

            var e = new Event()
            {
                Name = model.Name,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                CreatedOn = DateTime.Now,
                TypeId = model.TypeId,
                OrganiserId = GetUserId()
            };

            await dbContext.Events.AddAsync(e);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var userId = GetUserId();

            var ep = new EventParticipant()
            {
                EventId = id,
                HelperId = userId
            };

            if (dbContext.EventsParticipants.Any(ep => ep.EventId == id && ep.HelperId == userId))
            {
                return RedirectToAction("All");
            }

            await dbContext.EventsParticipants.AddAsync(ep);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Joined");
        }
        
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var events = await dbContext.Events
                .AsNoTracking()
                .Include(e => e.EventsParticipants)
                .Where(e => e.EventsParticipants.Any(ep => ep.EventId == e.Id && ep.HelperId == GetUserId()))
                .Select(e => new JoinedEventsViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organiser.UserName,

                }).ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Leave(int id)
        {
            var userId = GetUserId();

            var e = await dbContext.EventsParticipants
                .Where(ep => ep.EventId == id && ep.HelperId == userId)
                .FirstOrDefaultAsync();

            if (e == null)
            {
                return RedirectToAction("All");
            }

            dbContext.EventsParticipants.Remove(e);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var edit = await dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (edit == null)
            {
                ModelState.AddModelError(string.Empty, "Wrong event Id");
                return RedirectToAction("All");
            }

            var model = new EditViewModel()
            {
                Id = edit.Id,
                Name = edit.Name,
                Description = edit.Description,
                Start = edit.Start,
                End = edit.End,
                TypeId = edit.TypeId,
                Types = await GetAllTypes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Types = await GetAllTypes();
                return View(model);
            }

            var e = await dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            e.Name = model.Name;
            e.Description = model.Description;
            e.Start = model.Start;
            e.End = model.End;
            e.TypeId = model.TypeId;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Details(int id)
        {
            var e = await dbContext.Events
                .Include(e => e.Organiser)
                .Include(e => e.Type)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (e == null)
            {
                return RedirectToAction("All");
            }

            var model = new DetailsViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Start = e.Start.ToString(DateTimeFormat),
                End = e.End.ToString(DateTimeFormat),
                Organiser = e.Organiser.UserName,
                CreatedOn = e.CreatedOn.ToString(DateTimeFormat),
                Type = e.Type.Name,
            };

            return View(model);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<List<TypeViewModel>> GetAllTypes()
        {
            var types = await dbContext.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToListAsync();

            return types;
        }
    }
}
