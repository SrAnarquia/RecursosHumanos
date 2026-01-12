window.ReclutamientoOverlay = {
    open() {
        document.getElementById('reclutamientoOverlay').classList.add('show');
        document.body.classList.add('overflow-hidden');
    },
    close() {
        document.getElementById('reclutamientoOverlay').classList.remove('show');
        document.body.classList.remove('overflow-hidden');
    }
};