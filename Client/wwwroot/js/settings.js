async function importDatabase(byteArray, name) {
    const db = window.indexedDB.open("SqliteStorage");
    db.onsuccess = () => {
        // Unable to call Module.FS_createDataFile("/", filename, req.result, true, true, true) more than once as it
        // will error out, and dbstorage.js calls it when the application starts up
        // Therefore, update IndexedDb and restart reload instead, so that app.db gets updated
        const putRequest = db.result.transaction("Files", "readwrite").objectStore("Files").put(byteArray, "file");
        putRequest.onsuccess = () => {
            window.location.reload();
        };
    };
}

// Based on https://www.meziantou.net/generating-and-downloading-a-file-in-a-blazor-webassembly-application.htm
async function exportDatabase(name) {
    const content = FS.readFile(`/${name}`);
    const file = new File([content], name);
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = name;
    a.target = "_self";
    a.click();
}

async function deleteDatabase() {
    window.indexedDB.deleteDatabase("SqliteStorage");
}

async function logout(clientId, authority, knownAuthority, postLogoutRedirectUri) {
    const msalConfig = {
        auth: {
            clientId: clientId,
            authority: authority,
            knownAuthorities: [knownAuthority],
            postLogoutRedirectUri: postLogoutRedirectUri,
        },
        cache: {
            cacheLocation: "localStorage",
            storeAuthStateInCookie: false,
        },
    };

    const publicClientApplication = new msal.PublicClientApplication(msalConfig);
    const logoutRequest = {
        account: publicClientApplication.getAllAccounts()[0]
    };

    publicClientApplication.logoutRedirect(logoutRequest);
}
