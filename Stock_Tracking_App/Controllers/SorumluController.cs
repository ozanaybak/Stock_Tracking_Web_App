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
    public class SorumluController : Controller
    {
        // GET: Sorumlu
        // GET: DEPO
        StockEntities db = new StockEntities();

        public ActionResult Index()
        {
            return View(db.SORUMLU.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SORUMLU yeni)
        {
            bool kaydedilecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        var objectInDb = db.SORUMLU.Where(w => w.SORUMLU_ADI == yeni.SORUMLU_ADI).FirstOrDefault();

                        if (objectInDb != null)


                        {
                            kaydedilecekMi = false;

                            switch (objectInDb.STATU)
                            {

                                case true:
                                    TempData["msg"] = "toastr.warning('" + "Statüsü aktif olan bir kayıt vardır!"
                                                       + "', '', {positionClass: 'md-toast-top-right'});"
                                                       + "$('#toast-container').attr('class','md-toast-top-right');";
                                    break;

                                case false:
                                    TempData["msg"] = "toastr.warning('" + "Statüsü pasif olan bir kayıt vardır, kaydı düzenleyin!"
                                                       + "', '', {positionClass: 'md-toast-top-right'});"
                                                       + "$('#toast-container').attr('class','md-toast-top-right');";


                                    break;

                            }
                        }

                        //kaydet
                        if (kaydedilecekMi == true)
                        {


                            yeni.STATU = true;
                            yeni.OLUSTURMA_TARIHI = DateTime.Now;
                            yeni.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                            db.SORUMLU.Add(yeni);
                            db.SaveChanges();

                            TempData["msg"] = "toastr.success('" + "Kayıt Başarılı"
                                               + "', '', {positionClass: 'md-toast-top-right'});"
                                               + "$('#toast-container').attr('class','md-toast-top-right');";
                        }



                    }

                    else
                    {
                        kaydedilecekMi = false;
                        TempData["msg"] = "toastr.danger('" + "Veritabanı bağlantı hatası!"
                                          + "', '', {positionClass: 'md-toast-top-right'});"
                                          + "$('#toast-container').attr('class','md-toast-top-right');";
                    }

                }
                catch (Exception ex)
                {
                    kaydedilecekMi = false;
                    TempData["msg"] = "toastr.danger('" + ex.Message
                                      + "', '', {positionClass: 'md-toast-top-right'});"
                                      + "$('#toast-container').attr('class','md-toast-top-right');";
                }


            }

            else
            {
                kaydedilecekMi = false;
            }

            if (kaydedilecekMi == true)
            {
                return RedirectToAction("Index");
            }

            else
            {
                return View(yeni);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //HTTP 400 hatası
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SORUMLU duzenlenecek = db.SORUMLU.Find(id);

            if (duzenlenecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(duzenlenecek);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(SORUMLU duzenlenmis)
        {
            bool duzenlenecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        //Depo eşleştirmede kaydı var mı kontrolü
                        STOK_HAREKET eslestirme = db.STOK_HAREKET.Where(w => w.SORUMLU_ID == duzenlenmis.SORUMLU_ID).FirstOrDefault();

                        if (eslestirme != null)
                        {
                            duzenlenecekMi = false;
                            TempData["msg"] = "toastr.warning('" + "Depo eşleştirmede depoya bağlı kayıt olduğu için düzenleme yapılamaz!"
                                                       + "', '', {positionClass: 'md-toast-top-right'});"
                                                       + "$('#toast-container').attr('class','md-toast-top-right');";
                        }

                        else
                        {
                            //düzenlenmiş isimde, kendinden farklı id ile veritabanında bir kayıt var mı
                            SORUMLU objectInDb = db.SORUMLU.Where(w => w.SORUMLU_ADI == duzenlenmis.SORUMLU_ADI && w.SORUMLU_ID == duzenlenmis.SORUMLU_ID).FirstOrDefault();

                            if (objectInDb != null)
                            {
                                duzenlenecekMi = false;

                                switch (objectInDb.STATU)
                                {

                                    case true:
                                        TempData["msg"] = "toastr.warning('" + "Statüsü aktif olan bir kayıt vardır!"
                                                           + "', '', {positionClass: 'md-toast-top-right'});"
                                                           + "$('#toast-container').attr('class','md-toast-top-right');";
                                        break;

                                    case false:
                                        TempData["msg"] = "toastr.warning('" + "Statüsü pasif olan bir kayıt vardır, kaydı düzenleyin!"
                                                           + "', '', {positionClass: 'md-toast-top-right'});"
                                                           + "$('#toast-container').attr('class','md-toast-top-right');";


                                        break;

                                }
                            }
                            if (duzenlenecekMi == true)
                            {
                                duzenlenmis.GUNCELLEYEN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                duzenlenmis.GUNCELLEME_TARIHI = DateTime.Now;
                                db.Entry(duzenlenmis).State = EntityState.Modified;
                                db.SaveChanges();

                                TempData["msg"] = "toastr.success('" + "Güncelleme Başarılı!"
                                            + "', '', {positionClass: 'md-toast-top-right'});"
                                            + "$('#toast-container').attr('class','md-toast-top-right');";


                            }

                        }
                    }

                    else
                    {
                        duzenlenecekMi = false;
                        TempData["msg"] = "toastr.danger('" + "Veritabanı bağlantı hatası!"
                                          + "', '', {positionClass: 'md-toast-top-right'});"
                                          + "$('#toast-container').attr('class','md-toast-top-right');";
                    }

                }

                catch (Exception ex)
                {
                    duzenlenecekMi = false;
                    TempData["msg"] = "toastr.danger('" + ex.Message
                                      + "', '', {positionClass: 'md-toast-top-right'});"
                                      + "$('#toast-container').attr('class','md-toast-top-right');";
                }
            }

            else
            {
                duzenlenecekMi = false;
            }

            //sayfa yönlendirmeleri

            if (duzenlenecekMi == true)
            {
                return RedirectToAction("Index");
            }

            else
            {
                return View(duzenlenmis);
            }


        }

        public ActionResult Delete(int? id)
        {
            bool silinecekMi = true;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SORUMLU silinecek = db.SORUMLU.Find(id);

            if (silinecek == null)
            {
                return HttpNotFound();
            }

            else
            {
                //Depo eşleştirme tablosunda depo id ile ilgili herhangi bir kayıt var mı kontrolü
                STOK_HAREKET eslestirme = db.STOK_HAREKET.Where(w => w.SORUMLU_ID == silinecek.SORUMLU_ID).FirstOrDefault();

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
                    db.SORUMLU.Remove(silinecek);
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