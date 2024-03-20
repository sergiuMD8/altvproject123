using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using System.Numerics;

namespace AltvServeru
{
    public class Commands : IScript
    {
        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(TPlayer.TPlayer tplayer, string command)
        {
            tplayer.SendChatMessage("{FF0000}Данная команда не найдена");
            return;

        }

        [Command("car")]
        public void CSpawnCar(TPlayer.TPlayer tplayer, string VehicleName, int R = 0, int G = 0, int B = 0)
        {
            if(!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Suporter)) 
            {
                tplayer.SendChatMessage("{FF0000} Ваш уровень администратора слижком низкий");
                return ;
            
            }
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 1.5f, tplayer.Position.Z), tplayer.Rotation);
            if (veh != null) 
            {
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                tplayer.SendChatMessage(" {04B404}Ваш транспорт создан");
            }
            else
            {
                tplayer.SendChatMessage(" {FF0000} Неврзможно создать транспорт");
            }
            Utils.adminLog($"Игрок с ником {tplayer.PlayerName} заспавнил себе транспорт {VehicleName}", "AltV server");
        }

        [Command("freezeme")]
        public void CMD_freezeme(TPlayer.TPlayer tplayer, bool freezeme)
        {
            if (!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000} Ваш уровень администратора слижком низкий");
                return;
            }

            tplayer.Emit("freezePlayer", freezeme);
            if (freezeme == true)
            {
                tplayer.SendChatMessage("{04B404} Вы были заморожены");
            }
            else
            {
                tplayer.SendChatMessage("{04B404} Вы были разморожены");
            }
        }

        [Command("hp")]
        public void SetEntitiHealth(TPlayer.TPlayer tplayer, ushort hp)
        {
            if (hp == 100)
            {
                // Если hp равно 100, устанавливаем здоровье в 200
                tplayer.Health = 200;
            }
            else if (hp == 0)
            {
                // Если hp равно 0, устанавливаем здоровье в 0
                tplayer.Health = 0;
            }
            else
            {
                // Во всех остальных случаях устанавливаем здоровье равное hp
                int playerHealth = tplayer.Health = (ushort)(100 + hp);
                Alt.Log(playerHealth.ToString());
            }

        }

        [Command("tp")]
        public void CMD_tp(TPlayer.TPlayer tplayer, float x, float y, float z)
        {
            if (!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000} Ваш уровень администратора слижком низкий");
                return;

            }

            AltV.Net.Data.Position position = new AltV.Net.Data.Position(x, y, z+0.2f);
            tplayer.Position = position;
            tplayer.SendChatMessage("{04B404} Вы были телепортированы");
            return ;
        }

        [Command("fraktioninfo")]
        public void CMD_fraktioninfo(TPlayer.TPlayer tplayer)
        {
            tplayer.SendChatMessage($"Вы находитесь в фракции {tplayer.PFrakionName()} на должности {tplayer.PRangName()}!");
            return ;
        }

        [Command("makeleader")]
        public void CMD_makeleader(TPlayer.TPlayer tplayer, string playertarget, int frak)
        {
            if (!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000} Ваш уровень администратора слижком низкий");
                return;

            }
            TPlayer.TPlayer target = Utils.GetPlayerByName(playertarget);
            if (target == null)
            {
                tplayer.SendChatMessage("{FF0000} Игрок не найден!");
                return;
            }
            if(frak < 0 || frak > TPlayer.TPlayer.Fraktionen.Length)
            {
                tplayer.SendChatMessage("{FF0000} Нет фракции!");
            }
            target.Fraktion = frak;
            target.Rang = 5;
            Datebank.AccountUpdate(tplayer);
            tplayer.SendChatMessage($"Вы заначили {target.Name} лидером фракции {TPlayer.TPlayer.Fraktionen[frak]}");
            target.SendChatMessage($"{tplayer.Name} вы стали лидером фракции {TPlayer.TPlayer.Fraktionen[frak]}");

        }

        [Command("invite")]
        public void CMD_invite(TPlayer.TPlayer tplayer, string playertarget)
        {
            if(tplayer.Fraktion == 0 || tplayer.Rang < 5)
            {
                tplayer.SendChatMessage("{FF0000}Вы не состоите в фракции или ваш ранг слижком низкий!");
                return ;
            }
            TPlayer.TPlayer target = Utils.GetPlayerByName(playertarget);
            if (target == null)
            {
                tplayer.SendChatMessage("{FF0000} Недействительный игрок");
                return;
            }
            target.Fraktion = tplayer.Fraktion;
            target.Rang = 1;
            Datebank.AccountUpdate(tplayer);
            tplayer.SendChatMessage($"Вы пригласили {target.Name} в фракцию {TPlayer.TPlayer.Fraktionen[tplayer.Fraktion]}");
            target.SendChatMessage($"{tplayer.Name} вы стали членом фракции {TPlayer.TPlayer.Fraktionen[tplayer.Fraktion]}");

        }


        [Command("setrang")]
        public void CMD_setrang(TPlayer.TPlayer tplayer)

        {
            if (!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000} Ваш уровень администратора слижком низкий");
                return;

            }
            tplayer.Rang = 2;
            Datebank.AccountUpdate(tplayer);
            tplayer.SendChatMessage($"Ваш ранг был повышен!");
            return;
        }





        [Command("pistol")]
        public void CMD_pistol(TPlayer.TPlayer tplayer)
        {
            if(tplayer.Fraktion != 1)
            {
                string msg = "{FF0000} Вы не работаете в LSPD";
                tplayer.SendChatMessage (msg);
                return ;
            }
            tplayer.GiveWeapon(AltV.Net.Enums.WeaponModel.HeavyRevolverMkII, 15, true);
            tplayer.SendChatMessage("{04B404}Вы получили оружие!");
                
        }

       

    }
}
