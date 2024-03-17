using AltV.Net;
using AltV;
using AltV.Net.Elements.Entities;
using AltvServeru.TPlayer;
namespace AltvServeru
{
    class Server : Resource
    {
        public override void OnStart()
        {
            Alt.Log("server started");
            Utils.adminLog("Сервер был запушен", "AltV server");
            // MYSQL
            Datebank.InitConnection();
            
        }

        public override void OnStop()
        {
            Alt.Log("server stop");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TPlayerFactory();
        }
        
    }
}
