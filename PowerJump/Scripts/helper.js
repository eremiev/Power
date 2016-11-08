function DropdownSelectChange(selectedList, value) {

    if (selectedList == "Events") {
        var project = document.getElementById("Projects");
        if (value) {
            project.disabled = true;
        } else {
            project.disabled = false;
        }

    } else if (selectedList == "Projects") {
        var event = document.getElementById("Events");
        if (value) {
            event.disabled = true;
        } else {
            event.disabled = false;
        }
    }
}