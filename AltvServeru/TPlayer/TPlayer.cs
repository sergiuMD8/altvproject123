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

        public static String[] Fraktionen = new String[3] { "No Fraction", "Los Santos Police Department", "Newsfirma" };
        public static String[] RangName = new String[6] { "No Rank", "Стажер", "Ученик", "Cотрудник", "Заместитель Начальника", "Начальник" };

        public enum AdminRanks { Player,Moderator,Suporter,Administrator};
        public int PlayerID { get; set; }
        public String PlayerName { get; set; }
        public long Geld {  get; set; }
        public int Adminlevel { get; set; }
        public int Fraktion { get; set; }
        public int Rang {  get; set; }



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

        public bool IsPlayerInFraktion(int frak)
        {
            return Fraktion == frak;
        }

        public string PFrakionName()
        {
            return Fraktionen[Fraktion];
        }

        public int PFractionRang()
        {
            return Rang;
        }

        public String PRangName()
        {
            return RangName[Rang];
        }
    }
}
