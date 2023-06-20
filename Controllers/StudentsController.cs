using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bus_Pass_Mgt_Asp;
using Bus_Pass_Mgt_Asp.Models;

using Microsoft.AspNetCore.Http;
using System.IO;

namespace Bus_Pass_Mgt_Asp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MyDBContext _context;

        public StudentsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.SID == id);
            if (student == null)
            {
                return NotFound();
            }

            var Bus = await _context.Buses.SingleOrDefaultAsync(x => x.BID == id);
            var location = await _context.Locations.Where(m => m.BID == Bus.BID).FirstOrDefaultAsync();
            if (location != null)

                Bus.MapLink = "http://maps.google.com/maps?&z=20&q=" + location.Longitude + "+" + location.Latitude + "&ll=" + location.Longitude + "+" + location.Latitude;
            student.Bus = Bus;
            
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            var Buses =  _context.Buses.ToList();
            ViewBag.Buses = Buses;
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Student student)
        {
            student.PWD = student.Mobile;


            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var ID = student.SID;
                    if (file == null || file.Length == 0) return Content("file not selected");

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Students\\" + ID + ".jpg");

                    using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        file.CopyTo(stream);
                        stream.Close();
                        stream.Dispose();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            var Buses = await _context.Buses.ToListAsync();

            ViewBag.Buses = Buses;
            ViewBag.BID = student.BID;


            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var item in Buses)
            {
                selectListItems.Add(new SelectListItem { Selected = student.BID == item.BID, Text = item.BusNumber, Value = item.BID.ToString() });
            }

            var list = new SelectList(selectListItems.ToArray());

            ViewBag.SelectList = selectListItems;



            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Student student)
        {
            if (id != student.SID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        var ID = student.SID;
                        if (file == null || file.Length == 0) return Content("file not selected");

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Students\\" + ID + ".jpg");

                        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            file.CopyTo(stream);
                            stream.Close();
                            stream.Dispose();
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.SID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.SID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.SID == id);
        }
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]

        public IActionResult Login(string Email, string PWD)
        {

            Student ExistingUser = _context.Students.FirstOrDefault(x => x.Email == Email && x.PWD == PWD);
            if (ExistingUser == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View();
            }


            HttpContext.Session.SetString("UserType", "Student");
            HttpContext.Session.SetString("SID", ExistingUser.SID.ToString());

            return RedirectToAction("Details", "Students",new { id = ExistingUser.SID.ToString() });

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("SID");
            return View("Login");
        }
        public IActionResult PasswordChange()
        {
            try
            {
                ViewBag.UID = HttpContext.Session.GetString("SID");
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Students");
                throw;
            }

        }
        [HttpPost]
        public IActionResult PasswordChange(PasswordChange passwordChange)
        {

            if (passwordChange.NewPassword != passwordChange.RePassword)
            {
                ViewBag.Error = "New & confirm Password do not match.";
                return View();
            }
            string ExistingPassword = _context.Students.Where(x => x.SID == passwordChange.UID).Select(x => x.PWD).FirstOrDefault();

            if (!ExistingPassword.Equals(passwordChange.OldPassword)) { ViewBag.Message = "Invalid Old Password."; return View(); }
            if (ExistingPassword.Equals(passwordChange.NewPassword)) { ViewBag.Message = "No changes has been made."; return View(); }

            var user = _context.Students.FirstOrDefault(x => x.SID == passwordChange.UID);
            user.PWD = passwordChange.NewPassword;
            _context.SaveChanges();

            ViewBag.Message = "Password Changed Successfully.";

            return View();
        }

        public async Task<IActionResult> ViewCard(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.SID == id);
            if (student == null)
            {
                return NotFound();
            }

            var Bus = await _context.Buses.SingleOrDefaultAsync(x => x.BID == id);
            var location = await _context.Locations.Where(m => m.BID == Bus.BID).FirstOrDefaultAsync();
            if (location != null)

                Bus.MapLink = "http://maps.google.com/maps?&z=20&q=" + location.Longitude + "+" + location.Latitude + "&ll=" + location.Longitude + "+" + location.Latitude;
            student.Bus = Bus;

            
            ViewBag.user = student;
            return View();
        }
    }
}
