const VacacionesOverlayCreate = {
    open() {
        fetch('/Vacacions/Create')
            .then(response => response.text())
            .then(html => {
                document.getElementById('vacacionesOverlayContainer').innerHTML = html;
                document
                    .getElementById('vacacionesOverlay')
                    .classList.add('show');
            });
    },
    close() {
        document
            .getElementById('vacacionesOverlay')
            .classList.remove('show');
    }
};


document.addEventListener("submit", function (e) {
    if (e.target.id === "formCrearVacacion") {
        e.preventDefault();

        const form = e.target;

        fetch(form.action, {
            method: "POST",
            body: new FormData(form),
            credentials: "same-origin" // 🔥 CLAVE
        })
            .then(response => {
                if (response.ok) {
                    VacacionesOverlayCreate.close();
                    location.reload();
                } else {
                    return response.text();
                }
            })
            .then(html => {
                if (html) {
                    document.getElementById("vacacionesOverlayContainer").innerHTML = html;
                }
            });
    }
});
