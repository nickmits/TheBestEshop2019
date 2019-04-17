// Write your Javascript code.

async function sendData(url, data) {    
    const urlToSendRequest = "https://" + window.location.host + url;

    const rawData = await fetch(urlToSendRequest, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: "same-origin",
        body: JSON.stringify(data)
    });
    return rawData.json();
}