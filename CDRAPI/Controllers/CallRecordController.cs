using CDRAPI.DTOs;
using CDRAPI.Helpers;
using CDRAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace CDRAPI.Controllers
{
    public class CallRecordController : BaseController
    {
        private readonly IBackgroundQueue<RequestInformation> _backgroundQueue;
        private readonly IAppRepository _appRepository;

        public CallRecordController(IBackgroundQueue<RequestInformation> backgroundQueue, IAppRepository appRepository)
        {
            _backgroundQueue = backgroundQueue;
            _appRepository = appRepository;
        }

        [HttpPost("upload")]
        public IActionResult UploadCSV()
        {
            //Status code 202 to show request accepted. anything from an email to a notification (implemented using SignalR) can help indicate when the upload is done
            //using request variable can be extended to contain meta data about the user who initiated the process and used to send out notification when process is done
            var request = new RequestInformation();
            _backgroundQueue.Enqueue(request);
            return Accepted();

        }

        [HttpPost("averagecallcost")]
        public async Task<ActionResult> AverageCallCost(AverageCall call)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.AverageCallCost(call);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new {error = ModelState.Values.SelectMany(x => x.Errors)});
        }

        [HttpPost("averageCallDuration")]
        public async Task<ActionResult> AverageCallDuration(SingleDate single)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.AverageCallDuration(single);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new { error = ModelState.Values.SelectMany(x => x.Errors) });
        }


        [HttpPost("averageNumberOfCalls")]
        public async Task<ActionResult> AverageNumberOfCalls(AverageCall call)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.AverageNumberOfCalls(call);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new { error = ModelState.Values.SelectMany(x => x.Errors) });
        }

        [HttpPost("highestCallDuration")]
        public async Task<ActionResult> HighestCallDuration (SingleDate single)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.HighestCallDuration(single);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new { error = ModelState.Values.SelectMany(x => x.Errors) });
        }

        [HttpPost("numberOfCalls")]
        public async Task<ActionResult> NumberOfCalls(AverageCall averageCall)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.NumberOfCalls(averageCall);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new { error = ModelState.Values.SelectMany(x => x.Errors) });
        }

        [HttpPost("numberOfCallsByCaller")]
        public async Task<ActionResult> NumberOfCallsByCaller(Caller caller)
        {
            if (ModelState.IsValid)
            {
                var result = await _appRepository.NumberOfCallsByCaller(caller);
                return Ok(result);
            }
            throw new APIException(HttpStatusCode.BadRequest, new { error = ModelState.Values.SelectMany(x => x.Errors) });
        }
    }
}
