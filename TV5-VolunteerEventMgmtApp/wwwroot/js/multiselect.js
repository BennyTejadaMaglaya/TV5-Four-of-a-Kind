let listOfChosen = document.getElementById("selectedOptions");
let listOfAvailable = document.getElementById("availableOptions");
let count = document.getElementById('count');

let originalOptions = Array.from(listOfChosen.options).map(opt => opt.value);

// Function to update the count
function updateCount() {
    count.textContent = listOfChosen.options.length;
}

// Function to move options from a list box to the other
function switchItems(event, senderList, receiverList) {
    let senderID = senderList.id;
    let selectedOptions = document.querySelectorAll(`#${senderID} option:checked`);
    event.preventDefault();

    if (selectedOptions.length === 0) {
        alert("No item was selected.");
    } else {
        selectedOptions.forEach(function (opt) {
            senderList.remove(opt.index);
            let inserted = false;
            opt.selected = false;
            // insert in the corresponding alphabetical position
            for (let i = 0; i < receiverList.children.length; i++) {
                if (opt.textContent.localeCompare(receiverList.children[i].textContent) <= 0) { // -1 if opt comes before child 
                    receiverList.insertBefore(opt, receiverList.children[i]); // if so, insert before child
                    inserted = true;
                    break;
                }
            }
            if (!inserted) {
                receiverList.appendChild(opt);
            }
            opt.scrollIntoView()
            updateCount();
        });
    }
}

// Create closures so that we can access the event & the 2 parameters
let addOptions = (event) => switchItems(event, listOfAvailable, listOfChosen);
let removeOptions = (event) => switchItems(event, listOfChosen, listOfAvailable);

// Assign the closures as the event handlers for each button
document.getElementById("btnAdd").addEventListener("click", addOptions);
document.getElementById("btnRemove").addEventListener("click", removeOptions);

updateCount(); // Update count of items in the class when page loads

document.getElementById("btnSubmit").addEventListener("click", function () {
    listOfChosen.childNodes.forEach(opt => opt.selected = "selected");
});
