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
    public class StockManagementController : Controller
    {
        // GET: StockManagement
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            ViewBag.STOK_OLCUBIRIM = new SelectList((from t1 in db.OLCU_BIRIMI
                                                     select new
                                                     {
                                                         OLCUBIRIM_ID = t1.OLCUBIRIM_ID,
                                                         OLCUBIRIM_AD = t1.OLCUBIRIM_ADI
                                                     }).Distinct().OrderBy(o => o.OLCUBIRIM_ID), "OLCUBIRIM_ID", "OLCUBIRIM_AD");
            return View(db.STOK.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.STOK_OLCUBIRIM = new SelectList((from t1 in db.OLCU_BIRIMI
                                                     select new
                                                     {
                                                         OLCUBIRIM_ID = t1.OLCUBIRIM_ID,
                                                         OLCUBIRIM_AD = t1.OLCUBIRIM_ADI
                                                     }).Distinct().OrderBy(o => o.OLCUBIRIM_ID), "OLCUBIRIM_ID", "OLCUBIRIM_AD");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(STOK yeni)
        {
            bool kaydedilecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.STOK.Where(w => w.STOK_ID == yeni.STOK_ID).FirstOrDefault();

                        if (objectInDb != null)


                        {
                            kaydedilecekMi = false;

                            switch (objectInDb.STATU)
                            {

                                case true:
                                    TempData["msg"] = "toastr.warning('" +
                            "There is an active record exist." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                                    break;

                                case false:
                                    TempData["msg"] = "toastr.warning('" +
                            "There is an inactive record exist." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                                    break;

                            }
                        }

                        //kaydet
                        if (kaydedilecekMi == true)
                        {


                            yeni.STATU = true;
                            yeni.OLUSTURMA_TARIHI = DateTime.Now;
                            yeni.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]); ; //şimdilik admin kullanıcısının idsi
                            db.STOK.Add(yeni);
                            db.SaveChanges();

                            TempData["msg"] = "toastr.warning('" +
                            "Record is successfully added.." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');"; ;
                        }



                    }

                    else
                    {
                        kaydedilecekMi = false;
                        TempData["msg"] = "toastr.warning('" +
                            "Database problem occured." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                    }

                }
                catch (Exception ex)
                {
                    kaydedilecekMi = false;
                    TempData["msg"] = "toastr.warning('" +
                   ex.Message +
                   "', '', {positionClass: 'md-toast-top-right'});" +
                   "$('#toast-container').attr('class','md-toast-top-right');";
                }


            }

            else
            {
                kaydedilecekMi = false;
            }

            //sayfa yönlendirmeleri
            if (kaydedilecekMi == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.STOK_OLCUBIRIM = new SelectList((from t1 in db.OLCU_BIRIMI
                                                         select new
                                                         {
                                                             OLCUBIRIM_ID = t1.OLCUBIRIM_ID,
                                                             OLCUBIRIM_AD = t1.OLCUBIRIM_ADI
                                                         }).Distinct().OrderBy(o => o.OLCUBIRIM_ID), "OLCUBIRIM_ID", "OLCUBIRIM_AD", yeni.STOK_OLCUBIRIM);
                return View(yeni);
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.STOK_OLCUBIRIM = new SelectList((from t1 in db.OLCU_BIRIMI
                                                     select new
                                                     {
                                                         OLCUBIRIM_ID = t1.OLCUBIRIM_ID,
                                                         OLCUBIRIM_AD = t1.OLCUBIRIM_ADI
                                                     }).Distinct().OrderBy(o => o.OLCUBIRIM_ID), "OLCUBIRIM_ID", "OLCUBIRIM_AD");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            STOK edited = db.STOK.Find(id);

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
        public ActionResult Edit(STOK edited)
        {
            bool isEdited = true;
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        STOK objectInDb = db.STOK.Where(w => w.STOK_ID != edited.STOK_ID && w.STOK_AD == edited.STOK_AD).FirstOrDefault();
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
                ViewBag.STOK_OLCUBIRIM = new SelectList((from t1 in db.OLCU_BIRIMI
                                                         select new
                                                         {
                                                             OLCUBIRIM_ID = t1.OLCUBIRIM_ID,
                                                             OLCUBIRIM_AD = t1.OLCUBIRIM_ADI
                                                         }).Distinct().OrderBy(o => o.OLCUBIRIM_ID), "OLCUBIRIM_ID", "OLCUBIRIM_AD", edited.STOK_OLCUBIRIM);
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

            STOK silinecek = db.STOK.Find(id);

            if (silinecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                //Stok durum tablosunda stok id ile ilgili herhangi bir kayıt var mı kontrolü
                STOK_DURUM stokDurum = db.STOK_DURUM.Where(w => w.STOK_ID == silinecek.STOK_ID).FirstOrDefault();
                STOK_HAREKET stokHareket = db.STOK_HAREKET.Where(w => w.STOK_ID == silinecek.STOK_ID).FirstOrDefault();

                if (stokDurum != null || stokHareket != null)
                {
                    silinecekMi = false;
                    silinecek.STATU = false;
                    db.Entry(silinecek).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "toastr.warning('" + "Depo eşleştirmede depo eşleştirmeye bağlı kayıt " +
                                     "olduğu için silinemez, statüsü pasif olarak ayarlandı!"
                                   + "', '', {positionClass: 'md-toast-top-right'});"
                                   + "$('#toast-container').attr('class','md-toast-top-right');";

                }

                else
                {
                    db.STOK.Remove(silinecek);
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