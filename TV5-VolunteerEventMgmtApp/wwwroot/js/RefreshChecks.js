function RefreshChecks(container_ID, URI, showNoDataMsg, noDataMsg, fadeOutIn) {
    // Select the container by its ID.
    var container = $("#" + container_ID);

    // Use jQuery's shorthand document-ready wrapper to fetch the JSON.
    $(function () {
        $.getJSON(URI, function (data) {
            // If data exists and is not an empty object...
            if (data !== null && !jQuery.isEmptyObject(data)) {
                // Clear the container
                container.empty();

                // For each item returned in the JSON...
                $.each(data, function (index, item) {
                    // Build a unique id for the checkbox.
                    var checkboxId = 'checkbox-' + item.value;

                    // Create the checkbox input.
                    var checkbox = $('<input/>', {
                        type: 'checkbox',
                        id: checkboxId,
                        name: 'selectedLocations',
                        class: 'form-check-input mb-2',
                        value: item.value
                    });

                    // If the JSON indicates that this item should be selected, mark it as checked.
                    if (item.selected) {
                        checkbox.prop('checked', true);
                    }

                    // Create the label for the checkbox.
                    var label = $('<label/>', {
                        "class": "form-check-label",
                        for: checkboxId,
                        text: item.text
                    });

                    // Wrap the checkbox and label in a container.
                    // Here we use Bootstrap’s classes for a toggle-style checkbox.
                    var wrapper = $('<div/>', {
                        "class": "form-check form-switch mb-2"
                    });
                    wrapper.append(checkbox).append(label);

                    // Append the wrapper to the main container.
                    container.append(wrapper);
                });
            } else {
                // If no data is returned and we want to show a message...
                if (showNoDataMsg) {
                    container.empty();
                    noDataMsg = noDataMsg || 'No Matching Data';
                    // Append a paragraph with the message.
                    container.append($('<p/>', { text: noDataMsg }));
                }
            }
        });
    });

    // Optionally add a fade out/in effect
    if (fadeOutIn) {
        container.fadeToggle(400, function () {
            container.fadeToggle(400);
        });
    }
}