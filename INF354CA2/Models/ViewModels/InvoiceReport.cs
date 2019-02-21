using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INF354CA2.Models.ViewModels
{
    public class InvoiceReport
    {
        public List<ProductGroup> groups;
        public List<string> xAxis;
        public List<decimal> yAxis;
        public List<ChartItem> sales;
    }
}