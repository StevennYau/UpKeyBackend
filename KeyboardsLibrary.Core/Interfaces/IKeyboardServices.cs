using System.Collections.Generic;
using System.Threading.Tasks;
using KeyboardsLibrary.Core.Entity;

namespace KeyboardsLibrary.Core
{
    public interface IKeyboardServices
    {
        List<EbayKeyboard> GetKeyboardsEbay();
        EbayKeyboard AddKeyboard(EbayKeyboard kb);

        EbayKeyboard GetKeyboard(string id);

        EbayKeyboard GetKeyboardEbayId(string id);

        void DeleteKeyboard(string id);
        void DeleteKeyboardEbayId(string id);

        EbayKeyboard UpdateKeyboard(EbayKeyboard kb);

        List<EbayKeyboard> StoreAndUpdate(List<EbayKeyboard> KbList);

        Task<List<EbayKeyboard>> GetScrapedData();
    }
}