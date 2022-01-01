async function createImage(image) {
    image.isFavourite = image.isFavourite ? 1 : 0;
    image.takenOn = new Date(image.takenOn);
    const tx = (await db).transaction(["done", "image"], "readwrite");

    const imageStore = tx.objectStore("image");
    const addImageTask = imageStore.add(image);

    const doneStore = tx.objectStore("done");
    const key = formatDate(image.takenOn);
    const existingDoneOnDate = await doneStore.get(key);

    const doneImage = {
        id: image.id,
        discriminator: "Image"
    };

    let addDoneTask;
    if (!existingDoneOnDate) {
        addDoneTask = doneStore.add([doneImage], key);
    } else {
        existingDoneOnDate.push(doneImage);
        addDoneTask = doneStore.put(existingDoneOnDate, key);
    }

    await Promise.all([
        addImageTask,
        addDoneTask,
        tx.done,
    ]);
}

async function updateImage(image) {
    image.isFavourite = image.isFavourite ? 1 : 0;
    image.takenOn = new Date(image.takenOn);

    const tx = (await db).transaction(["done", "image"], "readwrite");
    const imageStore = tx.objectStore("image");

    const existingImage = await imageStore.get(image.id);
    const existingDate = formatDate(existingImage.takenOn);
    const updatedDate = formatDate(image.takenOn);

    const updateImageTask = imageStore.put(image);

    if (existingDate === updatedDate) {
        await updateImageTask;
    } else {
        const doneStore = tx.objectStore("done");

        const doneOnUpdatedDate = await doneStore.get(updatedDate);

        const doneImage = {
            id: image.id,
            discriminator: "Image"
        };

        let addDoneTask;
        if (!doneOnUpdatedDate) {
            addDoneTask = doneStore.add([doneImage], updatedDate);
        } else {
            doneOnUpdatedDate.push(doneImage);
            addDoneTask = doneStore.put(doneOnUpdatedDate, updatedDate);
        }

        const doneOnExistingDate = await doneStore.get(existingDate);
        const removed = doneOnExistingDate.filter(item => item.discriminator !== "Image" || item.id !== image.id);

        let removeDoneTask;
        if (removed.length === 0) {
            doneStore.delete(existingDate);
        } else {
            removeDoneTask = doneStore.put(removed, existingDate);
        }

        await Promise.all([
            updateImageTask,
            addDoneTask,
            removeDoneTask,
            tx.done,
        ]);
    }
}

async function deleteImage(id) {
    const tx = (await db).transaction(["done", "image"], "readwrite");

    const imageStore = tx.objectStore("image");
    const image = await imageStore.get(id);

    const key = formatDate(new Date(image.takenOn));
    const doneStore = tx.objectStore("done");
    const existingDoneOnDate = await doneStore.get(key);

    const deleteImageTask = imageStore.delete(id);

    let removeDoneTask;
    if (existingDoneOnDate.length === 1) {
        removeDoneTask = doneStore.delete(key);
    } else {
        const remainingDoneOnDate = existingDoneOnDate.filter(item => item.discriminator !== "Image" || item.id !== id);
        removeDoneTask = doneStore.put(remainingDoneOnDate, key);
    }

    await Promise.all([
        deleteImageTask,
        removeDoneTask,
        tx.done,
    ]);
}
