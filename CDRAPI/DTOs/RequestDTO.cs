using System.ComponentModel.DataAnnotations;

namespace CDRAPI.DTOs
{

    //Average number of calls
    //average call cost
    //Number of calls made
    public class AverageCall
    {
        //[Required(ErrorMessage = "From date is required")]
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    //Longest call
    //Average call duration in date
    //Highest call cost in day
    public class SingleDate
    {
        public DateTime? Date { get; set; }
    }

    //Number of calls make by caller in a day
    public class Caller
    {
        public string CallerId { get; set; }
        public DateTime? Date { get; set; }
    }



}
