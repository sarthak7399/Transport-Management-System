using Bus_Pass_Mgt_Asp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Pass_Mgt_Asp.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyDBContext db;

        public UsersController(MyDBContext context)
        {
            db = context;
        }
        // GET: UsersController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        
        public IActionResult Login(string Email, string PWD)
        {

            User ExistingUser = db.Users.FirstOrDefault(x => x.Email == Email && x.PWD == PWD);
            if (ExistingUser == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View();
            }


            HttpContext.Session.SetString("UserType", ExistingUser.UserType);
            HttpContext.Session.SetString("UID", ExistingUser.UID.ToString());

            if (ExistingUser.UserType.Equals("Admin")) return RedirectToAction("Index", "Buses");
            return RedirectToAction("Details", "Students");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UID");
            return View("Login");
        }
    }
}
