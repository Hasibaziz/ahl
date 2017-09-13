using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Test.Models;
using System.Data;

namespace Test.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login(LoginModel model)
        {
            ModelState.Clear();
            return View("Login", model);
        }
        [HttpPost]
        public ActionResult Login(string submit, LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == "123" && model.UserName == "Admin")
                {
                    SystemContact contactAd = new SystemContact();
                    contactAd.FirstName = "Admin";
                    contactAd.LastName = "Admin";
                    CurrentUserName = "Admin";
                    Session["UserName"] = "Admin";
                    //Guid _id = new Guid("0b39599f-1db7-4156-8ceb-1da3dce8b5bf");
                    contactAd.Id = "1";
                    SetLoginSessionData(model, false);
                    return RedirectToAction("Index", "Home");
                }
                else if (model.Password == "123" && model.UserName == "SCAN")
                {
                    SystemContact contactAd = new SystemContact();
                    contactAd.FirstName = "SCAN";
                    contactAd.LastName = "SCAN";
                    CurrentUserName = "SCAN";
                    Session["UserName"] = "SCAN";
                    //Guid _id = new Guid("0b39599f-1db7-4156-8ceb-1da3dce8b5bf");
                    contactAd.Id = "1";
                    SetLoginSessionData(model, false);
                    return RedirectToAction("Index", "Home");
                }
                else if (model.Password == "123#" && model.UserName == "PAC")
                {
                    SystemContact contactAd = new SystemContact();
                    contactAd.FirstName = "PAC";
                    contactAd.LastName = "PAC";
                    CurrentUserName = "PAC";
                    Session["UserName"] = "PAC";
                    //Guid _id = new Guid("0b39599f-1db7-4156-8ceb-1da3dce8b5bf");
                    contactAd.Id = "1";
                    SetLoginSessionData(model, false);
                    return RedirectToAction("Index", "Home");
                }
                else if (model.Password == "123" && model.UserName == "LINE")
                {
                    SystemContact contactAd = new SystemContact();
                    contactAd.FirstName = "LINE";
                    contactAd.LastName = "LINE";
                    CurrentUserName = "LINE";
                    Session["UserName"] = "LINE";
                    //Guid _id = new Guid("0b39599f-1db7-4156-8ceb-1da3dce8b5bf");
                    contactAd.Id = "1";
                    SetLoginSessionData(model, false);
                    return RedirectToAction("Index", "Home");
                }

                //DataTable dt = DAL.User.GetUserInfo(model.UserName);
                //if (dt.Rows.Count > 0)
                //{
                //    if (model.Password == dt.Rows[0]["VPASSWORD"].ToString())
                //    {
                //        SystemContact contact = new SystemContact();
                //        contact.FirstName = "";
                //        contact.LastName =dt.Rows[0]["VEMPNAME"].ToString() ;
                //        contact.Id = dt.Rows[0]["VUSERID"].ToString();
                //        SetLoginSessionData(contact, false);
                //        return RedirectToAction("Index", "Home");
                //    }
                //    else
                //    {

                //        ModelState.AddModelError("UserName", "invalid username or password.");
                //    }
                //}
                //else
                //{

                //    ModelState.AddModelError("UserName", "invalid username or password.");
                //}
            }
            return View("Login", model);
        }

        public ActionResult Logout(SystemUserModel model)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
