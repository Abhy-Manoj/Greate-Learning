using myproject.Models;
using System.Configuration;
using System.Web.Mvc;
using System.IO;
using System.Web;

public class CourseController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

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

    public ActionResult CreateCourse()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Courses course)
    {
        if (course.ImageFile != null && course.ImageFile.ContentLength > 0)
        {
            string fileName = Path.GetFileName(course.ImageFile.FileName);
            string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
            course.ImageFile.SaveAs(filePath);
            course.ImageUrl = "/Images/" + fileName;
        }

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

        // Check image file
        if (ImageFile != null && ImageFile.ContentLength > 0)
        {
            string fileName = Path.GetFileName(ImageFile.FileName);
            string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
            ImageFile.SaveAs(filePath);
            course.ImageUrl = "/Images/" + fileName;
        }

        // Check video file
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

        dataAccess.UpdateCourse(course);

        return Json(course, JsonRequestBehavior.DenyGet);
    }

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
}
