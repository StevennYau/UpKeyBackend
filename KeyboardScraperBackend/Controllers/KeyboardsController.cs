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

        [HttpGet("~/getAllKeyboards")]
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
            return Ok(_keyboardServices.GetKeyboardEbayId(EbayId));
        }

        [HttpPost]
        public IActionResult AddKeyboard(EbayKeyboard kb)
        {
            _keyboardServices.AddKeyboard(kb);
            return CreatedAtRoute("GetKeyboardEbayId", new { EbayId = kb.EbayId }, kb);
        }

        /*[HttpDelete("{id}")]
        public IActionResult DeleteKeyboard(string id)
        {
            _keyboardServices.DeleteKeyboard(id);
            return NoContent();
        }*/
        
        [HttpDelete("{EbayId}")]
        public IActionResult DeleteKeyboardEbayId(string EbayId)
        {
            _keyboardServices.DeleteKeyboardEbayId(EbayId);
            return NoContent();
        }

        /*[HttpPut]
        public IActionResult UpdateBook(EbayKeyboard kb)
        {
            return Ok(_keyboardServices.UpdateKeyboard(kb));
        }*/

        [HttpPut]
        public IActionResult StoreAndUpdate(List<EbayKeyboard> KbList)
        {
            return Ok(_keyboardServices.StoreAndUpdate(KbList));
        }

        [HttpGet("~/getScrapedData")]
        public IActionResult GetScrapedData()
        {
            return Ok(_keyboardServices.GetScrapedData().Result);
        }

    }
}