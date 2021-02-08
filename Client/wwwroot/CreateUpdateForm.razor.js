export function togglePicker(wrapperElement) {
    var popupBtn = wrapperElement.querySelector(".k-select");
    popupBtn.click();
}

export function disableInput(wrapperElement) {
    var input = wrapperElement.querySelector("input");
    input.setAttribute("disabled", "true");
}

