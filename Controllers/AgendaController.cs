using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TASM.Data;
using TASM.Models;

namespace TASM.Controllers
{
    public class AgendaController : Controller
    {
        private readonly TamsContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AgendaController(TamsContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Agenda
        public async Task<IActionResult> Index()
        {
            var tamsContext = _context.Agendas.Include(a => a.Lab).Include(a => a.Ta);
            var username = _userManager.GetUserName(User);

            var ta = await _context.Tas
                                .Include(t => t.Lab)
                                .FirstOrDefaultAsync(t => t.Name == username);
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            return View(await tamsContext.ToListAsync());
        }

        // GET: Agenda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var username = _userManager.GetUserName(User);

            var ta = await _context.Tas
                                .Include(t => t.Lab)
                                .FirstOrDefaultAsync(t => t.Name == username);
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            var agenda = await _context.Agendas
                .Include(a => a.Lab)
                .Include(a => a.Ta)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewData["Resources"] = agenda.Resources.Split(',').ToList();
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // GET: Agenda/Create
        public IActionResult Create()
        {
            var username = _userManager.GetUserName(User);
            var ta = _context.Tas
                    .Include(t => t.Lab)
                    .FirstOrDefault(t => t.Name == username);
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            ViewData["TaId"] = _context.Tas.Where(T=>T.Name == username).Select(T=>T.Id).FirstOrDefault();
            return View();
        }

        // POST: Agenda/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LabId,TaId,TopicName,Resources")] Agenda agenda)
        {
            var username = _userManager.GetUserName(User);

            var ta = await _context.Tas
                            .Include(t => t.Lab)
                            .FirstOrDefaultAsync(t => t.Name == username);
            if (ModelState.IsValid)
            {
                agenda.TaId = ta.Id;
                agenda.LabId = ta.LabId;
                _context.Add(agenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            ViewData["TaId"] = await _context.Tas.Where(T => T.Name == username).Select(T => T.Id).FirstOrDefaultAsync();
            return View(agenda);
        }

        // GET: Agenda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }
            var username = _userManager.GetUserName(User);
            var ta = await _context.Tas
                            .Include(t => t.Lab)
                            .FirstOrDefaultAsync(t => t.Name == username);
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            ViewData["TaId"] = await _context.Tas.Where(T => T.Name == username).Select(T => T.Id).FirstOrDefaultAsync();
            return View(agenda);
        }

        // POST: Agenda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabId,TaId,TopicName,Resources")] Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return NotFound();
            }
            var username = _userManager.GetUserName(User);
            var ta = await _context.Tas
                            .Include(t => t.Lab)
                            .FirstOrDefaultAsync(t => t.Name == username);

            if (ModelState.IsValid)
            {
                try
                {
                    agenda.TaId = ta.Id;
                    agenda.LabId = ta.LabId;
                    _context.Update(agenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendaExists(agenda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            ViewData["TaId"] = await _context.Tas.Where(T => T.Name == username).Select(T => T.Id).FirstOrDefaultAsync();
            return View(agenda);
        }

        // GET: Agenda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas
                .Include(a => a.Lab)
                .Include(a => a.Ta)
                .FirstOrDefaultAsync(m => m.Id == id);
            var username = _userManager.GetUserName(User);
            var ta = await _context.Tas
                            .Include(t => t.Lab)
                            .FirstOrDefaultAsync(t => t.Name == username);
            if (ta != null)
            {
                var labId = ta.LabId;
                ViewData["LabName"] = ta.Lab?.Name;
            }
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // POST: Agenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda != null)
            {
                _context.Agendas.Remove(agenda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendaExists(int id)
        {
            return _context.Agendas.Any(e => e.Id == id);
        }
    }
}
