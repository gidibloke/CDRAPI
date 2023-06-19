using Domain.LookupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CallRecord
    {
        public long Id { get; set; }
        public string CallerId { get; set; }
        public string Recipient { get; set; }
        public DateTime? CallDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Duration { get; set; }

        //decimal over float for money calculations
        public decimal? Cost { get; set; }
        public string Reference { get; set; }
        public int? Currency { get; set; }
        public virtual Currency Currencys { get; set; }


    }
}
