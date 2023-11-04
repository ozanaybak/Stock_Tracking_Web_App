using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Data.Entity.Infrastructure;

namespace stockProject.Controllers
{
    public class DEPOController : Controller
    {
        // GET: DEPO
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            return View(db.DEPO.ToList());
        }

        public ActionResult Add()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DEPO obj)
        {
            bool isCreated = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.DEPO.Where(w => w.DEPO_ADI == obj.DEPO_ADI).FirstOrDefault();

                        if (objectInDb != null)
                        {
                            isCreated = false;
                            switch (objectInDb.STATU)
                            {

                                case true:
                                    TempData["msg"] = "toastr.warning('" +
                            "There is an active record exists" +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                                    break;
                                case false:
                                    TempData["msg"] = "toastr.warning('" +
                            "There is an inactive record exists please fix the record." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                                    break;

                            }
                        }
                        if (isCreated == true)
                        {
                            obj.STATU = true;
                            obj.OLUSTURMA_TARIHI = DateTime.Now;
                            obj.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                            db.DEPO.Add(obj);
                            db.SaveChanges();
                            TempData["msg"] = "toastr.warning('" +
                            "Creation is successfull." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                        }

                    }
                    else
                    {
                        isCreated = false;
                        TempData["msg"] = "toastr.warning('" +
                            "Database problem occured." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                    }
                }
                catch (Exception ex)
                {
                    isCreated = false;
                    TempData["msg"] = "toastr.warning('" +
                   ex.Message +
                   "', '', {positionClass: 'md-toast-top-right'});" +
                   "$('#toast-container').attr('class','md-toast-top-right');";
                }

            }
            else
            {
                isCreated = false;
            }
            if (isCreated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPO edited = db.DEPO.Find(id);

            if (edited == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(edited);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DEPO edited)
        {
            bool isEdited = true;
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        DEPO_ESLESTIRME matching = db.DEPO_ESLESTIRME.Where(w => w.DEPO_ID == edited.DEPO_ID).FirstOrDefault();

                        if (matching != null)
                        {
                            isEdited = false;
                            TempData["msg"] = TempData["msg"] = "toastr.warning('" +
                            "There is an active matched record exist." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                        }
                        else
                        {
                            DEPO objectInDb = db.DEPO.Where(w => w.DEPO_ID != edited.DEPO_ID && w.DEPO_ADI == edited.DEPO_ADI).FirstOrDefault();
                            if (objectInDb != null)
                            {
                                isEdited = false;
                                switch (objectInDb.STATU)
                                {

                                    case true:
                                        TempData["msg"] = "toastr.warning('" +
                                "There is an active record exists" +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";
                                        break;
                                    case false:
                                        TempData["msg"] = "toastr.warning('" +
                                "There is an inactive record exists please fix the record." +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";
                                        break;

                                }
                            }
                            if(isEdited == true)
                            {
                                edited.GUNCELLEYEN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                edited.GUNCELLEME_TARIHI = DateTime.Now;
                                db.Entry(edited).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["msg"] = "toastr.success('" +
                                "Record successfully edited. " +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";
                            }
                        }

                    }
                    else
                    {
                        isEdited = false;
                        TempData["msg"] = "toastr.warning('" +
                                "Database connection error. " +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";
                    }
                }
                catch (Exception ex)
                {
                    isEdited = false;
                    TempData["msg"] = "toastr.warning('" +
                   ex.Message +
                   "', '', {positionClass: 'md-toast-top-right'});" +
                   "$('#toast-container').attr('class','md-toast-top-right');";

                }

            }
            else
            {
                isEdited = false;
            }

            if (isEdited == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(edited);
            }
            
        }

        public ActionResult Delete(int? id)
        {
            bool isdeleted = true;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            DEPO deleted = db.DEPO.Find(id);

            if(deleted == null)
            {
                return HttpNotFound();
            }
            else
            {
                DEPO_ESLESTIRME eslestirme = db.DEPO_ESLESTIRME.Where(w => w.DEPO_ID == deleted.DEPO_ID).FirstOrDefault();

                if(eslestirme != null)
                {
                    isdeleted = false;
                    deleted.STATU = false;
                    db.Entry(deleted).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["msg"] = "toastr.warning('" +
                                "Database connection error. " +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";

                }
                else
                {
                    db.DEPO.Remove(deleted);
                    db.SaveChanges();

                    TempData["msg"] = "toastr.warning('" +
                                "Record successfully deleted " +
                                "', '', {positionClass: 'md-toast-top-right'});" +
                                "$('#toast-container').attr('class','md-toast-top-right');";
                }

            }

            return RedirectToAction("Index");
            
        }

    }
}