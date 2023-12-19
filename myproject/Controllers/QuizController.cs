using myproject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace myproject.Controllers
{
    public class QuizController : Controller
    {
        //Add new quiz to the course
        public ActionResult AddQuiz(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuiz(Quiz quiz)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int courseId = quiz.CourseId;

                    string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(_connectionString))
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
            catch (Exception ex)
            {
                // Handle exceptions during quiz addition
                ViewBag.ErrorMessage = "An error occurred while adding the quiz.";
                return View("Error");
            }
        }

        //Get the quiz details for the admin
        public ActionResult GetQuiz(int courseId)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            List<Quiz> quizzes = new List<Quiz>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SPS_GetQuiz", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CourseId", courseId));

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Quiz quiz = new Quiz
                            {
                                QuizId = (int)reader["QuizId"],
                                CourseId = (int)reader["CourseId"],
                                Question = reader["Question"].ToString(),
                                Option1 = reader["Option1"].ToString(),
                                Option2 = reader["Option2"].ToString(),
                                Option3 = reader["Option3"].ToString(),
                                Option4 = reader["Option4"].ToString(),
                                C_Option = (int)reader["C_Option"],
                            };

                            quizzes.Add(quiz);
                        }
                    }
                }
            }

            return View(quizzes);
        }

        //Attend the quiz for the user
        public ActionResult TakeQuiz(int courseId)
        {
            int currentUserId = (int)Session["UserId"];
            int userId = currentUserId;

            string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SPS_TakeQuiz", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CourseId", courseId));
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Quiz quiz = new Quiz
                            {
                                QuizId = (int)reader["QuizId"],
                                CourseId = courseId,
                                Question = reader["Question"].ToString(),
                                Option1 = reader["Option1"].ToString(),
                                Option2 = reader["Option2"].ToString(),
                                Option3 = reader["Option3"].ToString(),
                                Option4 = reader["Option4"].ToString(),
                                
                            };

                            List<Quiz> quizList = new List<Quiz> { quiz }; // Create a List with a single quiz
                            return View(quizList);
                        }
                        else
                        {
                            // Handle the case where no more questions are available
                            ViewBag.ErrorMessage = "No more questions available for this quiz.";
                            return View("Error");
                        }
                    }
                }
            }
        }

        //Submit the quiz answer
        [HttpPost]
        public ActionResult SubmitQuiz(List<int> answer, int quizId)
        {
            int currentUserId = (int)Session["UserId"];
            int userId = currentUserId;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                List<int> correctAnswers = new List<int>(); // List to store correct answers
                int totalQuestions = answer.Count; // Total number of questions

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var singleAnswer in answer)
                    {
                        using (SqlCommand cmd = new SqlCommand("SPI_QuizAnswer", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@QuizId", quizId));
                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                            cmd.Parameters.Add(new SqlParameter("@Answer", singleAnswer));

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Fetch the correct answers from the database based on the quizId
                    using (SqlCommand getCorrectAnswersCmd = new SqlCommand("SPS_GetCorrectAnswers", connection))
                    {
                        getCorrectAnswersCmd.CommandType = CommandType.StoredProcedure;
                        getCorrectAnswersCmd.Parameters.Add(new SqlParameter("@QuizId", quizId));

                        using (SqlDataReader reader = getCorrectAnswersCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                correctAnswers.Add((int)reader["C_Option"]);
                            }
                        }
                    }
                }

                // Calculate the score by comparing user's answers with correct answers
                int score = 0;
                for (int i = 0; i < totalQuestions; i++)
                {
                    if (answer[i] == correctAnswers[i])
                    {
                        score++;
                    }
                }

                // Showing quiz results
                return RedirectToAction("QuizResult", new { score = score });
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur while submitting answers
                ViewBag.ErrorMessage = "An error occurred while submitting the quiz answers.";
                return View("Error");
            }
        }

        //Show the quiz score
        public ActionResult QuizResult(int score)
        {
            ViewBag.Score = score;
            return View("QuizResult");
        }

        //Delete the quiz
        public ActionResult DeleteQuiz(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SPD_Quiz", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("GetQuiz");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Quiz not found.";
                            return View("Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the quiz.";
                return View("Error");
            }
        }
    }
}