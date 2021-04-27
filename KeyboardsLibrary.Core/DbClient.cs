using KeyboardsLibrary.Core.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace KeyboardsLibrary.Core
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<EbayKeyboard> _keyboards;
        
        public DbClient(IOptions<KeyboardsDbConfig> keyboardDbConfig)
        {
            var client = new MongoClient(keyboardDbConfig.Value.Connection_String);
            var database = client.GetDatabase(keyboardDbConfig.Value.Database_Name);
            _keyboards = database.GetCollection<EbayKeyboard>(keyboardDbConfig.Value.Keyboard_Collection_Name);
        }
        
        public IMongoCollection<EbayKeyboard> GetKeyboardCollection()
        {
            return _keyboards;
        }
    }
}