using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TictactoeWebApp.Models;

namespace TictactoeWebApp.Controllers
{
    public class FormController : Controller
    {
        
        private readonly MySqlConnection mySqlConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormController(MySqlConnection conn, IHttpContextAccessor httpContextAccessor)
        {
            mySqlConnection = conn;
            _httpContextAccessor = httpContextAccessor;
        }

        //public FormController(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        [HttpGet]
        public IActionResult LoginPage(LoginIDViewModel login)
        {            
            var isLoggedIn = _httpContextAccessor.HttpContext.Session.GetInt32("isLoggedIn");
            if (isLoggedIn == 1)
            {
                return View("/Views/GameWindow/PlayerDetails.cshtml");
            }
            else
                return View(login);
           
        }

        [HttpGet]
        public IActionResult Register(RegisterViewModel regDetails)
        {

            return View(regDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginIDViewModel logDetails)
        {
            // Open SQL - DB connection
            await mySqlConnection.OpenAsync();

            // setup connection
            using var cmd = new MySqlCommand();
            cmd.Connection = mySqlConnection;

            // set variables for the login properties
            string password = logDetails.LoginPwd;
            string email = logDetails.LoginID;
            string mysqlEmail = "";
            string mysqlPass = "";

            // get SQL statement to be executed
            cmd.CommandText = "SELECT email, password FROM users WHERE email = @email AND password = @password";

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                mysqlEmail = reader.GetString("email");
                mysqlPass = reader.GetString("password");
            }

            if (mysqlEmail == email && mysqlPass == password)
            {
                _httpContextAccessor.HttpContext.Session.SetString("_email", email);
                _httpContextAccessor.HttpContext.Session.SetInt32("isLoggedIn", 1);
                return View("/Views/GameWindow/PlayerDetails.cshtml");
            }
            else
                return View("LoginPage");
            

        }

        

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterViewModel regDetails)
        {
            string fname = regDetails.FName;
            string lname = regDetails.LName;
            string email = regDetails.LoginID;
            string password = regDetails.LoginPwd;

            await mySqlConnection.OpenAsync();
            using var command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandText = "INSERT INTO users (first_name, last_name, email, password) VALUES (@fname, @lname, @email, @password)";
            command.Parameters.AddWithValue("@fname", fname);
            command.Parameters.AddWithValue("@lname", lname);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);

            try
            {
                // is the data insert successful?
                int rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch(MySqlException ex)
            {
                string err = ex.Message;
                if(ex.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                    err = "The email you are trying to signup with already exists in our systems";

                if(ex.Number == (int)MySqlErrorCode.NoDefaultForField)
                    err = "Kindly fill in all required fields";

                ViewBag.ErrorMessage = err;
                return View("Register");
            }


            string sux = "Registration successful. Please login";
            ViewBag.SuccessMessage = sux;
            return View("LoginPage");
        }

    }
}
