using myproject.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;

public class EUserController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public ActionResult Index()
    {
        var dataAccess = new User(_connectionString);
        var users = dataAccess.GetAllUsers();
        return View(users);
    }

    [HttpPost]
    public JsonResult Details(int id)
    {
        var dataAccess = new User(_connectionString);
        var user = dataAccess.GetUserById(id);
        return Json(user, JsonRequestBehavior.DenyGet);
    }
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(ViewUsers user)
    {
        var dataAccess = new User(_connectionString);
        dataAccess.InsertUser(user);
        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var dataAccess = new User(_connectionString);
        var user = dataAccess.GetUserById(id);
        return View(user);
    }

    [HttpPost]
    public JsonResult Edit(ViewUsers user)
    {
        var dataAccess = new User(_connectionString);
        dataAccess.UpdateUser(user);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }

    public ActionResult Delete(int id)
    {
        var dataAccess = new User(_connectionString);
        var user = dataAccess.GetUserById(id);
        return View(user);
    }

    [HttpPost]
    public JsonResult DeleteConfirmed(int id)
    {
        var dataAccess = new User(_connectionString);
        dataAccess.DeleteUser(id);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }
}
