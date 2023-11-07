using myproject.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

public class SelectedController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public ActionResult JoinCourse(int courseId)
    {
        string currentUsername = (string)Session["Username"];

        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SELECT Id FROM Signup WHERE Username = @Username", connection))
            {
                getUserIdCmd.Parameters.AddWithValue("@Username", currentUsername);
                var userIdObj = getUserIdCmd.ExecuteScalar();
                if (userIdObj != null)
                {
                    if (int.TryParse(userIdObj.ToString(), out currentUserId))
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_JoinCourse", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", currentUserId);
                            cmd.Parameters.AddWithValue("@CourseId", courseId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        if (currentUserId != -1)
        {
            return RedirectToAction("CourseJoined");
        }
        else
        {
            return RedirectToAction("ErrorPage");
        }
    }

}
