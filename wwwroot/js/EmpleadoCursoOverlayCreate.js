const CursoOverlay = {

    async open(idPersona) {

        const response = await fetch(`/Empleados/CreateCurso?idPersona=${idPersona}`);
        const html = await response.text();

        const container = document.getElementById("modalCursosContainer");
        container.innerHTML = html;
    },

    close() {
        const overlay = document.getElementById("cursoOverlay");
        if (overlay) overlay.remove();
    }
};
