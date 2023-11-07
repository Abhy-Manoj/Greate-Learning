using myproject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myproject.Controllers
{
    public class QuizController : Controller
    {

        public ActionResult AddQuiz(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                int courseId = quiz.CourseId;

                using (SqlConnection connection = new SqlConnection("Data Source=SYSLP1468;Initial Catalog=MyProjectDatabase;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("SPI_Quiz", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@CourseId", courseId));
                        cmd.Parameters.Add(new SqlParameter("@Question", quiz.Question));
                        cmd.Parameters.Add(new SqlParameter("@Option1", quiz.Option1));
                        cmd.Parameters.Add(new SqlParameter("@Option2", quiz.Option2));
                        cmd.Parameters.Add(new SqlParameter("@Option3", quiz.Option3));
                        cmd.Parameters.Add(new SqlParameter("@Option4", quiz.Option4));
                        cmd.Parameters.Add(new SqlParameter("@C_Option", quiz.C_Option));

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["QuizAddedMessage"] = "Quiz Added Successfully!";
                return RedirectToAction("AddQuiz", "Quiz", new { courseId = courseId });
            }

            return View(quiz);
        }

    }
}