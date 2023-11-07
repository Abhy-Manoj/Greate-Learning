using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myproject.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using myproject.Repository;

namespace myproject.Controllers
{
    public class RegistrationController : Controller
    {


        public ActionResult AddDetail()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddDetail(Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RegistrationRepository RegRepo = new RegistrationRepository();
                    var result = RegRepo.AddDetails(registration);

                    if (result.Item1)
                    {
                        ViewBag.SuccessMessage = "User Details Added Successfully";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result.Item2;
                        return View();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur during the registration process
                ViewBag.ErrorMessage = "An error occurred while adding user details.";
                return View();
            }
        }
    }

}


