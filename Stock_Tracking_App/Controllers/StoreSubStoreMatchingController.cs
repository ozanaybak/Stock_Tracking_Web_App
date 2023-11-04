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
    public class StoreSubStoreMatchingController : Controller
    {
        // GET: StoreSubStoreMatching
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            return View(db.DEPO_ESLESTIRME.ToList());
        }
        
        public ActionResult Create()
        {
            ViewBag.DEPO_ID = new SelectList((from t1 in db.DEPO
                                              where t1.STATU == true
                                              select new
                                              {
                                                  DEPO_ID = t1.DEPO_ID,
                                                  DEPO_ADI = t1.DEPO_ADI
                                              }).Distinct().OrderBy(o => o.DEPO_ID), "DEPO_ID", "DEPO_ADI") ;

            ViewBag.ALT_DEPO_ID = new SelectList((from t2 in db.ALT_DEPO
                                              where t2.STATU == true
                                              select new
                                              {
                                                  ALT_DEPO_ID = t2.ALT_DEPO_ID,
                                                  ALT_DEPO_ADI = t2.ALT_DEPO_ADI
                                              }).Distinct().OrderBy(o => o.ALT_DEPO_ID), "ALT_DEPO_ID", "ALT_DEPO_ADI");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DEPO_ESLESTIRME yeni)
        {
            bool kaydedilecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.DEPO_ESLESTIRME.Where(w => w.DEPO_ID == yeni.DEPO_ID && w.ALT_DEPO_ID == yeni.ALT_DEPO_ID).FirstOrDefault();

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
                            yeni.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]); //şimdilik admin kullanıcısının idsi
                            db.DEPO_ESLESTIRME.Add(yeni);
                            db.SaveChanges();

                            TempData["msg"] = "toastr.warning('" +
                            "Creation is successfull." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
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
                ViewBag.DEPO_ID = new SelectList((from t1 in db.DEPO
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      DEPO_ID = t1.DEPO_ID,
                                                      DEPO_ADI = t1.DEPO_ADI
                                                  }).Distinct().OrderBy(o => o.DEPO_ID), "DEPO_ID", "DEPO_ADI", yeni.DEPO_ID);

                ViewBag.ALT_DEPO_ID = new SelectList((from t2 in db.ALT_DEPO
                                                      where t2.STATU == true
                                                      select new
                                                      {
                                                          ALT_DEPO_ID = t2.ALT_DEPO_ID,
                                                          ALT_DEPO_ADI = t2.ALT_DEPO_ADI
                                                      }).Distinct().OrderBy(o => o.ALT_DEPO_ID), "ALT_DEPO_ID", "ALT_DEPO_ADI", yeni.ALT_DEPO_ID);
                return View(yeni);
            }

        }

        public ActionResult Edit(int? id)
        {
            ViewBag.DEPO_ID = new SelectList((from t1 in db.DEPO
                                              where t1.STATU == true
                                              select new
                                              {
                                                  DEPO_ID = t1.DEPO_ID,
                                                  DEPO_ADI = t1.DEPO_ADI
                                              }).Distinct().OrderBy(o => o.DEPO_ID), "DEPO_ID", "DEPO_ADI");

            ViewBag.ALT_DEPO_ID = new SelectList((from t2 in db.ALT_DEPO
                                                  where t2.STATU == true
                                                  select new
                                                  {
                                                      ALT_DEPO_ID = t2.ALT_DEPO_ID,
                                                      ALT_DEPO_ADI = t2.ALT_DEPO_ADI
                                                  }).Distinct().OrderBy(o => o.ALT_DEPO_ID), "ALT_DEPO_ID", "ALT_DEPO_ADI");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEPO_ESLESTIRME edited = db.DEPO_ESLESTIRME.Find(id);

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
        public ActionResult Edit(DEPO_ESLESTIRME edited)
        {
            bool isEdited = true;
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        DEPO_ESLESTIRME objectInDb = db.DEPO_ESLESTIRME.Where(w => w.DEPO_ESLESTIRME_ID != edited.DEPO_ESLESTIRME_ID && w.DEPO_ID == edited.DEPO_ID && w.ALT_DEPO_ID == edited.ALT_DEPO_ID).FirstOrDefault();
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
                ViewBag.DEPO_ID = new SelectList((from t1 in db.DEPO
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      DEPO_ID = t1.DEPO_ID,
                                                      DEPO_ADI = t1.DEPO_ADI
                                                  }).Distinct().OrderBy(o => o.DEPO_ID), "DEPO_ID", "DEPO_ADI",edited.DEPO_ID);

                ViewBag.ALT_DEPO_ID = new SelectList((from t2 in db.ALT_DEPO
                                                      where t2.STATU == true
                                                      select new
                                                      {
                                                          ALT_DEPO_ID = t2.ALT_DEPO_ID,
                                                          ALT_DEPO_ADI = t2.ALT_DEPO_ADI
                                                      }).Distinct().OrderBy(o => o.ALT_DEPO_ID), "ALT_DEPO_ID", "ALT_DEPO_ADI",edited.ALT_DEPO_ID);
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

            DEPO_ESLESTIRME silinecek = db.DEPO_ESLESTIRME.Find(id);

            if (silinecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                //Stok durum ve stok hareket tablosunda depo eşleştirme id ile ilgili herhangi bir kayıt var mı kontrolü
                STOK_DURUM stokDurum = db.STOK_DURUM.Where(w => w.DEPO_ESLESTIRME_ID == silinecek.DEPO_ESLESTIRME_ID).FirstOrDefault();
                STOK_HAREKET stokHareket = db.STOK_HAREKET.Where(w => w.DEPO_ESLESTIRME_ID == silinecek.DEPO_ESLESTIRME_ID).FirstOrDefault();

                if (stokDurum != null || stokHareket != null)
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
                    db.DEPO_ESLESTIRME.Remove(silinecek);
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