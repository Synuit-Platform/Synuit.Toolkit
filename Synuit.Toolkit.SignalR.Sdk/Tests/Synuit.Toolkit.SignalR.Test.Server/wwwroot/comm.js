//https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-2.2

function createHubConnection(url) {
    return new signalR.HubConnectionBuilder()
        .withUrl(url)
        .configureLogging(signalR.LogLevel.Information)
        .build();
}

async function startHubConnection(hubConnection) {
    try {
        await hubConnection.start();
        console.log('Comm: Successfully connected to hub ' + url + ' .');
        return true;
    }
    catch (err) {
        console.error('Comm: Error in hub startHubConnection(). Unable to establish connection with hub ' + url + ' .  ' + err);
        return false;
    }
}

async function startStreaming(hubConnection, serverFuncName, callback) {
    isOK = false;
    try {
        await hubConnection.stream(serverFuncName)
            .subscribe({
                next: item => {
                    try {
                        callback(item);
                    }
                    catch (err) {
                        console.error('Comm: Error in hub streaming callback. ' + err);
                    }
                },
                complete: () => console.log('Comm: Hub streaming completed.'),
                error: err => console.error('Comm: Error in hub streaming subscription. ' + err)
            });

        console.log('Comm: Hub streaming started.');
        isOK = true;
    }
    catch (err) {
        console.error('Comm: Error in hub startStreaming(). ' + err);
    }

    return isOK;
}
