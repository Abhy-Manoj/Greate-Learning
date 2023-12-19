using myproject.Models;
using System.Configuration;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System;

public class CourseController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    // Get the course details
    public ActionResult Index()
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        var courses = dataAccess.GetAllCourses();
        return View(courses);
    }

    [HttpPost]
    public JsonResult Details(int id)
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        var course = dataAccess.GetCourseById(id);
        return Json(course, JsonRequestBehavior.DenyGet);
    }

    // Create new Course
    public ActionResult CreateCourse()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Courses course)
    {
        // Adding the image file to a specific folder and converting to base64
        if (course.ImageFile != null && course.ImageFile.ContentLength > 0)
        {
            string base64Image = ConvertFileToBase64(course.ImageFile);
            course.ImageUrl = base64Image;
        }
        //adding the video file to a specific folder
        if (course.VideoFile != null && course.VideoFile.ContentLength > 0)
        {
            string videoFileName = Path.GetFileName(course.VideoFile.FileName);
            string videoFilePath = Path.Combine(Server.MapPath("~/Videos"), videoFileName);
            course.VideoFile.SaveAs(videoFilePath);
            course.VideoUrl = "/Videos/" + videoFileName;

        }

        course.Count = course.Count;
        var dataAccess = new CourseDataAccess(_connectionString);
        dataAccess.InsertCourse(course);
        return RedirectToAction("Courses", "Admin");
    }

    // Edit the Course
    public ActionResult Edit(int id)
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        var course = dataAccess.GetCourseById(id);
        return View(course);
    }

    [HttpPost]
    public ActionResult Edit(int id, HttpPostedFileBase ImageFile, HttpPostedFileBase VideoFile, Courses editedCourse)
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        var course = dataAccess.GetCourseById(id);

        // Check image file and convert to base64
        if (ImageFile != null && ImageFile.ContentLength > 0)
        {
            string base64Image = ConvertFileToBase64(ImageFile);
            course.ImageUrl = base64Image;
        }

        // Edit video file
        if (VideoFile != null && VideoFile.ContentLength > 0)
        {
            string videoFileName = Path.GetFileName(VideoFile.FileName);
            string videoFilePath = Path.Combine(Server.MapPath("~/Videos"), videoFileName);
            VideoFile.SaveAs(videoFilePath);
            course.VideoUrl = "/Videos/" + videoFileName;

        }

        course.CourseName = editedCourse.CourseName;
        course.Description = editedCourse.Description;
        course.Duration = editedCourse.Duration;
        course.Count = editedCourse.Count;

        dataAccess.UpdateCourse(course);

        return Json(course, JsonRequestBehavior.DenyGet);
    }

    // Delete course
    public ActionResult Delete(int id)
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        var course = dataAccess.GetCourseById(id);
        return View(course);
    }

    [HttpPost]
    public JsonResult DeleteConfirmed(int id)
    {
        var dataAccess = new CourseDataAccess(_connectionString);
        dataAccess.DeleteCourse(id);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }

    // Helper method to convert file to base64
    private string ConvertFileToBase64(HttpPostedFileBase file)
    {
        using (Stream inputStream = file.InputStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                inputStream.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                string base64 = Convert.ToBase64String(fileBytes);
                return base64;
            }
        }
    }
}
