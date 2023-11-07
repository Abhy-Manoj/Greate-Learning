using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
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

                    string adminQuery = "SELECT Id, Username FROM Login WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(adminQuery, connection))
                    {
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
                        string userQuery = "SELECT Password, Id, Username FROM Signup WHERE Username = @Username";
                        using (SqlCommand command = new SqlCommand(userQuery, connection))
                        {
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
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session["Username"] = model.Username;
                    if (userId != -1)
                    {
                        Session["UserId"] = userId;
                    }

                    if (userIsAdmin)
                    {
                        return RedirectToAction("Homepage", "Admin");
                    }
                    else
                    {
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
