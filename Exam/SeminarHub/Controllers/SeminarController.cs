using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext dbContext;

        public SeminarController(SeminarHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await dbContext.Seminars
                .AsNoTracking()
                .Select(s => new AllSeminarsViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime,
                    Organizer = s.Organizer.UserName,
                })
                .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await GetAllCategories();

            var model = new AddSeminarViewModel()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSeminarViewModel model)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                model.Categories = await GetAllCategories();
                return View(model);
            }

            var seminar = new Seminar()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                DateAndTime = model.DateAndTime,
                Duration = model.Duration,
                CategoryId = model.CategoryId,
                OrganizerId = userId
            };

            await dbContext.Seminars.AddAsync(seminar);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seminar = await dbContext.Seminars
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seminar == null)
            {
                return BadRequest();
            }

            if (seminar.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var categories = await GetAllCategories();

            var model = new EditSeminarViewModel()
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime,
                Duration = seminar.Duration,
                Categories = categories,
                CategoryId = seminar.CategoryId,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditSeminarViewModel model)
        {
            var userId = GetUserId();
            
            var seminar = await dbContext.Seminars
                .FirstOrDefaultAsync(s => s.Id == id);

            
            if (seminar == null)
            {
                return BadRequest();
            }

            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetAllCategories();
                return View(model);
            }

            seminar.Topic = model.Topic;
            seminar.Lecturer = model.Lecturer;
            seminar.Details = model.Details;
            seminar.DateAndTime = model.DateAndTime;
            seminar.Duration = model.Duration;
            seminar.CategoryId = model.CategoryId;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var userId = GetUserId();

            var seminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            var participantAlreadyJoined = seminar.SeminarsParticipants
                .Any(sp => sp.ParticipantId == userId);

            if (participantAlreadyJoined)
            {
                return RedirectToAction("All");
            }

            if (!participantAlreadyJoined)
            {
                seminar.SeminarsParticipants.Add(new SeminarParticipant()
                {
                    ParticipantId = userId,
                    SeminarId = seminar.Id
                });

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Joined");
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var userId = GetUserId();

            var seminars = await dbContext.SeminarsParticipants
                .AsNoTracking()
                .Where(sp => sp.ParticipantId == userId)
                .Select(s => new JoinedSeminarsViewModel()
                {
                    Id = s.SeminarId,
                    Topic = s.Seminar.Topic,
                    Lecturer = s.Seminar.Lecturer,
                    DateAndTime = s.Seminar.DateAndTime,
                    Organizer = s.Seminar.Organizer.UserName,
                })
                .ToListAsync();

            return View(seminars);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = GetUserId();

            var seminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            var sp = seminar.SeminarsParticipants
                .FirstOrDefault(sp => sp.ParticipantId == userId);

            if (sp == null)
            {
                return BadRequest();
            }

            dbContext.SeminarsParticipants.Remove(sp);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Joined");
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await dbContext.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new DetailsViewModel()
                {
                    Id = s.Id,
                    DateAndTime = s.DateAndTime.ToString(DataConstants.DateTimeFormat),
                    Duration = s.Duration,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    Details = s.Details,
                    Organizer = s.Organizer.UserName,
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var seminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .Select(s => new DeleteViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime
                })
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            var sp = await dbContext.SeminarsParticipants
                .Where(sp => sp.SeminarId == id)
                .FirstOrDefaultAsync();

            dbContext.SeminarsParticipants.RemoveRange(seminar.SeminarsParticipants);
            
            dbContext.Seminars.Remove(seminar);
            
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        /// <summary>
        /// Get the current User's Id
        /// </summary>
        /// <returns></returns>
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        /// <summary>
        /// Get all categories for the dropdown menu
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoriesViewModel>> GetAllCategories()
        {
            var categories = await dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoriesViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return categories;
        }
    }
}
