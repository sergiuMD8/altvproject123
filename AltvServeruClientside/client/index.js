/// <reference types ="@altv/types-client"/>
/// <reference types ="@altv/types-natives"/>

import * as alt from "alt-client"
import * as native from "natives"


//Variable

let loginHud;
let guiHud;
//===========================================================//


alt.onServer('freezePlayer', (freeze) => {
    const IPlayer = alt.Player.local.scriptID;
    native.freezeEntityPosition(IPlayer, freeze);

})

//===========================================================//

alt.onServer('CloseLoginHud', () => {
    alt.showCursor(false)
    alt.toggleGameControls(true)
    alt.toggleVoiceControls(true)

    if (loginHud)
    {
        loginHud.destroy();
    }
})

//===========================================================//

alt.onServer('SendErrorMessage', (text) => {
    loginHud.emit('ErrorMessage', text);
})

//===========================================================//

alt.on('connectionComplete', () => {
    loadBlips();


    guiHud = new alt.WebView("http://resources/AltvServeruClientside/gui/gui.html");
    loginHud = new alt.WebView("http://resources/AltvServeruClientside/login/login.html");
    loginHud.focus();

    alt.showCursor(true)
    alt.toggleGameControls(false)
    alt.toggleVoiceControls(false)

    loginHud.on('Auth.Login', (name, password) => {
        alt.emitServer('Event.Login', name, password)
    })

    loginHud.on('Auth.Register', (name, password) => {
        alt.emitServer('Event.Register', name, password)
    })
})

alt.onServer('sendNotification', (statusCode, text) => {
    guiHud.emit('sendNotification', statusCode, text);
});

function loadBlips()
{
    createBlip(496, 5499, 774,8,29,1.0,false,"spawn");
}

function createBlip(x,y,z,sprite,color,scale=1.0,shortRange=false, name="")
{
    const tempBlip = new alt.PointBlip(x,y,z);

    tempBloop.sprite =sprite;
    tempBlip.color = color;
    tempBlip.shortRange = shortRange;
    tempBlip.scale = scale;
    if(name,length > 0)
    tempBlip.name = name;
}