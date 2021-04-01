using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TictactoeWebApp.Models;

namespace TictactoeWebApp.Controllers
{
    public class FormController : Controller
    {
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(RegisterViewModel regDetails)
        {

            return Ok(regDetails);
        }
    }
}
