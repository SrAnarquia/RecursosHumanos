window.ReclutamientoOverlay = {

    // ===================== CREATE =====================
    create() {
        // Eliminar cualquier modal previo
        const old = document.getElementById('reclutamientoOverlay');
        if (old) old.remove();

        // Clonar el Create original limpio desde el template oculto
        const container = document.getElementById('create-modal-container');
        if (!container) return;

        const html = container.innerHTML;
        document.body.insertAdjacentHTML("beforeend", html);

        this.open();
        this.attachCloseEvents();
    },

    // ===================== EDIT =====================
    edit(id) {
        // Eliminar cualquier modal previo
        const old = document.getElementById('reclutamientoOverlay');
        if (old) old.remove();

        fetch(`/DatosReclutamientoes/Edit/${id}`)
            .then(r => r.text())
            .then(html => {
                document.body.insertAdjacentHTML("beforeend", html);
                this.open();
                this.attachCloseEvents();
            });
    },

    // ===================== OPEN =====================
    open() {
        const modal = document.getElementById('reclutamientoOverlay');
        if (!modal) return;

        modal.classList.add('show');
        document.body.classList.add('overflow-hidden');
    },

    // ===================== CLOSE =====================
    close() {
        const modal = document.getElementById('reclutamientoOverlay');
        if (!modal) return;

        modal.remove(); // Elimina del DOM
        document.body.classList.remove('overflow-hidden');
    },

    // ===================== EVENTOS DE CIERRE =====================
    attachCloseEvents() {
        const modal = document.getElementById('reclutamientoOverlay');
        if (!modal) return;

        // Cerrar con botón "Cancelar" o "X"
        const closeButtons = modal.querySelectorAll('.btn-close, .btn-secondary');
        closeButtons.forEach(btn => {
            btn.addEventListener('click', () => this.close());
        });

        // Cerrar haciendo click fuera del modal
        modal.addEventListener('click', e => {
            if (e.target === modal) this.close();
        });
    }
};
