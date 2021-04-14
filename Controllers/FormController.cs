using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using MySql.Data.MySqlClient;
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
        public FormController(MySqlConnection connection)
        {
            mySqlConnection = connection;
        }

        [HttpGet]
        public IActionResult LoginPage(LoginIDViewModel login)
        {
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
                return View("/Views/GameWindow/PlayerDetails.cshtml");
            else
                return View("LoginPage");

            ISession("LoggedInUserDetail") = new LoggedInUserDetail("Greg Smith", "gsmith", "admin");


        }

        private Login ISession(string v)
        {
            throw new NotImplementedException();
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

        public ISession LoginSession()
        {
            Session("LoggedInUserDetail") = new LoggedInUserDetail("Greg Smith", "gsmith", "admin");
            return Ok();
        }
    }
}
