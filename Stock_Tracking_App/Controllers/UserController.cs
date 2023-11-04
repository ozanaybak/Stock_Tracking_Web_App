using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Design;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace stockProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        StockEntities db = new StockEntities();
        public ActionResult Index()
        {
            return View(db.KULLANICI.ToList());
        }

        public ActionResult Create()
        {

            ViewBag.KUL_TIP = new SelectList((from t1 in db.KULLANICI_TIP
                                              where t1.STATU == true
                                              select new
                                              {
                                                  KULTIP_ID = t1.KULTIP_ID,
                                                  KULTIP_ADI = t1.KULTIP_ADI
                                              }).Distinct().OrderBy(o => o.KULTIP_ID), "KULTIP_ID", "KULTIP_ADI"); ;
            return View();
        }
        [HttpPost]
        public ActionResult Create(KULLANICI yeni)
        {
            bool kaydedilecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.KULLANICI.Where(w => w.KUL_USERNAME == yeni.KUL_USERNAME).FirstOrDefault();

                        if (objectInDb != null)


                        {
                            kaydedilecekMi = false;

                            switch (objectInDb.STATU)
                            {

                                case true:
                                    TempData["msg"] = "toastr.warning('" +
                            "There is an inactive record exist." +
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
                            yeni.OLUSTURAN_KULLANICI = Session["KUL_AD"].ToString(); //şimdilik admin kullanıcısının idsi
                            db.KULLANICI.Add(yeni);
                            db.Database.Log = Console.Write;
                            db.SaveChanges();

                            TempData["msg"] = "toastr.warning('" +
                            "Creation successfull." +
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
                ViewBag.KUL_TIP = new SelectList((from t1 in db.KULLANICI_TIP
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      KULTIP_ID = t1.KULTIP_ID,
                                                      KULTIP_ADI = t1.KULTIP_ADI
                                                  }).Distinct().OrderBy(o => o.KULTIP_ID), "KULTIP_ID", "KULTIP_ADI", yeni.KUL_TIP);
                return View(yeni);
            }

        }
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KULLANICI edited = db.KULLANICI.Find(id);

            if (edited == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.KUL_TIP = new SelectList((from t1 in db.KULLANICI_TIP
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      KULTIP_ID = t1.KULTIP_ID,
                                                      KULTIP_ADI = t1.KULTIP_ADI
                                                  }).Distinct().OrderBy(o => o.KULTIP_ID), "KULTIP_ID", "KULTIP_ADI", edited.KUL_TIP);
                return View(edited);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KULLANICI edited)
        {
            bool isEdited = true;
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var matchingDepoEslestirme = db.DEPO_ESLESTIRME.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();
                        var matchingDepo = db.DEPO.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();
                        var matchingAltDepo = db.ALT_DEPO.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();
                        var matchingStok = db.STOK.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();
                        var statusStok = db.STOK_DURUM.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();
                        var movementStok = db.STOK_HAREKET.Where(w => w.OLUSTURAN_KULLANICI == edited.KULLANICI_ID || w.GUNCELLEYEN_KULLANICI == edited.KULLANICI_ID).Count();

                        var toplam = matchingAltDepo + matchingDepo + statusStok + movementStok + matchingDepoEslestirme + matchingStok;

                        if (toplam != 0)
                        {
                            isEdited = false;
                            TempData["msg"] = TempData["msg"] = "toastr.warning('" +
                            "There is an active matched record exist." +
                            "', '', {positionClass: 'md-toast-top-right'});" +
                            "$('#toast-container').attr('class','md-toast-top-right');";
                        }
                        else
                        {
                            KULLANICI objectInDb = db.KULLANICI.Where(w => w.KULLANICI_ID != edited.KULLANICI_ID && w.KUL_USERNAME == edited.KUL_USERNAME).FirstOrDefault();
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
                                edited.STATU = true;
                                edited.GUNCELLEYEN_KULLANICI = (string)Session["KUL_AD"];
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
                ViewBag.KUL_TIP = new SelectList((from t1 in db.KULLANICI_TIP
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      KULTIP_ID = t1.KULTIP_ID,
                                                      KULTIP_ADI = t1.KULTIP_ADI
                                                  }).Distinct().OrderBy(o => o.KULTIP_ID), "KULTIP_ID", "KULTIP_ADI", edited.KUL_TIP);
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

            KULLANICI silinecek = db.KULLANICI.Find(id);

            if (silinecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                //Depo eşleştirme tablosunda kullanıcı id ile ilgili herhangi bir kayıt var mı kontrolü
                var altDepo = db.ALT_DEPO.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var depo = db.DEPO.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var depoEslestirme = db.DEPO_ESLESTIRME.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var stokDurum = db.STOK_DURUM.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var sorumlu = db.SORUMLU.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var stokHareket = db.STOK_HAREKET.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();
                var stok = db.STOK.Where(w => w.GUNCELLEYEN_KULLANICI == silinecek.KULLANICI_ID || w.OLUSTURAN_KULLANICI == silinecek.KULLANICI_ID).Count();

                var toplam = altDepo + depo + depoEslestirme + stokDurum + sorumlu + stokHareket + stok;


                if (toplam != 0)
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
                    db.KULLANICI.Remove(silinecek);
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
