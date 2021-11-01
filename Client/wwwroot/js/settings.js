// Based on https://www.meziantou.net/generating-and-downloading-a-file-in-a-blazor-webassembly-application.htm
// Modified to marshal parameters as byte[] is not being passed from C#
async function exportDatabaseAsFile(name) {
    const jsonContent = await exportDatabase()
    const file = new File([jsonContent], name);
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = name;
    a.target = "_self";
    a.click();
}

async function exportDatabase() {
    const idb = await db
    const transaction = idb.transaction(
        idb.objectStoreNames,
        'readonly'
    )

    const to= {}
    for (const storeName of idb.objectStoreNames) {
        let cursor = await transaction
            .objectStore(storeName)
            .openCursor()

        let allObjects
        if (storeName === "done") {
            allObjects = {}
            while (cursor) {
                allObjects[cursor.key] = cursor.value
                cursor = await cursor.continue()
            }
        } else if (storeName === 'image') {
            allObjects = []
            while (cursor) {
                const image = cursor.value
                image.data = Base64FromUint8Array(image.data)
                allObjects.push(image)
                cursor = await cursor.continue()
            }
        } else {
            allObjects = []
            while (cursor) {
                allObjects.push(cursor.value)
                cursor = await cursor.continue()
            }
        }

        toExport[storeName] = allObjects
    }

    return JSON.stringify(toExport)
}

async function importDatabase(json) {
    const idb = await db
    const tx = idb.transaction(
        idb.objectStoreNames,
        'readwrite'
    )

    const toAwait = []
    const jsonObject = JSON.parse(json)
    Object.keys(jsonObject)
        .forEach(storeName => {
            const store = tx.objectStore(storeName)
            if (storeName === "done") {
                const dateToDone = jsonObject[storeName]
                Object.keys(dateToDone)
                    .forEach(key => {
                        const promise = store.add(dateToDone[key], key)
                        toAwait.push(promise);
                    })
            } else if (storeName === "image") {
                jsonObject[storeName]
                    .forEach(image => {
                        image.data = Uint8ArrayFromBase64(image.data);
                        const promise = store.add(image)
                        toAwait.push(promise);
                    })
            } else {
                jsonObject[storeName]
                    .forEach(value => {
                        const promise = store.add(value)
                        toAwait.push(promise);
                    })
            }
        })

    await Promise.all([
        toAwait,
        tx.done,
    ])
}

async function clearDatabase() {
    const idb = await db
    const tx = idb.transaction(
        idb.objectStoreNames,
        'readwrite'
    )

    const toAwait = []
    for (const storeName of idb.objectStoreNames) {
        const promise = tx
            .objectStore(storeName)
            .clear()
        toAwait.push(promise)
    }

    await Promise.all([
        toAwait,
        tx.done,
    ])
}

async function deleteDatabase() {
    idb.deleteDB('Couple')
}

