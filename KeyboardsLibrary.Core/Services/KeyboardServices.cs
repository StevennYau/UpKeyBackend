using System;
using System.Collections.Generic;
using KeyboardsLibrary.Core.Entity;

namespace KeyboardsLibrary.Core
{
    public class KeyboardServices : IKeyboardServices
    {
        public List<EbayKeyboard> GetKeyboardsEbay()
        {
            return new List<EbayKeyboard>
            {
                new EbayKeyboard
                {
                    Name = "test",
                    Price = 12.99
                }
            };
        }
    }
}