async function getIssues() {
    return (await db).transaction("issue").store.getAll();
}

async function createIssue(issue) {
    (await db).transaction("issue", 'readwrite').store.add(issue);
}

async function updateIssue(issue) {
    (await db).transaction("issue", 'readwrite').store.put(issue);
}

async function deleteIssue(id) {
    (await db).transaction("issue", 'readwrite').store.delete(id);
}

async function completeTask(completedTask) {
    const tx = (await db).transaction(['done', 'issue'], 'readwrite');
    const issueStore = tx.objectStore('issue');
    const doneStore = tx.objectStore('done');

    const toUpdate = await issueStore.get(completedTask.issueId);
    toUpdate.tasks = toUpdate.tasks
        .filter(task => task.id !== completedTask.id);

    const issuePromise = toUpdate.tasks.length === 0
        ? issueStore.delete(completedTask.issueId)
        : issueStore.put(toUpdate);

    const key = formatDate(new Date(completedTask.createdOn))
    const existingDoneOnDate = await doneStore.get(key)

    let completedTaskPromise
    if (!existingDoneOnDate) {
        const toAdd = toCompletedTaskViewModel(completedTask)
        completedTaskPromise = doneStore.add([toAdd], key)
    } else {
        const existingIssue = existingDoneOnDate
            .find(task => task.issueTitle === completedTask.issueTitle)
        if (existingIssue) {
            existingIssue.contents.push(completedTask.content)
        } else {
            const toAdd = toCompletedTaskViewModel(completedTask)
            existingDoneOnDate.push(toAdd)
        }
        completedTaskPromise = doneStore.put(existingDoneOnDate, key)
    }

    await Promise.all([
        issuePromise,
        completedTaskPromise,
        tx.done,
    ]);
}

function toCompletedTaskViewModel(completedTask) {
    return {
        'for': completedTask.for,
        contents: [completedTask.content],
        issueTitle: completedTask.issueTitle,
        createdOn: completedTask.createdOn,
        discriminator: 'CompletedTask',
    }
}
