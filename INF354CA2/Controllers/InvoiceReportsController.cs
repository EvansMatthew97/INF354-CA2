using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INF354CA2.Models;
using INF354CA2.Models.ViewModels;

namespace INF354CA2.Controllers
{
    public class InvoiceReportsController : Controller
    {
        private HardwareDBEntities db = new HardwareDBEntities();

        // GET: InvoiceReports
        public ActionResult Index(DateTime? startDate = null, DateTime? endDate = null, string groupBy = null)
        {
            // get the minimum and maximum dates from the invoices, to allow the user to select from range of dates
            var orderedDates = db.lginvoices.OrderBy(i => i.inv_DATETIME).ToList();
            ViewBag.minDate = (DateTime)orderedDates.First().inv_DATETIME;
            ViewBag.maxDate = (DateTime)orderedDates.Last().inv_DATETIME;

            // if the user hasn't specified start or end dates, set the date range to a default of all time
            if (startDate == null) startDate = ViewBag.minDate;
            if (endDate == null) endDate = ViewBag.maxDate;

            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.groupBy = groupBy;

            var productTotals = db.lgproducts.Select(product => new InvoiceProductTotal
            {
                product = product,
                total = product.lglines
                    .Where(line => line.lginvoice.inv_DATETIME >= startDate && line.lginvoice.inv_DATETIME <= endDate)
                    .Sum(line => line.lginvoice.inv_total),
            });

            var userDefinedGrouping = productTotals.GroupBy(g =>
                groupBy == "prod_type" ? g.product.prod_type : groupBy == "prod_base" ? g.product.prod_base : groupBy == "prod_category" ? g.product.prod_category : g.product.lgbrand.brand_name
            ).ToList();

            var breakPointedGroups = userDefinedGrouping
                .Select(g => new ProductGroup { title = g.Key, products = g.OrderByDescending(p => p.total).ToList(), total = (decimal)g.Sum(p => p.total) })
                .OrderByDescending(g => g.total)
                .ToList();

            InvoiceReport report = new InvoiceReport {
                groups = breakPointedGroups,
                xAxis = breakPointedGroups.Select(g => g.title).ToList(),
                yAxis = breakPointedGroups.Select(g => g.total).ToList()
            };
            return View(report);
        }
    }
}