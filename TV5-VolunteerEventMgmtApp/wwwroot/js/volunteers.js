


const confirmationBox = document.getElementById("pending-confirmations")
export function confirmVolunteer(e) {
    const volunteerId = e.target.dataset.volunteerid
    // hit out confirm endpoint
    // if 200 response code we remove from confirmations
    // if 400 we will add an error message
    console.log(e.target.dataset)

    const confirmation = fetch("volunteer/ConfirmVolunteer/" + volunteerId, { method: "post" })
        .then(res => {
            if (res.ok) {
                // then remove it from the list of confirmations 
                const confirmElement = document.getElementById("confirmation-" + volunteerId)
                confirmationBox.removeChild(confirmElement);
            }
            else {

            }
        })
        .catch(err => {
            console.log(err)
        })
}

export function denyVolunteer(e) {
    const volunteerId = e.target.dataset.volunteerid
    const confirmation = fetch("volunteer/DenyVolunteer/" + volunteerId, { method: "post" })
        .then(res => console.log(res.ok))
        .catch(err => {

        })

}






window.onload = () => {
    document.querySelectorAll(".v-btn-confirm").forEach(s => s.addEventListener("click", confirmVolunteer))
    document.querySelectorAll(".v-btn-deny").forEach(s => s.addEventListener("click", denyVolunteer))
}