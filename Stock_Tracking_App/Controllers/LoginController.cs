using stockProject.Helper;
using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace stockProject.Controllers
{
    
    public class LoginController : Controller
    {
        // GET: Login
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LogIn login)
        {
            Boolean isLoggedIn = false;

            if(ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var user = db.KULLANICI.Where(w => w.KUL_USERNAME == login.USERNAME && w.KUL_SIFRE == login.PASSWORD).FirstOrDefault();
                        mandatoryParameterDefiniton.adminDefinition();
                        mandatoryParameterDefiniton.ReportUser();
                        mandatoryParameterDefiniton.StoreManager();

                        if (user != null)
                        {
                            Session["KULLANICI_ID"] = user.KULLANICI_ID;
                            Session["KULLANICI_USERNAME"] = user.KUL_USERNAME;
                            Session["KUL_AD"] = user.KUL_AD;
                            Session["KUL_SOYAD"] = user.KUL_SOYAD;
                            Session["KUL_TIP"] = user.KUL_TIP;
                            Session.Timeout = 120;
                            isLoggedIn = true;
                            FormsAuthentication.SignOut();
                            FormsAuthentication.SetAuthCookie(user.KUL_AD, false);

                        }
                        else
                        {
                            isLoggedIn = false;
                            killLoginSession(this.HttpContext);
                            FormsAuthentication.SignOut();
                            TempData["msg"] = "toastr.warning('" +
                            "Hatalı giriş bilgileri" +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                        }
                    }
                    else {
                        isLoggedIn = false;
                        TempData["msg"] = "toastr.warning('" +
                            "Hatalı giriş bilgileri" +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                    }
                    
                }catch (Exception ex)
                {
                    isLoggedIn = false;
                    TempData["msg"] = "toastr.warning('" +
                   ex.Message +
                   "', '', {positionClass: 'md-toast-top-right'});" +
                   "$('#toast-container').attr('class','md-toast-top-right');";
                }
            }
            if (isLoggedIn == true)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {

                FormsAuthentication.SignOut();
                return View(login);
            }
            
        }

        public ActionResult LogOut()
        {
            killLoginSession(this.HttpContext);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Login");
        }

        public static void killLoginSession(HttpContextBase context)
        {
            context.Session["KULLANICI_ID"] = null;
            context.Session["KULLANICI_USERNAME"] = null;
            context.Session["KUL_AD"] = null;
            context.Session["KUL_SOYAD"] = null;
            context.Session["KUL_TIP"] = null;
        }
    }
}