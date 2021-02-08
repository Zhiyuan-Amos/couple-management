(function () {
    const db = idb.openDB('Couple', 1, {
        upgrade(db) {
            db.createObjectStore('todo', { keyPath: 'id' });
            db.createObjectStore('event', { keyPath: 'id' });
        },
    });

    window.localStore = {
        get: async (storeName, key) => (await db).transaction(storeName).store.get(key),
        getAll: async (storeName) => (await db).transaction(storeName).store.getAll(),
        put: async (storeName, value) => (await db).transaction(storeName, 'readwrite').store.put(value),
        delete: async (storeName, key) => (await db).transaction(storeName, 'readwrite').store.delete(key),

        putEvent: async (event, added, removed) => {
            const tx = (await db).transaction(['event', 'todo'], 'readwrite');
            const eventStore = tx.objectStore('event');
            const toDoStore = tx.objectStore('todo');

            await Promise.all([
                eventStore.put(event),
                added.map(id => toDoStore.delete(id)),
                removed.map(toDo => toDoStore.put(toDo)),
                tx.done,
            ]);
        },

        deleteEvent: async (id, removed) => {
            const tx = (await db).transaction(['event', 'todo'], 'readwrite');
            const eventStore = tx.objectStore('event');
            const toDoStore = tx.objectStore('todo');

            await Promise.all([
                eventStore.delete(id),
                removed.map(toDo => toDoStore.put(toDo)),
                tx.done,
            ]);
        }
    };
})();

