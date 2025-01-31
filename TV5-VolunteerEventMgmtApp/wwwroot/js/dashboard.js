﻿document.addEventListener('DOMContentLoaded', function () {
	var calendarEl = document.getElementById('calendar');
	var calendar = new FullCalendar.Calendar(calendarEl, {
		headerToolbar: {
			left: 'prev,next today',
			center: 'title',
			right: 'dayGridWeek,listWeek'
		},
		initialView: 'dayGridWeek',
		eventDidMount: function (info) {
			// hides the time coz it takes too much space
			const timeElement = info.el.querySelector('.fc-event-time');
			if (timeElement) {
				timeElement.style.display = 'none';
			}
			// change the color based on attendance, kinda how GitHub does the contribution activity
			// var attendancePercentage = info.event.extendedProps.attendancePercentage;
			// if (attendancePercentage) {
			// 	var backgroundColor = '';
			// 	var textColor = '';
			// 	if (attendancePercentage >= 1 && attendancePercentage <= 25) {
			// 		Contrast Ratio: 17.34:1 (WCAG AAA: Pass)
			// 		backgroundColor = '#F6E4F6'; very light purple
			// 		textColor = '#000000';
			// 	} else if (attendancePercentage >= 26 && attendancePercentage <= 50) {
			// 		Contrast Ratio: 12.5:1 (WCAG AAA: Pass)
			// 		backgroundColor = '#EBB7EB'; medium light purple
			// 		textColor = '#000000';
			// 	} else if (attendancePercentage >= 51 && attendancePercentage <= 75) {
			// 		Contrast Ratio: 7.18:1 (WCAG AAA: Pass)
			// 		backgroundColor = '#D86FD8'; base purple
			// 		textColor = '#000000';
			// 	} else if (attendancePercentage >= 76 && attendancePercentage <= 100) {
			// 		Contrast Ratio: 7.29:1 (WCAG AAA: Pass)
			// 		backgroundColor = '#902790'; dark purple
			// 		textColor = '#ffffff';
			// 	}
			// 	info.el.style.backgroundColor = backgroundColor;
			// 	info.el.style.color = textColor;
			// }

			// these are from Location Controller...
			var DefaultColors = ["#4CAF50", "#2196F3", "#9C27B0", "#00BCD4", "#8BC34A", "#607D8B"];

			var locationId = info.event.extendedProps.locationId;

			if (locationId !== undefined) {
				var index = (locationId - 1) % DefaultColors.length;
				var backgroundColor = DefaultColors[index];

				var textColor = getContrastYIQ(backgroundColor);

				info.el.style.backgroundColor = backgroundColor;
				info.el.style.color = textColor;
			}

			// Function to determine the best text color (black or white) based on background color
			function getContrastYIQ(hexcolor) {
				hexcolor = hexcolor.replace("#", "");
				var r = parseInt(hexcolor.substr(0, 2), 16);
				var g = parseInt(hexcolor.substr(2, 2), 16);
				var b = parseInt(hexcolor.substr(4, 2), 16);
				// Calculate the perceptive luminance
				var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
				// Return black or white depending on brightness
				return (yiq >= 128) ? '#000000' : '#ffffff';
			}
		},
		eventClick: function (info) {
			info.jsEvent.preventDefault();

			// Fetch event details
			var event = info.event;

			// Update modal content
			document.getElementById('eventModalLabel').textContent = event.title;
			document.querySelector('#eventModal .modal-body').innerHTML = `
				<p><strong>Start Time:</strong> ${event.start.toLocaleString()}</p>
				<p><strong>End Time:</strong> ${event.end ? event.end.toLocaleString() : 'N/A'}</p>
				<p><strong>Attendance Percentage:</strong> ${event.extendedProps.attendancePercentage}%</p>
			`;

			// Update the details link
			var detailsLink = document.getElementById('detailsLink');
			detailsLink.href = `/AttendanceSheet/Details/${event.id}`;

			$('#eventModal').modal('show');
		},
		datesSet: function (info) {
			var selectedYear = info.start.getFullYear();
			var selectedLocationId = $('#locationDropdown').val();
			loadAttendanceGraph(selectedYear, selectedLocationId);
		}
	});
	calendar.render();

	function loadAttendanceGraph(year, locationId) {
		var url = `/AttendanceSheet/ShowData?year=${year}&locationId=${locationId}`;
		$('#attendance-graph-container').load(url);
	}

	var initialYear = new Date().getFullYear();
	var initialLocationId = $('#locationDropdown').val();
	loadAttendanceGraph(initialYear, initialLocationId);

	$('#locationDropdown').change(function () {
		var selectedYear = calendar.getDate().getFullYear();
		var selectedLocationId = $(this).val();
		loadAttendanceGraph(selectedYear, selectedLocationId);
	});

	var locationDropdown = document.getElementById('locationDropdown');
	var singerCountEl = document.getElementById('singerCount');

	function fetchAllAttendanceData() {
		fetch(`/AttendanceSheet/GetAllAttendance`)
			.then(response => response.json())
			.then(events => {
				calendar.removeAllEvents();
				calendar.addEventSource(events);
			})
			.catch(error => console.error('Error fetching all attendance data:', error));
	}

	function fetchAttendanceData(locationId) {
		fetch(`/AttendanceSheet/GetAttendanceByLocation?locationId=${locationId}`)
			.then(response => response.json())
			.then(events => {
				calendar.removeAllEvents();
				calendar.addEventSource(events);
			})
			.catch(error => console.error('Error fetching attendance data:', error));
	}

	function fetchTotalSingerCount() {
		fetch(`/AttendanceSheet/GetTotalSingerCount`)
			.then(response => response.json())
			.then(count => {
				singerCountEl.textContent = count;
			})
			.catch(error => console.error('Error fetching total singer count:', error));
	}

	function fetchSingerCount(locationId) {
		fetch(`/AttendanceSheet/GetSingerCountByLocation?locationId=${locationId}`)
			.then(response => response.json())
			.then(count => {
				singerCountEl.textContent = count;
			})
			.catch(error => console.error('Error fetching singer count:', error));
	}

	function fetchStatistics(locationId) {
		var url = locationId && locationId !== "all" ? `/AttendanceSheet/GetStatistics?locationId=${locationId}` : `/AttendanceSheet/GetStatistics`;
		fetch(url)
			.then(response => response.json())
			.then(data => {
				if (locationId && locationId !== "all") {
					document.getElementById('specificLocationStats').style.display = 'block';
					document.getElementById('allLocationsStats').style.display = 'none';
					document.getElementById('mean').textContent = data.mean.toFixed(2);
					document.getElementById('percentageAttendance').textContent = data.percentageAttendance.toFixed(2);
				} else {
					document.getElementById('specificLocationStats').style.display = 'none';
					document.getElementById('allLocationsStats').style.display = 'block';

					document.getElementById('highestmean').textContent = data.highestmean.toFixed(2);
					document.getElementById('highestPercentageAttendance').textContent = data.highestPercentageAttendance.toFixed(2);
					document.getElementById('highestmeanLocation').textContent = data.highestMeanLocation;

					document.getElementById('lowestmean').textContent = data.lowestmean.toFixed(2);
					document.getElementById('lowestPercentageAttendance').textContent = data.lowestPercentageAttendance.toFixed(2);
					document.getElementById('lowestmeanLocation').textContent = data.lowestMeanLocation;
				}
			})
			.catch(error => console.error('Error fetching statistics:', error));
	}

	locationDropdown.addEventListener('change', function () {
		var locationId = this.value;
		if (locationId === "all") {
			fetchAllAttendanceData();
			fetchTotalSingerCount();
			fetchStatistics(locationId);
		} else if (locationId) {
			fetchAttendanceData(locationId);
			fetchSingerCount(locationId);
			fetchStatistics(locationId);
		} else {
			calendar.removeAllEvents();
			singerCountEl.textContent = '0';
		}
	});

	fetchAllAttendanceData();
	fetchTotalSingerCount();
	fetchStatistics(locationDropdown.value);
});