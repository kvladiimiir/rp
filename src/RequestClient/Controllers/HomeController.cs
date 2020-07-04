using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackendApi;
using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RequestClient.Models;

namespace RequestClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendRequestAsync(string requestDescription, string taskData)
        {
            if (requestDescription == null) 
            {
                ViewBag.TaskResult = "Отправлен пустой запрос";
                return View();
            }

            string taskResult = "";

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Job.JobClient(channel); 
            var reply = await client.RegisterAsync(
                            new RegisterRequest { 
                                Description = requestDescription,
                                Data = taskData 
                            });
            taskResult = reply.Id;
            ViewBag.TaskResult = taskResult;

            return RedirectToAction( "TextAnalyzer", new { id = taskResult } );
        }

        public async Task<IActionResult> TextAnalyzerAsync(string id)
        {
            string status = String.Empty;

            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Job.JobClient(channel);
            var reply = await client.GetProcessingResultAsync( 
                new GetProcessingResultRequest { 
                    Id = id 
                    });

            ViewBag.Status = reply.Status;
            ViewBag.ResultRanc = reply.Ranc;

            return View();
        }
    }
}
