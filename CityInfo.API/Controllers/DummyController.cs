using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/testdatabase")]
    public class DummyController :Controller
    {
        private CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public IActionResult TestDataBase()
        {
            return Ok();
        }
    }
}
