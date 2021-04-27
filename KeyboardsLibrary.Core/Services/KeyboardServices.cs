using System;
using System.Collections.Generic;
using KeyboardsLibrary.Core.Entity;
using MongoDB.Driver;

namespace KeyboardsLibrary.Core
{
    public class KeyboardServices : IKeyboardServices
    {
        private readonly IMongoCollection<EbayKeyboard> _keyboards;
        public KeyboardServices(IDbClient dbClient)
        {
            _keyboards = dbClient.GetKeyboardCollection();
        }
        public List<EbayKeyboard> GetKeyboardsEbay()
        {
            return _keyboards.Find(keyboard => true).ToList();
        }
    }
}