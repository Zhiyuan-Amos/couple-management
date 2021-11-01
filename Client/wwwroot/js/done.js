async function getDone() {
    const kvp = {}

    const tx = (await db).transaction(['done', 'image'])
    const doneStore = tx.objectStore('done')
    let cursor = await doneStore.openCursor()

    const imageStore = tx.objectStore('image')

    while (cursor) {
        for (const item of cursor.value) {
            if (item.discriminator === "Image") {
                const image = await imageStore.get(item.id)
                item.isFavourite = image.isFavourite === 1
                item.takenOn = image.takenOn
                item.data = Base64FromUint8Array(image.data)
            }
        }
        kvp[cursor.key] = cursor.value

        cursor = await cursor.continue()
    }

    return kvp
}
