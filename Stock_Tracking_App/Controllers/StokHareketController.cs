using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockProject.Controllers
{
    public class StokHareketController : Controller
    {
        public StockEntities db = new StockEntities();
        // GET: StokHareket
        public ActionResult HareketEkle()
        {
            ViewBag.STOK_ID = new SelectList((from t1 in db.STOK
                                              where t1.STATU == true
                                              select new
                                              {
                                                  STOK_ID = t1.STOK_ID,
                                                  STOK_AD = t1.STOK_AD,
                                              }).Distinct().OrderBy(o => o.STOK_ID), "STOK_ID", "STOK_AD");

            ViewBag.HAREKET_TIP = new SelectList((from t1 in db.HAREKET_TIP
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      HAREKET_TIP_ID = t1.HAREKET_TIP_ID,
                                                      HAREKET_TIP_ADI = t1.HAREKET_TIP_ADI,

                                                  }).Distinct().OrderBy(o => o.HAREKET_TIP_ID), "HAREKET_TIP_ID", "HAREKET_TIP_ADI");

            ViewBag.DEPO_ESLESTIRME_ID = new SelectList((from t1 in db.DEPO_ESLESTIRME
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      DEPO_ESLESTIRME_ID = t1.DEPO_ESLESTIRME_ID,
                                                      DEPO_ESLESTIRME_ADI = t1.DEPO.DEPO_ADI + "/" + t1.ALT_DEPO.ALT_DEPO_ADI

                                                  }).Distinct().OrderBy(o => o.DEPO_ESLESTIRME_ID), "DEPO_ESLESTIRME_ID", "DEPO_ESLESTIRME_ADI");

            ViewBag.SORUMLU_ID = new SelectList((from t1 in db.SORUMLU
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      SORUMLU_ID = t1.SORUMLU_ID,
                                                      SORUMLU_ADI = t1.SORUMLU_ADI,

                                                  }).Distinct().OrderBy(o => o.SORUMLU_ID), "SORUMLU_ID", "SORUMLU_ADI");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult HareketEkle(STOK_HAREKET yeni)
        {
            var trans = db.Database.BeginTransaction();
            Boolean kaydedilecekMi = true;

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Database.Exists())
                    {
                        // Hareket tipi işletmeye çıkışsa stok durum tablosundan depo eşleştirmesindeki miktarı kontrol et

                        STOK_DURUM durum = db.STOK_DURUM.Where(w => w.STOK_ID == yeni.STOK_ID && w.DEPO_ESLESTIRME_ID == yeni.DEPO_ESLESTIRME_ID).FirstOrDefault();

                        // İşlem göstergesi

                        var islemGostergesi = (from t1 in db.HAREKET_TIP
                                               where t1.HAREKET_TIP_ID == yeni.HAREKET_TIP
                                               select new
                                               {
                                                   ISLEM_GOSTERGESI = t1.ISLEM_GOSTERGESI
                                               }).FirstOrDefault();

                        if (islemGostergesi.ISLEM_GOSTERGESI == false)
                        {

                            if (durum.DURUM_MIKTAR < yeni.HAREKET_MIKTAR)
                            {
                                kaydedilecekMi = false;

                                TempData["msg"] = "toastr.warning('" + "Yeterli miktarda stok bulunmamaktadır!"
                                                  + Convert.ToInt32(durum.DURUM_MIKTAR) + " miktarda çıkış yapabilirsiniz."
                                                  + "', '', {positionClass: 'md-toast-top-right'});"
                                                  + "$('#toast-container').attr('class','md-toast-top-right');";

                            }

                            else
                            {
                                var minMiktar = (from t1 in db.STOK
                                                 where t1.STOK_ID == yeni.STOK_ID
                                                 select new
                                                 {
                                                     MIN_MIKTAR = t1.MIN_MIKTAR
                                                 }).FirstOrDefault();


                                if ((durum.DURUM_MIKTAR - yeni.HAREKET_MIKTAR) < minMiktar.MIN_MIKTAR)
                                {

                                    kaydedilecekMi = false;

                                    TempData["msg"] = "toastr.warning('" + "Minimum miktarda stok hatası!"
                                                      + "', '', {positionClass: 'md-toast-top-right'});"
                                                      + "$('#toast-container').attr('class','md-toast-top-right');";
                                }


                                else
                                {
                                    kaydedilecekMi = true;

                                    // Stok hareket tablosunu güncelle
                                    yeni.HAREKET_TARIHI = DateTime.Now;
                                    yeni.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                    yeni.OLUSTURMA_TARIHI = DateTime.Now;
                                    db.STOK_HAREKET.Add(yeni);
                                    db.SaveChanges();

                                    // Stok durum tablosunu güncelle
                                    durum.DURUM_MIKTAR = durum.DURUM_MIKTAR - yeni.HAREKET_MIKTAR;
                                    durum.GUNCELLEYEN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                    durum.GUNCELLEME_TARIHI = DateTime.Now;
                                    db.Entry(durum).State = EntityState.Modified;
                                    db.SaveChanges();

                                    trans.Commit();

                                    TempData["msg"] = "toastr.success('" + "Kayıt Başarılı"
                                                      + "', '', {positionClass: 'md-toast-top-right'});"
                                                      + "$('#toast-container').attr('class','md-toast-top-right');";
                                }

                            }
                        }

                        // Malzeme ambarına girişse
                        else
                        {
                            kaydedilecekMi = true;

                            // Stok hareket tablosunu güncelle

                            yeni.HAREKET_TARIHI = DateTime.Now;
                            yeni.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                            yeni.OLUSTURMA_TARIHI = DateTime.Now;
                            db.STOK_HAREKET.Add(yeni);
                            db.SaveChanges();

                            if (durum != null)
                            {
                                // Stok durum tablosunu güncelle
                                durum.DURUM_MIKTAR = durum.DURUM_MIKTAR + yeni.HAREKET_MIKTAR;
                                durum.GUNCELLEYEN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                durum.GUNCELLEME_TARIHI = DateTime.Now;
                                db.Entry(durum).State = EntityState.Modified;
                                db.SaveChanges();

                                trans.Commit();

                            }

                            else
                            {
                                STOK_DURUM yeniDurum = new STOK_DURUM();

                                yeniDurum.STOK_ID = yeni.STOK_ID;
                                yeniDurum.DEPO_ESLESTIRME_ID = yeni.DEPO_ESLESTIRME_ID;
                                yeniDurum.DURUM_MIKTAR = yeni.HAREKET_MIKTAR;
                                yeniDurum.OLUSTURAN_KULLANICI = Convert.ToInt32(Session["KULLANICI_ID"]);
                                yeniDurum.OLUSTURMA_TARIHI = DateTime.Now;
                                db.STOK_DURUM.Add(yeniDurum);
                                db.SaveChanges();

                                trans.Commit();
                            }

                            TempData["msg"] = "toastr.success('" + "Kayıt Başarılı"
                                              + "', '', {positionClass: 'md-toast-top-right'});"
                                              + "$('#toast-container').attr('class','md-toast-top-right');";
                        }


                    }

                    else
                    {
                        kaydedilecekMi = false;

                        TempData["msg"] = "toastr.warning('" + "Veritabanı bağlantı hatası"
                                          + "', '', {positionClass: 'md-toast-top-right'});"
                                          + "$('#toast-container').attr('class','md-toast-top-right');";
                    }
                }
                catch (Exception ex)
                {
                    kaydedilecekMi = false;

                    TempData["msg"] = "toastr.warning('" + ex.Message
                                      + "', '', {positionClass: 'md-toast-top-right'});"
                                      + "$('#toast-container').attr('class','md-toast-top-right');";

                }
            }

            else
            {
                kaydedilecekMi = false;
            }

            // Sayfa Yönlendirmeleri

            if (kaydedilecekMi == true)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ViewBag.STOK_ID = new SelectList((from t1 in db.STOK
                                                  where t1.STATU == true
                                                  select new
                                                  {
                                                      STOK_ID = t1.STOK_ID,
                                                      STOK_AD = t1.STOK_AD,
                                                  }).Distinct().OrderBy(o => o.STOK_ID), "STOK_ID", "STOK_AD", yeni.STOK_ID);

                ViewBag.HAREKET_TIP = new SelectList((from t1 in db.HAREKET_TIP
                                                      where t1.STATU == true
                                                      select new
                                                      {
                                                          HAREKET_TIP_ID = t1.HAREKET_TIP_ID,
                                                          HAREKET_TIP_ADI = t1.HAREKET_TIP_ADI,
                                                      }).Distinct().OrderBy(o => o.HAREKET_TIP_ID), "HAREKET_TIP_ID", "HAREKET_TIP_ADI", yeni.HAREKET_TIP);

                ViewBag.DEPO_ESLESTIRME_ID = new SelectList((from t1 in db.DEPO_ESLESTIRME
                                                             where t1.STATU == true
                                                             select new
                                                             {
                                                                 DEPO_ESLESTIRME_ID = t1.DEPO_ESLESTIRME_ID,
                                                                 DEPO_ESLESTIRME_ADI = t1.DEPO.DEPO_ADI + " / " + t1.ALT_DEPO.ALT_DEPO_ADI,
                                                             }).Distinct().OrderBy(o => o.DEPO_ESLESTIRME_ID), "DEPO_ESLESTIRME_ID", "DEPO_ESLESTIRME_ADI", yeni.DEPO_ESLESTIRME_ID);

                ViewBag.SORUMLU_ID = new SelectList((from t1 in db.SORUMLU
                                                     where t1.STATU == true
                                                     select new
                                                     {
                                                         SORUMLU_ID = t1.SORUMLU_ID,
                                                         SORUMLU_ADI = t1.SORUMLU_ADI,
                                                     }).Distinct().OrderBy(o => o.SORUMLU_ID), "SORUMLU_ID", "SORUMLU_ADI", yeni.SORUMLU_ID);
                trans.Rollback();
                return View();
            }
        }
    }
}