using CDRAPI.DTOs;

namespace CDRAPI.Interfaces
{
    //Average number of calls
    //average call cost
    //Number of calls made 

    //Longest call
    //Average call in date
    //Highest call cost in day
    //Number of calls make by caller in a day
    public interface IAppRepository
    {
        Task<SingleResponse> AverageNumberOfCalls(AverageCall averageCall);
        Task<SingleResponse> AverageCallCost(AverageCall averageCall);
        Task<SingleResponse> NumberOfCalls(AverageCall averageCall);
        Task<SingleResponse> AverageCallDuration(SingleDate singleDate);
        Task<SingleResponse> HighestCallDuration(SingleDate singleDate);
        Task<SingleResponse> NumberOfCallsByCaller(Caller caller);

    }
}
