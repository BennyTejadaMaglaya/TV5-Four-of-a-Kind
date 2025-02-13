const confirmationBox = document.getElementById("pending-confirmations")
let empty;
let currentVolunteer; // Stores the old state of the volunteer being edited.
export function confirmVolunteer(e) {
    const volunteerId = e.target.dataset.volunteerid
    // hit out confirm endpoint
    // if 200 response code we remove from confirmations
    // if 400 we will add an error message
    fetch("volunteer/ConfirmVolunteer/" + volunteerId, { method: "post" })
        .then(res => {
            if (res.ok) {
                // then remove it from the list of confirmations 
                const confirmElement = document.getElementById("confirmation-" + volunteerId)
                confirmationBox.removeChild(confirmElement);
            }
            else {
                // e msg
            }
        })
        .catch(err => {
            console.log(err)
        })
}

export function denyVolunteer(e) {
    const volunteerId = e.target.dataset.volunteerid
    fetch("volunteer/DenyVolunteer/" + volunteerId, { method: "post" })
        .then(res => {
            if (res.ok) {
                // then remove it from the list of confirmations 
                const confirmElement = document.getElementById("confirmation-" + volunteerId)
                confirmationBox.removeChild(confirmElement);
            }
            else {
                // e msg
                console.log(res)
            }
        })
        .catch(err => {
            console.log(err)
        })

}
function saveD() {
    if (empty) {
        confirmationBox.innerHTML = empty;
        return;
    }
    console.log(confirmationBox.children)
    const c = confirmationBox.cloneNode(true).innerHTML;
    confirmationBox.innerHTML = ""
    
    empty = c;
    console.log(empty + "wow")

}

function editVolunteer(e) {
    if (currentVolunteer !== undefined) {
        // already editing a volunteer
        return;
    }
    const id = e.target.dataset.volunteerid;
    currentVolunteer = getVolunteerValues(id);
    const element = document.getElementById("v-edit-" + id)
    const editable = element.querySelector(".editable")
    editable.innerHTML = createEditForm(id)
    bindEditorButtons();

    
    // if theres already a volunteer being edited we wont let them do this (status message eventually)

    // save the old state in case of discard chnges
    // switch the element to a mini form with the current values 
    // try and post to the edit endpoint on save
    // if 200 then 
}

function getVolunteerValues(id) {
    // temporarily store the volunteers current values before editing.

    const oldValues = {
        numShifts: document.getElementById("v-edit-shifts-" + id).innerHTML,
        timesLate: document.getElementById("v-edit-late-" + id).innerHTML,
        firstName: document.getElementById("v-edit-first-" + id).innerHTML,
        lastName: document.getElementById("v-edit-last-" + id).innerHTML,
        joinDate: document.getElementById("v-edit-join-" + id).innerHTML,
        phoneNumber: document.getElementById("v-edit-phone-" + id).innerHTML,
        email: document.getElementById("v-edit-email-" + id).innerHTML,
        id:id
    }
    console.log(oldValues)
    return oldValues;
    
}

function saveVolunteerChanges(e) {
    if (!validateEditorFormData) {
        return // error message for frontend validation
    }
    const id = e.target.dataset.volunteerid;
    currentVolunteer.firstName = document.getElementById('v-edit-first').value
    currentVolunteer.lastName = document.getElementById('v-edit-last').value
    currentVolunteer.email = document.getElementById('v-edit-email').value
    currentVolunteer.phoneNumber = document.getElementById('v-edit-phone').value
    fetch("/volunteer/update/" + id, {
        method: "post",
        body: JSON.stringify(
            {
                firstName: currentVolunteer.firstName,
                lastName: currentVolunteer.lastName,
                phoneNumber: currentVolunteer.phoneNumber,
                email:currentVolunteer.email,
            }
        ),
        headers: { 'Content-Type': 'application/json' }
    }, )
        .then(res => {
            if (!res.ok) {
                // show error messages 
                return;
            }
            const editable = getVolunteerEditable(id)
            editable.innerHTML = createVolunteerDetailsBox(currentVolunteer)
            currentVolunteer = undefined;
            bindEditButton(id)
        })
        .catch(err => {

        })
    // validate
    // try post chnges

    // if 200 set change the form to the details display 
    // if 400 show the error message and keep the form display
}

function validateEditorFormData() {
    return true;
}

function discardVolunteerChanges(e) {
    // switch to the details view and set currentVolunteer to undefined;
    // rebind edit button
    const id = e.target.dataset.volunteerid;
    const editable = getVolunteerEditable(id)
    editable.innerHTML = createVolunteerDetailsBox(currentVolunteer)
    currentVolunteer = undefined;
    bindEditButton(id)

}

function bindEditorButtons() {
    document.getElementById("v-edit-save").addEventListener("click", saveVolunteerChanges)
    document.getElementById("v-edit-discard").addEventListener("click", discardVolunteerChanges)
}

function bindEditButton(id) {
    document.getElementById(`v-btn-edit-${id}`).addEventListener("click", editVolunteer)
}

function getVolunteerEditable(id) {
    const element = document.getElementById("v-edit-" + id)
    const editable = element.querySelector(".editable")
    return editable;
}

function createEditForm() {
    // requires a current volunteer
    // needs the id so we know which 
   
    const form = document.createElement("form");
    form.onsubmit = (e) => e.preventDefault()
    form.id = "v-edit"
    const editForm = `
                <div>
                    <label class="d-block" for="v-edit-first">First Name</label>
                    <input id="v-edit-first" value="${currentVolunteer.firstName}"/>
                </div>
                <div>
                    <label class="d-block" for="v-edit-first">Last Name</label>
                    <input id="v-edit-last" value="${currentVolunteer.lastName}"/>
                </div>
                <div>
                    <label class="d-block" for="v-edit-first">Phone</label>
                    <input id="v-edit-phone" value="${currentVolunteer.phoneNumber}"/>
                </div>
                <div>
                    <label class="d-block" for="v-edit-first">Email</label>
                    <input id="v-edit-email" value="${currentVolunteer.email}"/>
                </div>
                <div class="d-inline d-flex gap-1 align-items-bottom">
                    <button class="h-fit" id="v-edit-save" data-volunteerid="${currentVolunteer.id}">Save</button>
                    <button class="w-fit" id="v-edit-discard" data-volunteerid="${currentVolunteer.id}">Discard</button>
                </div>
    `           
    form.innerHTML = editForm;
    return editForm;
}

function createVolunteerDetailsBox(volunteer) {
    const detailsBox = `
    <div class="editable d-inline">
        <div class="d-inline">
            <span class="d-inline" id="v-edit-first-${volunteer.id}">${volunteer.firstName}</span>
            <span class="d-inline" id="v-edit-last-${volunteer.id}">${volunteer.lastName}</span>
        </div>

        <div>
            <span id="v-edit-email-${volunteer.id}">${volunteer.email}</span>
            <span id="v-edit-phone-${volunteer.id}">${volunteer.phoneNumber}</span>
        </div>
    </div>

    <div>
        <span id="v-edit-shifts-${volunteer.id}">${volunteer.numShifts}</span>
        <span id="v-edit-late-${volunteer.id}">${volunteer.timesLate}</span>
        <span id="v-edit-join-${volunteer.id}">${volunteer.joinDate}</span>
    </div>
    <button id="v-btn-edit-${volunteer.id}" data-volunteerid="${volunteer.id}">Edit</button>
    `
    return detailsBox
}


function onLoadVolunteerPage() { // this just adds listeners to all the partial elements ATM, allows them to change without reloading the page
    document.querySelectorAll(".v-btn-confirm").forEach(s => s.addEventListener("click", confirmVolunteer)) // attach listeners to confirm buttons
    document.querySelectorAll(".v-btn-deny").forEach(s => s.addEventListener("click", denyVolunteer)) // attach listeners to deny buttons
    document.querySelectorAll(".v-btn-edit").forEach(s => s.addEventListener("click", editVolunteer)) // attach listeners to edit buttons
}





window.onload = () => {
    onLoadVolunteerPage();
}