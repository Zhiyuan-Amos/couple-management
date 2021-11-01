function Uint8ArrayFromBase64(base64)
{
    return Uint8Array.from(window.atob(base64), (v) => v.charCodeAt(0));
}

function Base64FromUint8Array(uint8Array)
{
    let data = ''
    for (let i = 0; i < uint8Array.byteLength; i++) {
        data += String.fromCharCode(uint8Array[i])
    }

    return btoa(data)
}
