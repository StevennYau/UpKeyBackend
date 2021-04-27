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

        public EbayKeyboard AddKeyboard(EbayKeyboard kb)
        {
            _keyboards.InsertOne(kb);
            return kb;
        }

        public EbayKeyboard GetKeyboard(string id)
        {
            return _keyboards.Find(kb => kb.Id == id).First();
        }

        public EbayKeyboard GetKeyboardEbayId(string id)
        {
            return _keyboards.Find(kb => kb.EbayId == id).First();
        }

        public void DeleteKeyboard(string id)
        {
            _keyboards.DeleteOne(kb => kb.Id == id);
        }

        public void DeleteKeyboardEbayId(string id)
        {
            _keyboards.DeleteOne(kb => kb.EbayId == id);
        }

        public EbayKeyboard UpdateKeyboard(EbayKeyboard kb)
        {
            GetKeyboardEbayId(kb.EbayId);
            _keyboards.ReplaceOne(kbs => kbs.EbayId == kb.EbayId, kb);
            return kb;
        }

        public List<EbayKeyboard> StoreAndUpdate(List<EbayKeyboard> KbList)
        {
            foreach (var kb in KbList)
            {
                EbayKeyboard compare = _keyboards.Find(keyb => keyb.EbayId == kb.EbayId).FirstOrDefault();

                if (compare == null)
                {
                    Console.WriteLine("DOENST EXIST SO POST");
                    Console.WriteLine(kb.Name);
                    _keyboards.InsertOne(kb);
                }
                else // update since compare was found
                {
                    Console.WriteLine("UPDATE");
                    Console.WriteLine(compare.Name);
                    Console.WriteLine(kb.Name);
                    Console.WriteLine(compare.EbayId);
                    Console.WriteLine(kb.EbayId);
                    kb.Id = compare.Id;
                    _keyboards.ReplaceOne(kbs => kbs.EbayId == kb.EbayId, kb);
                }
            }

            return KbList;
        }
    }
}