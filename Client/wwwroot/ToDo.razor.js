export async function getAll() {
    return (await db).transaction("todo").store.getAll();
}

export async function add(todo) {
    (await db).transaction("todo", 'readwrite').store.add(todo);
}

export async function update(todo) {
    (await db).transaction("todo", 'readwrite').store.put(todo);
}

export async function remove(id) {
    (await db).transaction("todo", 'readwrite').store.delete(id);
}

