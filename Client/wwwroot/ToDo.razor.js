export async function getAll() {
    return (await db).transaction("todo").store.getAll();
}

export async function add(value) {
    (await db).transaction("todo", 'readwrite').store.add(value);
}

export async function update(value) {
    (await db).transaction("todo", 'readwrite').store.put(value);
}

export async function remove(key) {
    (await db).transaction("todo", 'readwrite').store.delete(key);
}

