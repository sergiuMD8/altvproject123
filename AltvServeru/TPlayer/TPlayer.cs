using AltV.Net;
using AltV.Net.CApi.Libraries;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltvServeru.TPlayer
{
    public class TPlayer : Player
    {
        public enum AdminRanks { Player,Moderator,Suporter,Administrator};
        public int PlayerID { get; set; }
        public String PlayerName { get; set; }
        public long Geld {  get; set; }
        public int Adminlevel { get; set; }

        public bool Einloggt { get; set; }
       
        public TPlayer(ICore core, IntPtr nativePointer, uint id) : base(core, nativePointer, id) 
        {
            Geld = 5000;
            Adminlevel = 0;
            Einloggt = false;
        }

        public bool IsPlayerAdmin(int alvl)
        {
            return Adminlevel >= alvl;
        }
    }
}
