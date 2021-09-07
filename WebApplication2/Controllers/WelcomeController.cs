using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    
    [Route("/api/")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
      
        [HttpGet]
        [Route("welcome")]
        public IActionResult Welcome()
        {
            return Ok("Queo com tu AY ÉT PI ĐÓT NÉT CO");
        }
    }
}
