using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;

namespace AltvServeru
{
    public class Events :IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(TPlayer.TPlayer tplayer, String reason) 
        {
            Alt.Log($"Игрок {tplayer.Name} подключился к серверу");
            tplayer.Spawn(new AltV.Net.Data.Position(496, 5499, 774), 0);
            tplayer.Model = (uint)PedModel.JoshCutscene;
        }


        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, String reason)
        {
            Alt.Log($"Игрок {tplayer.Name} покинул сервер - причина {reason}");
        }


        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (!Datebank.IsAccountRegistred(name))
            {
                if (!tplayer.Einloggt && name.Length > 3 && password.Length > 5)
                {
                    tplayer.PlayerName = name;
                    Datebank.NewAccountRegistration(name, password);
                    tplayer.Spawn(new AltV.Net.Data.Position(496, 5499, 774), 0);
                    tplayer.Model = (uint)PedModel.JoshCutscene;
                    tplayer.Einloggt = true;
                    tplayer.Emit("CloseLoginHud");
                    tplayer.SendChatMessage("{00c900} Успешная регистрация аккаунта");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Аккаунт с введенным именем уже существует");
            }
        }



        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer tplayer, String name, String password) 
        {
            if (Datebank.IsAccountRegistred(name))
            {
                if(!tplayer.Einloggt && name.Length > 3 && password.Length > 5) 
                {
                    if(Datebank.PasswordCheck(name, password))
                    {
                        tplayer.PlayerName = name;
                        Datebank.AccountLaden(tplayer);
                        tplayer.Spawn(new AltV.Net.Data.Position(496, 5499, 774), 0);
                        tplayer.Model = (uint)PedModel.JoshCutscene;
                        tplayer.Einloggt = true;
                        tplayer.Emit("CloseLoginHud");
                        tplayer.SendChatMessage("{00c900} Вы вошли в свой аккаунт");

                    }
                    else
                    {
                        tplayer.Emit("SendErrorMessage", "Неверный пароль от акаунта");
                    }
                }
                else
                {
                    tplayer.Emit("SendErrorMessage", "Недопустимый ввод, пожалуйста проверте правильность ввода");
                }
                

            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Ваша учетная записть не найдена");
            }
        }





















        [ScriptEvent(ScriptEventType.PlayerDead)]
        public void OnPlayerDead(TPlayer.TPlayer tplayer, IEntity killer, uint weapon)
        {
            tplayer.Spawn(new AltV.Net.Data.Position(522, 250, 105), 10);
        }
    }
}
