using myproject.Models;
using System.Configuration;
using System.Web.Mvc;

public class NewadminController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public ActionResult Index()
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        var admins = dataAccess.GetAllAdmins();
        return View(admins);
    }

    [HttpPost]
    public JsonResult Details(int id)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        var admin = dataAccess.GetAdminById(id);
        return Json(admin, JsonRequestBehavior.DenyGet);
    }

    public ActionResult CreateAdmin()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Admin admin)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        dataAccess.InsertAdmin(admin);
        return RedirectToAction("Admindetails", "Admin");
    }

    public ActionResult Edit(int id)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        var admin = dataAccess.GetAdminById(id);
        return View(admin);
    }

    [HttpPost]
    public JsonResult Edit(Admin admin)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        dataAccess.UpdateAdmin(admin);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }

    public ActionResult EditPass(int id)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        var admin = dataAccess.GetAdminById(id);
        return View(admin);
    }

    [HttpPost]
    public JsonResult EditPass(Admin admin)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        dataAccess.UpdateAdminPass(admin);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }

    public ActionResult Delete(int id)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        var admin = dataAccess.GetAdminById(id);
        return View(admin);
    }

    [HttpPost]
    public JsonResult DeleteConfirmed(int id)
    {
        var dataAccess = new AdminDataAccess(_connectionString);
        dataAccess.DeleteAdmin(id);
        return Json("Success", JsonRequestBehavior.DenyGet);
    }
}
