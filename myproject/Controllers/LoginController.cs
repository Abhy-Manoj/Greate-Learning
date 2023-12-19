using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using myproject.Models;

namespace myproject.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = false;
                bool userIsAdmin = false;
                int userId = -1;
                string username = null;

                string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_LoginUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isAuthenticated = true;
                                userIsAdmin = true;
                                userId = (int)reader["Id"];
                                username = reader["Username"].ToString();
                            }
                        }
                    }

                    if (!isAuthenticated)
                    {
                        using (SqlCommand command = new SqlCommand("SP_SignupUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", model.Username);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string hashedPassword = reader["Password"].ToString();
                                    userId = (int)reader["Id"];
                                    username = reader["Username"].ToString();

                                    if (BCrypt.Net.BCrypt.Verify(model.Password, hashedPassword))
                                    {
                                        isAuthenticated = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (isAuthenticated)
                {
                    Session["Username"] = model.Username;

                    if (userId != -1)
                    {
                        Session["UserId"] = userId;
                    }
                    if (userIsAdmin)
                    {
                        Session["Roles"] = "Admin";
                        return RedirectToAction("Homepage", "Admin");
                    }
                    else
                    {
                        Session["Roles"] = "User";
                        return RedirectToAction("Userhome", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials.");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
