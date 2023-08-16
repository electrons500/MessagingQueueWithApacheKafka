using ApacheKafkaConsumerDemo.Model;
using ApacheKafkaConsumerDemo.Service;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ApacheKafkaConsumerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
       
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

    }
}
