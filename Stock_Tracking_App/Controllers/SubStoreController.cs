using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace stockProject.Controllers
{
    public class SubStoreController : Controller
    {
        // GET: SubStore
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            return View(db.ALT_DEPO.ToList());
        }
        public ActionResult Add()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ALT_DEPO obj)
        {
            bool isCreated = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.ALT_DEPO.Where(w => w.ALT_DEPO_ADI == obj.ALT_DEPO_ADI).FirstOrDefault();

                        if (objectInDb != null)
                        {
                            isCreated = false;
                            switch (objectInDb.STATU)
                            {

                                case true:
                                    TempData["msg"] = "There is an active record exists.";
                                    break;
                                case false:
                                    TempData["msg"] = "There is an inactive record exits ! please fix the record.";
                                    break;

                            }
                        }
                        if (isCreated == true)
                        {
                            obj.STATU = true;
                            obj.OLUSTURMA_TARIHI = DateTime.Now;
                            obj.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                            db.ALT_DEPO.Add(obj);
                            db.SaveChanges();
                            TempData["msg"] = "Creation is successfull";
                        }

                    }
                    else
                    {
                        isCreated = false;
                        TempData["msg"] = "There is a database issue occured !";
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
            ALT_DEPO edited = db.ALT_DEPO.Find(id);

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
        public ActionResult Edit(ALT_DEPO edited)
        {
            bool isEdited = true;
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        DEPO_ESLESTIRME matching = db.DEPO_ESLESTIRME.Where(w => w.ALT_DEPO_ID != edited.ALT_DEPO_ID).FirstOrDefault();

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
                            ALT_DEPO objectInDb = db.ALT_DEPO.Where(w => w.ALT_DEPO_ID != edited.ALT_DEPO_ID && w.ALT_DEPO_ADI == edited.ALT_DEPO_ADI).FirstOrDefault();
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
                            if (isEdited == true)
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
            bool silinecekMi = true;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ALT_DEPO silinecek = db.ALT_DEPO.Find(id);

            if (silinecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                //Depo eşleştirme tablosunda alt depo id ile ilgili herhangi bir kayıt var mı kontrolü
                DEPO_ESLESTIRME eslestirme = db.DEPO_ESLESTIRME.Where(w => w.ALT_DEPO_ID == silinecek.ALT_DEPO_ID).FirstOrDefault();

                if (eslestirme != null)
                {
                    silinecekMi = false;
                    silinecek.STATU = false;
                    db.Entry(silinecek).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "toastr.warning('" + "Depo eşleştirmede depoya bağlı kayıt " +
                                     "olduğu için silinemez, statüsü pasif olarak ayarlandı!"
                                   + "', '', {positionClass: 'md-toast-top-right'});"
                                   + "$('#toast-container').attr('class','md-toast-top-right');";

                }

                else
                {
                    db.ALT_DEPO.Remove(silinecek);
                    db.SaveChanges();

                    TempData["msg"] = "toastr.success('" + "Silme işlemi başarılı!"
                                   + "', '', {positionClass: 'md-toast-top-right'});"
                                   + "$('#toast-container').attr('class','md-toast-top-right');";
                }
            }

            return RedirectToAction("Index");
        }

    }
}