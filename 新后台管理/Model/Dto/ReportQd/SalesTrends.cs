using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.ReportQd
{
    public class SalesTrends
    {
        public string qdid { get; set; }
        public string qdmc { get; set; }

        public int month { get; set; }

        public decimal money { get; set; }

    }
}
