// Global variables (do NOT redeclare them inside DOMContentLoaded)
let currentOpenEventId = null;
let contextTimeslotId = null;
let contextVolunteerId = null;

// ==================== DOM LOADED ====================
document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Hide volunteer menu on click outside or ESC
    document.addEventListener('click', function () {
        hideVolunteerMenu();
    });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            hideVolunteerMenu();
        }
    });

    // Attach event to "Remove Volunteer" menu item
    const removeBtn = document.getElementById('removeMenuItem');
    if (removeBtn) {
        removeBtn.addEventListener('click', function () {
            removeVolunteer(contextTimeslotId, contextVolunteerId);
        });
    }

    // Prevent dragging from inside volunteer-panel
    document.addEventListener('dragstart', function (event) {
        if (event.target.closest('.volunteer-panel')) {
            event.preventDefault();
        }
    });

   
});

// ==================== DRAG & DROP ====================
function dragVolunteer(ev, volunteerId, fromTimeslotId) {
    ev.stopPropagation();
    ev.dataTransfer.setData("volunteerId", volunteerId);
    ev.dataTransfer.setData("fromTimeslotId", fromTimeslotId);
}

function allowDrop(ev) {
    ev.preventDefault();
}

function handleDrop(ev, toTimeslotId) {
    ev.preventDefault();

    const volunteerId = ev.dataTransfer.getData("volunteerId");
    const fromTimeslotId = ev.dataTransfer.getData("fromTimeslotId");

    console.log(`Dropping volunteer ${volunteerId} from timeslot ${fromTimeslotId} to ${toTimeslotId}`);

    // If user drops onto the same timeslot, do nothing
    if (fromTimeslotId === toTimeslotId) {
        console.log("Same timeslot. No move needed.");
        return;
    }

    // Move or Create logic
    if (fromTimeslotId) {
        // If fromTimeslotId is present, do a "move"
        fetch('/VolunteerAttendees/quickMove', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                volunteerId: volunteerId,
                oldTimeslotId: parseInt(fromTimeslotId),
                newTimeslotId: parseInt(toTimeslotId)
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                location.reload();
            })
            .catch(error => console.error('Error:', error));
    } else {
        // If fromTimeslotId is null/undefined, do a "create"
        fetch('/VolunteerAttendees/quickCreate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                volunteerId: volunteerId,
                newTimeslotId: parseInt(toTimeslotId)
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                location.reload();
            })
            .catch(error => console.error('Error:', error));
    }
}

// ==================== VOLUNTEER PANEL TOGGLE ====================
function toggleVolunteerPanel(eventId, locationId) {
    const panelEl = document.getElementById(`timeslots-${eventId}`);
    if (!panelEl) return;

    const bsCollapse = new bootstrap.Collapse(panelEl, { toggle: false });

    if (panelEl.classList.contains("show")) {
        bsCollapse.hide();
    } else {
        loadVolunteers(eventId, locationId); 
        bsCollapse.show();
    }

 
              
}


function loadVolunteers(eventId, locationId) {
    const volunteersListEl = document.getElementById(`volunteersList-${eventId}`);
    if (!volunteersListEl) return;

    volunteersListEl.innerHTML = '<p>Loading volunteers...</p>';

    // Example fetch. Adjust URL as needed
    fetch(`/VolunteerEvent/GetVolunteersByLocation?locationId=${locationId}`)
        .then(response => response.ok ? response.text() : Promise.reject("Failed to load."))
        .then(html => {
            volunteersListEl.innerHTML = html;
        })
        .catch(err => {
            console.error(err);
            volunteersListEl.innerHTML = '<p>Could not load volunteers.</p>';
        });
}

// ==================== CONTEXT MENU LOGIC ====================
function showVolunteerMenu(ev, timeslotId, volunteerId) {
    ev.preventDefault();

    contextTimeslotId = timeslotId;
    contextVolunteerId = volunteerId;

    const menu = document.getElementById('volunteerContextMenu');
    if (!menu) return;
    menu.classList.remove('d-none');

    menu.style.top = ev.clientY + 'px';
    menu.style.left = ev.clientX + 'px';
}

function hideVolunteerMenu() {
    const menu = document.getElementById('volunteerContextMenu');
    if (!menu) return;
    menu.classList.add('d-none');
    contextTimeslotId = null;
    contextVolunteerId = null;
}

function volunteerKeyDown(ev, timeslotId, volunteerId) {
    // SHIFT+F10 or ContextMenu key
    if ((ev.key === 'F10' && ev.shiftKey) || ev.key === 'ContextMenu') {
        ev.preventDefault();
        showVolunteerMenu(ev, timeslotId, volunteerId);
    }
}

function removeVolunteer(timeslotId, volunteerId) {
    if (!confirm("Remove this volunteer from the timeslot?")) {
        hideVolunteerMenu();
        return;
    }

    // Example remove endpoint. Adjust as needed
    fetch('/VolunteerAttendees/RemoveVolunteer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ volunteerId: volunteerId, newTimeslotId: timeslotId })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to remove volunteer");
            }

            // ==================  FIGURE OUT HOW TO ONLY RELOAD JUST THIS TIMESLOT TO PREVENT THE PANEL FROM CLOSING  ===========
            location.reload();
        })
        .catch(error => console.error(error))
        .finally(() => {
            hideVolunteerMenu();
        });
}

// ==================== OPTIONAL: HIGHLIGHT EVENT ====================
function highlightEvent(eventId) {
    document.querySelectorAll('.event-card[data-eventid]').forEach(card => {
        const thisEventId = parseInt(card.getAttribute('data-eventid'));
        if (thisEventId === eventId) {
            card.classList.remove('dimmed-event');
        } else {
            card.classList.add('dimmed-event');
        }
    });
}
