using Microsoft.Ajax.Utilities;
using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace stockProject.Helper
{
    public class mandatoryParameterDefiniton
    {
            public static void adminDefinition()
        {
            StockEntities db = new StockEntities();
            KULLANICI user = db.KULLANICI.Where(w => w.KUL_USERNAME == "admin").FirstOrDefault();

            if (user == null)
            {
                KULLANICI admin = new KULLANICI();

                admin.KUL_USERNAME = "admin";
                admin.KUL_AD = "admin";
                admin.KUL_SOYAD = "admin";
                admin.KUL_SIFRE = "123";
                admin.KUL_TIP = 1;
                admin.STATU = true;
                admin.OLUSTURAN_KULLANICI = admin.KUL_USERNAME;
                admin.OLUSTURMA_TARIHI = DateTime.Now;

                db.KULLANICI.Add(admin);
                db.SaveChanges();

                
                


            }
        }

        public static void StoreManager()
        {
            StockEntities db = new StockEntities();
            KULLANICI user = db.KULLANICI.Where(w => w.KUL_USERNAME == "depoadmin").FirstOrDefault();

            if (user == null)
            {
                KULLANICI admin = new KULLANICI();

                admin.KUL_USERNAME = "depoadmin";
                admin.KUL_AD = "DepoYetkilisi";
                admin.KUL_SOYAD = "DepoYetkilisi";
                admin.KUL_SIFRE = "123";
                admin.KUL_TIP = 2;
                admin.STATU = true;
                admin.OLUSTURAN_KULLANICI = admin.KUL_USERNAME;
                admin.OLUSTURMA_TARIHI = DateTime.Now;

                db.KULLANICI.Add(admin);
                db.SaveChanges();





            }
        }

        public static void ReportUser()
        {
            StockEntities db = new StockEntities();
            KULLANICI user = db.KULLANICI.Where(w => w.KUL_USERNAME == "raporadmin").FirstOrDefault();

            if (user == null)
            {
                KULLANICI admin = new KULLANICI();

                admin.KUL_USERNAME = "raporadmin";
                admin.KUL_AD = "RaporKullanıcısı";
                admin.KUL_SOYAD = "RaporKullanıcısı";
                admin.KUL_SIFRE = "123";
                admin.KUL_TIP = 3;
                admin.STATU = true;
                admin.OLUSTURAN_KULLANICI = admin.KUL_USERNAME;
                admin.OLUSTURMA_TARIHI = DateTime.Now;

                db.KULLANICI.Add(admin);
                db.SaveChanges();





            }
        }


    }
}