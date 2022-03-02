export function synchronizeFileWithIndexedDb(filename) {
    return new Promise((res, rej) => {
        const db = window.indexedDB.open("SqliteStorage", 1);
        db.onupgradeneeded = () => {
            db.result.createObjectStore("Files", { keypath: "id" });
        };

        db.onsuccess = () => {
            const req = db.result.transaction("Files", "readonly").objectStore("Files").get("file");
            req.onsuccess = () => {
                Module.FS_createDataFile("/", filename, req.result, true, true, true);
                res();
            };
        };
    });
}
