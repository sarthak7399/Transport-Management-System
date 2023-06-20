using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bus_Pass_Mgt_Asp;
using Bus_Pass_Mgt_Asp.Models;

namespace Bus_Pass_Mgt_Asp.Controllers
{
    public class BusesController : Controller
    {
        private readonly MyDBContext _context;

        public BusesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Buses
        public async Task<IActionResult> Index()
        {

            var Buses = await GetAllBusses();
            return View(Buses);
        }
        public async Task<IActionResult> GetBusses()
        {

            var Buses = await GetAllBusses();
            return Ok(Buses);
        }
        public async Task<List<Bus>> GetAllBusses()
        {
            var Buses = await _context.Buses.ToListAsync();
            var Locations = await _context.Locations.ToListAsync();
            foreach (var bus in Buses)
            {
                var location = Locations.Where(m => m.BID == bus.BID).FirstOrDefault();
                if (location == null)
                    continue;
                bus.MapLink = "http://maps.google.com/maps?&z=20&q=" + location.Longitude + "+" + location.Latitude + "&ll=" + location.Longitude + "+" + location.Latitude;
                
            }

            return (Buses);
        }
        //public static async Task<Bus> GetBus(int id)
        //{
        //    var Bus = await _context.Buses.SingleOrDefaultAsync(x => x.BID == id);
        //    var location = await _context.Locations.Where(m => m.BID == Bus.BID).FirstOrDefaultAsync();
        //    if (location != null)

        //        Bus.MapLink = "http://maps.google.com/maps?&z=20&q=" + location.Longitude + "+" + location.Latitude + "&ll=" + location.Longitude + "+" + location.Latitude;

        //    return (Bus);
        //}

        public async Task<JsonResult> GetAllBussesAsJson() { return  Json(GetAllBusses()); }

        // GET: Buses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses
                .FirstOrDefaultAsync(m => m.BID == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // GET: Buses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(  Bus bus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        // GET: Buses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        // POST: Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Bus bus)
        {
            if (id != bus.BID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(bus.BID))
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
            return View(bus);
        }

        // GET: Buses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses
                .FirstOrDefaultAsync(m => m.BID == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         public IActionResult AssignedStudents(int id)
        {
            var students = _context.Students.Where(x => x.BID == id);
            
            return View(students);
        }

        private bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.BID == id);
        }
        public async Task<IActionResult> ViewLocation(int id=1)
        {
           

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.BID == id);
            if (location == null)
            {
                return NotFound();
            }
            ViewBag.MapLink = "http://maps.google.com/maps?&z=20&q=" + location.Longitude + "+" + location.Latitude + "&ll=" + location.Longitude + "+" + location.Latitude;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DriverLogin([FromBody]Bus bus)
        {
            

            var Bus = await _context.Buses.FirstOrDefaultAsync(m => m.BusNumber == bus.BusNumber && m.DriverMobile== bus.DriverMobile);
            


            return Ok(Bus);
        }

    }
}
