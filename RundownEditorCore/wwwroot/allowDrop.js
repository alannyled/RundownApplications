﻿function allowDrop(event) {
    event.target.closest("tr").style.borderTop = "5px solid rgba(0,0,255,0.2)";
    event.preventDefault();
}

function leaveDrop(event) {
    event.target.closest("tr").style.borderTop = "none";
}

function endDrop(event) {

    const tbody = event.target.closest("tbody");

    if (tbody) {
        const rows = tbody.querySelectorAll("tr");
        rows.forEach(row => {
            row.style.borderTop = "none";
        });
    }
}