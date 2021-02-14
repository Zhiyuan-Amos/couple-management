export async function getAll() {
    return (await db).transaction("event").store.getAll();
}

// Another possible implementation is to pass only the Event (w/ ToDos),
// then retrieve the existing Event from database & generate set difference.
// Pro: Impossible for callers of the method to pass the wrong set of model.
// Con: Additional code to generate set difference and additional database call.
// I've used this implementation for code simplicity.
export async function add(event, added, removed) {
    const tx = (await db).transaction(['event', 'todo'], 'readwrite');
    const eventStore = tx.objectStore('event');
    const toDoStore = tx.objectStore('todo');

    await Promise.all([
        eventStore.add(event),
        added.map(id => toDoStore.delete(id)),
        removed.map(toDo => toDoStore.add(toDo)),
        tx.done,
    ]);
}

export async function update(event, added, removed) {
    const tx = (await db).transaction(['event', 'todo'], 'readwrite');
    const eventStore = tx.objectStore('event');
    const toDoStore = tx.objectStore('todo');

    await Promise.all([
        eventStore.put(event),
        added.map(id => toDoStore.delete(id)),
        removed.map(toDo => toDoStore.add(toDo)),
        tx.done,
    ]);
}

export async function remove(id, removed) {
    const tx = (await db).transaction(['event', 'todo'], 'readwrite');
    const eventStore = tx.objectStore('event');
    const toDoStore = tx.objectStore('todo');

    await Promise.all([
        eventStore.delete(id),
        removed.map(toDo => toDoStore.put(toDo)),
        tx.done,
    ]);
}

