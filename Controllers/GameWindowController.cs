using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TictactoeWebApp.Controllers
{
    public class GameWindowController : Controller
    {
        public IActionResult PlayGame()
        {
            return View();
        }

        [Authorize]
        public IActionResult PlayerDetails()
        {
            return View();
        }
    }
}
