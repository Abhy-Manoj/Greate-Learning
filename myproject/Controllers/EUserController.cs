using myproject.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;

public class EUserController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    //View details of the user
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

    //Edit the user details
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

    //Edit the user password
    public ActionResult EditPass(int id)
    {
        var dataAccess = new User(_connectionString);
        var user = dataAccess.GetUserById(id);
        return View(user);
    }

    [HttpPost]
    public JsonResult EditPass(ViewUsers user)
    {
        var dataAccess = new User(_connectionString);
        dataAccess.UpdateUserPass(user);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }

    //Delete the user details from database
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
