async function getFavouriteImages() {
    const favouriteImages = await (await db).transaction("image").store.index('isFavourite').getAll(1)
    favouriteImages.forEach(image => image.isFavourite = image.isFavourite === 1)
    return favouriteImages
}
