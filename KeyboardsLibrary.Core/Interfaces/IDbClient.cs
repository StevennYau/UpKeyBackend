using KeyboardsLibrary.Core.Entity;
using MongoDB.Driver;

namespace KeyboardsLibrary.Core
{
    public interface IDbClient
    {
        IMongoCollection<EbayKeyboard> GetKeyboardCollection();
    }
}