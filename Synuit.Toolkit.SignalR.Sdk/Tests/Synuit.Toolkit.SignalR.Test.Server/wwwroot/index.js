const HTTPS = false;

var  url = HTTPS ? 'https://localhost:15001/hub/the1st' : 'http://localhost:15000/hub/the1st';

func(url);

async function func(url) {
    const hubConnection = createHubConnection(url);

    hubConnection.on('ReceiveMessage', (source, message) => document.writeln(source + ' ' + message + '<br/>'));

    if (await startHubConnection(hubConnection)) {
        console.log('Index: Hub connected.');

        await hubConnection.invoke('ProcessDto',
            [{ 'ClientId': 'WebClient', 'Data': 20 },
             { 'ClientId': 'WebClient', 'Data': 21 }]);
            
        if (await startStreaming(hubConnection,
            'StartStreaming',
            item => document.write(item.clientId + ' &nbsp;  &nbsp; ' + item.data + '<br/>'))) {
            console.log('Index: Hub streaming started.');
        }
    }
}

