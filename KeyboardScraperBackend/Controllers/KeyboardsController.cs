using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyboardsLibrary.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KeyboardScraperBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyboardsController : ControllerBase
    {

        private readonly IKeyboardServices _keyboardServices;
        public KeyboardsController(IKeyboardServices keyboardServices)
        {
            _keyboardServices = keyboardServices;
        }

        [HttpGet]
        public IActionResult GetKeyboardsEbay()
        {
            return Ok(_keyboardServices.GetKeyboardsEbay());
        }
    }
}