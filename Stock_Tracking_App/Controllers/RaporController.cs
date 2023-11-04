using stockProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockProject.Controllers
{
    public class RaporController : Controller
    {
        // GET: Rapor
        StockEntities db = new StockEntities();
        public ActionResult StokHareketRapor()
        {
            return View(db.STOK_HAREKET.ToList());
        }
    }
}