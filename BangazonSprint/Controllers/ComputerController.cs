using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BangazonSprintStartUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        // GET: api/Computer
        [HttpGet]
        public IEnumerable<string> GetAllComputers()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Computer/5
        [HttpGet("{id}", Name = "Computer")]
        public string GetComputer(int id)
        {
            return "value";
        }

        // POST: api/Computer
        [HttpPost]
        public void PostComputer([FromBody] string value)
        {
        }

        // PUT: api/Computer/5
        [HttpPut("{id}")]
        public void PutComputer(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteComputer(int id)
        {
        }
    }
}
