using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ServerWebHook.Models;

namespace ServerWebHook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        private static List<Client> clients = new List<Client>();


        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult RegisterClient([FromQuery] string clientUrl)
        {

            if (!clients.Any(x=>x.URL==clientUrl))
            {
                clients.Add(new Client() {URL=clientUrl});
            }
            else
            {
                return BadRequest("URL already exists");
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Webhook(string evento, string port)
        {


            foreach (var client in clients)
            {
                if (!client.URL.Contains(port)) 
                {
                    continue;
                }

                HttpClient httpclient = new HttpClient();

                httpclient.PostAsJsonAsync(client.URL, evento);
                
            }

            return Ok();
        }
    }
}
