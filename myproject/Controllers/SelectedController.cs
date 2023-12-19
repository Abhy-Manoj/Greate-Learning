using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

public class SelectedController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    //When user tries to join a course
    public ActionResult JoinCourse(int courseId)
    {
        string currentUsername = (string)Session["Username"];
        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SP_SignupUser", connection))
            {
                getUserIdCmd.Parameters.AddWithValue("@Username", currentUsername);
                getUserIdCmd.CommandType = CommandType.StoredProcedure;
                var userIdObj = getUserIdCmd.ExecuteScalar();
                if (userIdObj != null && int.TryParse(userIdObj.ToString(), out currentUserId))
                {
                    // Call stored procedure to insert enrollment request
                    using (SqlCommand insertRequestCmd = new SqlCommand("SP_InsertEnrollmentRequest", connection))
                    {
                        insertRequestCmd.CommandType = CommandType.StoredProcedure;
                        insertRequestCmd.Parameters.AddWithValue("@UserId", currentUserId);
                        insertRequestCmd.Parameters.AddWithValue("@CourseId", courseId);
                        insertRequestCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        return RedirectToAction("CourseRequestPending");
    }

}
