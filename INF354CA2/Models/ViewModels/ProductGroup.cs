using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INF354CA2.Models;

namespace INF354CA2.Models.ViewModels
{
    public class ProductGroup
    {
        public string title;
        public List<InvoiceProductTotal> products;
        public decimal total;
    }
}