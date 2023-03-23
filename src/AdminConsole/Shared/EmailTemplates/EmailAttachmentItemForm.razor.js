export const initPreviewImg = (inputArray, imgElem) => {
    const imgBlob = new Blob([inputArray], { type: 'image/jpeg' });
    const url = URL.createObjectURL(imgBlob);
    imgElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    imgElem.src = url;

    console.log(url);

    return url;
};

export const updatePreviewImage = (inputElem, imgElem) => {
    const url = URL.createObjectURL(inputElem.files[0]);
    imgElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    imgElem.src = url;

    console.log(url);

    return url;
};