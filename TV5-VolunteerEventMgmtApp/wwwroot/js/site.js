
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
function dragVolunteer(ev, volunteerId, fromTimeslotId = null) {
    ev.stopPropagation();
    ev.dataTransfer.setData("volunteerId", volunteerId);
    ev.dataTransfer.setData("fromTimeslotId", fromTimeslotId);

    if (fromTimeslotId !== null) {
        fetch('/VolunteerAttendees/RemoveVolunteer', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ volunteerId: volunteerId, newTimeslotId: fromTimeslotId })
        })
            .then(response => {
                if (!response.ok) {


                    throw new Error("Failed to remove volunteer");
                }
                refreshTimeslot(fromTimeslotId);

            })
            .catch(error => console.error(error))
    }
   
}

function allowDrop(ev) {
    ev.preventDefault();
}

function handleDrop(ev, toTimeslotId, openSlots, occupied) {
    ev.preventDefault();

    const volunteerId = ev.dataTransfer.getData("volunteerId");
   
        const fromTimeslotId = ev.dataTransfer.getData("fromTimeslotId");
    
   

    console.log(`Dropping volunteer ${volunteerId} from timeslot ${fromTimeslotId} to ${toTimeslotId}`);
    if (occupied && fromTimeslotId === "null") return;

    if (occupied && fromTimeslotId !== "null") {
        fetch('/VolunteerAttendees/quickCreate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                volunteerId: volunteerId,
                newTimeslotId: parseInt(fromTimeslotId)
            })
        })
            .then(response => {
                if (!response.ok) {
                   
                    throw new Error('Network response was not ok');
                }
                refreshTimeslot(toTimeslotId);
                refreshTimeslot(fromTimeslotId);
            })
            .catch(error => console.error('Error:', error));
     
       
    }

    // If user drops onto the same timeslot, do nothing
    if (fromTimeslotId === toTimeslotId) {
        console.log("Same timeslot. No move needed.");
        fetch('/VolunteerAttendees/quickCreate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                volunteerId: volunteerId,
                newTimeslotId: parseInt(fromTimeslotId)
            })
        })
            .then(response => {
                if (!response.ok) {
                   
                    throw new Error('Network response was not ok');
                }
                refreshTimeslot(fromTimeslotId);
                refreshTimeslot(toTimeslotId);
            })
            .catch(error => console.error('Error:', error));
     
        return;
    }

    let allowed = isDropAllowed(openSlots);
    if (!allowed) {
        console.log("Drop not allowed. Returning volunteer to original timeslot." + openSlots);
        // Refresh the original timeslot so that the volunteer returns
        fetch('/VolunteerAttendees/quickCreate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                volunteerId: volunteerId,
                newTimeslotId: parseInt(fromTimeslotId)
            })
        })
            .then(response => {
                if (!response.ok) {
                  
                    throw new Error('Network response was not ok');
                }
              
                refreshTimeslot(fromTimeslotId);
            })
            .catch(error => console.error('Error:', error));
     
        return;
    }

    // Move or Create logic
    if (fromTimeslotId === 'null') {
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
                refreshTimeslot(toTimeslotId);

            
            })
            .catch(error => console.error('Error:', error));
      
    } else {
        
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


                    refreshTimeslot(fromTimeslotId);
                    refreshTimeslot(toTimeslotId);
               
            })
            .catch (error => console.error('Error:', error));
    
        
    }
}

// ==================== VOLUNTEER PANEL TOGGLE ====================
function toggleVolunteerPanel(eventId, locationId) {
    console.log("testing");
    const panelEl = document.getElementById(`timeslots-${eventId}`);
    if (!panelEl) return;

    const bsCollapse = new bootstrap.Collapse(panelEl, { toggle: false });

    if (panelEl.classList.contains("show")) {
        bsCollapse.hide();
    } else {
        enableEditMode(eventId)
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
            refreshTimeslot(timeslotId);
        })
        .catch(error => console.error(error))
        .finally(() => {

            hideVolunteerMenu();
        });
}

// ==================== OPTIONAL: HIGHLIGHT EVENT ====================
function highlightEvent(eventId) {
    document.querySelectorAll(`.event-card`).forEach(card => {
        const thisEventId = parseInt(card.getAttribute('data-eventid'));
        if (thisEventId === eventId) {
            card.classList.remove('dimmed-event');
        } else {
            card.classList.add('dimmed-event');
        }
    });
}


function enableEditMode(eventId) {

    document.querySelectorAll('.event-card-container[data-eventid]').forEach(card => {
        const thisId = card.getAttribute('data-eventid');
        if (parseInt(thisId) !== eventId) {
            card.classList.add('dimmed-event');
            card.style.pointerEvents = 'none';
        } else {
            card.classList.remove('dimmed-event');
            card.style.pointerEvents = 'auto';
        }
    });
   
}

function cancelEditMode() {
    window.location = window.location.pathname + "?isEditMode=false";
}


function createTimeslot(eventId) {
    console.log("Creating new timeslot for event:", eventId);
    // Possibly open a modal or inline form to specify start/end time, capacity, etc.
}


function addVolunteerDblClick(event, eventId) {
    // in here send the fetch with the timeslotId and check if the logged in user exsists within that location if they do then complete the add.
}

function cancelEdit(eventId, locationId) {
    // Re-fetch the read-only partial
    $.ajax({
        url: '/VolunteerEvent/GetReadOnlyPartial?id=' + eventId,
        method: 'GET'
    }).done(function (html) {
        toggleVolunteerPanel(eventId, locationId)
        $('#eventContainer-' + eventId).html(html);
    });
}

// Open the edit partial for a timeslot
function editTimeslot(timeslotId) {
    console.log(`editing the time slot at ${timeslotId}`)
    $.ajax({
        url: '/VolunteerSignup/GetEditPartial?id=' + timeslotId,
        type: 'GET',
        success: function (html) {
            // Replace the entire timeslot row with the edit partial
            $('#timeslot-' + timeslotId).replaceWith(html);
        },
        error: function (err) {
            console.error('Error loading timeslot edit partial:', err);
        }
    });
}

// Cancel editing a timeslot by reloading the read-only partial
function cancelTimeslotEdit(timeslotId) {
    $.ajax({
        url: '/VolunteerSignup/GetReadOnlyPartial?id=' + timeslotId,
        type: 'GET',
        success: function (html) {
            $('#timeslot-' + timeslotId).replaceWith(html);
        },
        error: function (err) {
            console.error('Error loading timeslot read-only partial:', err);
        }
    });
}

// Save the new start/end times for the timeslot
function saveTimeslotEdit(timeslotId) {
    var newStart = $('#ts-start-' + timeslotId).val();
    var newEnd = $('#ts-end-' + timeslotId).val();

    // Post the new times to the server; adjust the URL as needed
    $.ajax({
        url: '/VolunteerSignup/Edit/' + timeslotId,
        type: 'POST',
        data: {
            // Assuming your model binds ArrivalTime and DepartureTime
            StartTime: newStart,
            EndTime: newEnd
        },
        success: function (html) {
            // Replace the timeslot row with the updated read-only partial
            $('#timeslot-' + timeslotId).replaceWith(html);
        },
        error: function (err) {
            console.error('Error saving timeslot edit:', err);
        }
    });
}
function createTimeslot(eventId) {
    console.log("Creating new timeslot for event:", eventId);
    $('#timeslotButtonContainer-' + eventId).empty();
    // AJAX call to fetch the create-timeslot partial
    $.ajax({
        url: '/VolunteerSignup/CreatePartial',
        type: 'GET',
        data: { eventId: eventId },
        success: function (html) {
            // Insert the returned HTML into a designated container within the event card.
            // Ensure your event card has an element with id "newTimeslotContainer-{eventId}".
            $('#newTimeslotContainer-' + eventId).html(html);
            $('#timeslotButtonContainer-' + eventId).html(`<div class="col-12 mt-2">
                        <button class="btn btn-primary" type="button" onclick="createTimeslot(@Model.Id)">Add Timeslot</button>
                    </div>`);
        },
        error: function (err) {
            console.error("Error loading create timeslot partial:", err);
        }
    });
}

function cancelCreateTimeslot(eventId) {
    // Simply clear the container for new timeslot creation.
    $('#newTimeslotContainer-' + eventId).empty();
   
}

function refreshTimeslot(timeslotId) {
    $(`#timeslot-${timeslotId} [data-bs-toggle="tooltip"]`).each(function () {
        let tooltipInstance = bootstrap.Tooltip.getInstance(this);
        if (tooltipInstance) {
            tooltipInstance.dispose();
        }
    });

    $.ajax({
        url: '/volunteerSignup/GetReadOnlyPartial',
        type: 'GET',
        data: { id: timeslotId },
        success: function (html) {
            // Replace the entire timeslot container (make sure your partial has an element with id "timeslot-{id}")
            $('#timeslot-' + timeslotId).replaceWith(html);
            // Reinitialize tooltips on the new content.
            var tooltipTriggerList = [].slice.call(document.querySelectorAll(`#timeslot-${timeslotId} [data-bs-toggle="tooltip"]`));
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });
        },
        error: function (err) {
            console.error("Error refreshing timeslot:", err);
        }
    });
}

function refreshTimeslots(eventId) {
   

    $.ajax({
        url: '/VolunteerSignup/GetTimeslotsPartial',
        type: 'GET',
        data: { eventId: eventId },
        success: function (html) {
            
            $('#' + eventId + '-time-slot-container').html(html);
           
        },
        error: function (err) {
            console.error("Error refreshing timeslots:", err);
        }
    });
}

$(document).ready(function () {
    console.log("Attaching delegated submit handler for createTimeslotForm");
    $(document.body).on('submit', 'form[id^="createTimeslotForm-"]', function (e) {
        e.preventDefault();

        var $form = $(this);
        var eventId = $form.find("input[name='VolunteerEventId']").val();
        console.log("Intercepted createTimeslotForm submit for event:", eventId);
        $.ajax({
            url: $form.attr('action'),
            type: $form.attr('method'),
            data: $form.serialize(),
            complete: function (jqXHR, textStatus) {
                console.log("AJAX complete. Status:", textStatus);
               
                refreshTimeslots(eventId);
               
                $('#newTimeslotContainer-' + eventId).empty();
            },
            error: function (err) {
                console.error("Error creating timeslot:", err);
            }
        });
    });
});

function isDropAllowed(openSlots) {
    if (openSlots > 0) {
        return true;
    }
    return false;
}
