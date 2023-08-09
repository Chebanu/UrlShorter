using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrlApp.Data;
using UrlShortener.Models;

namespace UrlApp.Controllers;

public class UrlShortsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly XUser _user;

    public UrlShortsController(ApplicationDbContext context, XUser user)
    {
        _context = context;
        _user = user;
    }

    // GET:
    // 
    public async Task<IActionResult> Index()
    {
          return _context.UrlShort != null ? 
                      View(await _context.UrlShort.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.UrlShort'  is null.");
    }

    // GET: UrlShorts/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.UrlShort == null)
        {
            return NotFound();
        }

        var urlShort = await _context.UrlShort
            .FirstOrDefaultAsync(m => m.Id == id);
        if (urlShort == null)
        {
            return NotFound();
        }

        return View(urlShort);
    }

    // GET: UrlShorts/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UrlShorts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OriginalUrl")] UrlShortRequest urlShortRequest)
    {
        UrlShort urlShort = await urlShortRequest.ToUrlShort(_user);

        if (ModelState.IsValid)
        {
            _context.Add(urlShort);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(urlShort);
    }

    // GET: UrlShorts/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.UrlShort == null)
        {
            return NotFound();
        }

        var urlShort = await _context.UrlShort.FindAsync(id);
        if (urlShort == null)
        {
            return NotFound();
        }
        return View(urlShort);
    }

    // POST: UrlShorts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,OriginalUrl,ShortUrl,CreatedDate,CreatedByUserId")] UrlShort urlShort)
    {
        if (id != urlShort.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(urlShort);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlShortExists(urlShort.Id))
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
        return View(urlShort);
    }

    // GET: UrlShorts/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.UrlShort == null)
        {
            return NotFound();
        }

        var urlShort = await _context.UrlShort
            .FirstOrDefaultAsync(m => m.Id == id);
        if (urlShort == null)
        {
            return NotFound();
        }

        return View(urlShort);
    }

    // POST: UrlShorts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.UrlShort == null)
        {
            return Problem("Entity set 'ApplicationDbContext.UrlShort'  is null.");
        }
        var urlShort = await _context.UrlShort.FindAsync(id);
        if (urlShort != null)
        {
            _context.UrlShort.Remove(urlShort);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UrlShortExists(Guid id)
    {
      return (_context.UrlShort?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
