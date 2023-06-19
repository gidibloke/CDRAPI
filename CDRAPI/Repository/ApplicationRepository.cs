using CDRAPI.Data;
using CDRAPI.DTOs;
using CDRAPI.Helpers;
using CDRAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CDRAPI.Repository
{
    public class ApplicationRepository : IAppRepository
    {
        //Using repository to keep it simple and i believe in slim controllers (controllers should only accept request but pass on request to appropiate service)
        //For enterprise applications, will prefer to the CQRS pattern and have a seperate project for Application logic
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SingleResponse> AverageCallCost(AverageCall averageCall)
        {
            //Add one day so you can get all the calls for the end day
            var endDate = averageCall.ToDate.Value.AddDays(1);
            var calls = await _context.CallRecords
                .Where(x => x.CallDate > averageCall.FromDate && x.CallDate < endDate).Select(x => x.Cost).ToListAsync();
            if(calls.Count != 0)
            {
                var average = Math.Round(calls.Average().Value, 3);
                return new SingleResponse(average.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new { Error = "No calls in the chosen duration" });
        }

        public async Task<SingleResponse> AverageCallDuration(SingleDate singleDate)
        {
            var calls = await _context.CallRecords.Where(x => x.CallDate == singleDate.Date).Select(x => x.Duration).ToListAsync();
            if(calls.Count != 0)
            {
                var average = Math.Round(calls.Average().Value, 3);
                return new SingleResponse(average.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new {Error = "No calls in the chosen duration"});
        }

        public async Task<SingleResponse> AverageNumberOfCalls(AverageCall averageCall)
        {
            var endDate = averageCall.ToDate.Value.AddDays(1);
            var numberOfDays = (averageCall.ToDate - averageCall.FromDate).Value.Days;
            var calls = await _context.CallRecords.
                Where(x => x.CallDate > averageCall.FromDate && x.CallDate < endDate).CountAsync();
            if(calls != 0)
            {
                var average = Math.Round((double)calls / numberOfDays, 3);
                return new SingleResponse(average.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new { Error = "No calls in the chosen duration" });
        }

        public async Task<SingleResponse> HighestCallDuration(SingleDate singleDate)
        {
            var calls = await _context.CallRecords
                .Where(x => x.CallDate == singleDate.Date).MaxAsync(x => x.Duration) ?? 0;
            if(calls != 0)
            {
                return new SingleResponse(calls.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new { Error = "No calls in the chosen duration" });
        }

        public async Task<SingleResponse> NumberOfCalls(AverageCall averageCall)
        {
            var endDate = averageCall.ToDate.Value.AddDays(1);
            var calls = await _context.CallRecords
                                .Where(x => x.CallDate > averageCall.FromDate && x.CallDate < endDate).CountAsync();
            if (calls != 0)
            {
                return new SingleResponse(calls.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new { Error = "No calls in the chosen duration" });
        }

        public async Task<SingleResponse> NumberOfCallsByCaller(Caller caller)
        {
            var query = _context.CallRecords.Where(x => x.CallerId == caller.CallerId).AsQueryable();
            if(caller.Date != null)
            {
                query = query.Where(x => x.CallDate ==caller.Date);
            }
            var calls = await query.CountAsync();
            if(calls != 0)
            {
                return new SingleResponse(calls.ToString());
            }
            throw new APIException(HttpStatusCode.BadRequest, new { Error = "No calls in the chosen duration" });
        }
    }
}
