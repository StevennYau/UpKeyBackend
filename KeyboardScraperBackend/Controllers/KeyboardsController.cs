using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyboardsLibrary.Core;
using KeyboardsLibrary.Core.Entity;
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

        /*[HttpGet("{id}", Name = "GetKeyboard")]
        public IActionResult GetKeyboard(string id)
        {
           return Ok(_keyboardServices.getKeyboard(id));
            
        }*/
        
        [HttpGet("{EbayId}", Name = "GetKeyboardEbayId")]
        public IActionResult GetKeyboardEbayId(string EbayId)
        {
            return Ok(_keyboardServices.getKeyboardEbayId(EbayId));
        }

        [HttpPost]
        public IActionResult AddKeyboard(EbayKeyboard kb)
        {
            _keyboardServices.AddKeyboard(kb);
            return CreatedAtRoute("GetKeyboardEbayId", new { EbayId = kb.EbayId }, kb);
        }

    }
}