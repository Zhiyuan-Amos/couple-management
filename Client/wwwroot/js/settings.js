// Based on https://www.meziantou.net/generating-and-downloading-a-file-in-a-blazor-webassembly-application.htm
// Modified to marshal parameters as byte[] is not being passed from C#
async function exportDatabaseAsFile(name) {
    const content = await exportDatabase();
    const file = new File([content], name);
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = name;
    a.target = "_self";
    a.click();
}

async function exportDatabase() {
    const idb = await db;
    const transaction = idb.transaction(
        idb.objectStoreNames,
        "readonly"
    );

    const toExport = [];
    for (const storeName of idb.objectStoreNames) {
        let cursor = await transaction
            .objectStore(storeName)
            .openCursor();

        toExport.push(storeName);
        if (storeName === "done") {
            while (cursor) {
                toExport.push(cursor.key);
                cursor.value.forEach(item => toExport.push(JSON.stringify(item)));
                toExport.push("");
                cursor = await cursor.continue();
            }
        } else if (storeName === "image") {
            while (cursor) {
                const image = cursor.value;
                image.data = Base64FromUint8Array(image.data);
                toExport.push(JSON.stringify(image));
                cursor = await cursor.continue();
            }
        } else {
            while (cursor) {
                toExport.push(JSON.stringify(cursor.value));
                cursor = await cursor.continue();
            }
        }
        toExport.push("---");
    }

    return toExport.join("\n");
}

async function clearDatabase() {
    const idb = await db;
    const tx = idb.transaction(
        idb.objectStoreNames,
        "readwrite"
    );

    const toAwait = [];
    for (const storeName of idb.objectStoreNames) {
        const promise = tx
            .objectStore(storeName)
            .clear();
        toAwait.push(promise);
    }

    await Promise.all([
        toAwait,
        tx.done,
    ]);
}

async function deleteDatabase() {
    idb.deleteDB("Couple");
}

async function importDone(done, date) {
    const store = (await db).transaction("done", "readwrite").store;
    if (done.discriminator === "CompletedTask") {
        done.createdOn = new Date(done.createdOn);
    }

    const existingDoneOnDate = await store.get(date);

    if (!existingDoneOnDate) {
        store.add([done], date);
    } else {
        existingDoneOnDate.push(done);
        store.put(existingDoneOnDate, date);
    }
}

async function importImage(image) {
    const store = (await db).transaction("image", "readwrite").store;
    image.isFavourite = image.isFavourite ? 1 : 0;
    image.takenOn = new Date(image.takenOn);
    image.data = Uint8ArrayFromBase64(image.data);

    store.add(image);
}

async function importIssue(issue) {
    issue.createdOn = new Date(issue.createdOn);
    (await db).transaction("issue", "readwrite").store.add(issue);
}
