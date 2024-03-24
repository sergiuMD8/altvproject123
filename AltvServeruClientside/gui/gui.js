

alt.on('sendNotification', (statusCode, text) => notify(statusCode, text));

function notify(statusCode, text) {
    if (text.length > 0) {
        toastr.options.positionClass = 'toast-top-right';
        toastr[statusCode](text);
    }
}
