using System.Collections.Generic;
using KeyboardsLibrary.Core.Entity;

namespace KeyboardsLibrary.Core
{
    public interface IKeyboardServices
    {
        List<EbayKeyboard> GetKeyboardsEbay();
    }
}