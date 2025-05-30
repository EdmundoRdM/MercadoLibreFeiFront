document.querySelectorAll(".agregar-carrito").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();

            const id = parseInt(this.dataset.id);
            const cantidad = parseInt(this.dataset.cantidad) || 1;
            const titulo = this.dataset.titulo;
            const precio = parseFloat(this.dataset.precio);
            const categorias = this.dataset.categorias.split(",");
            const archivoId = this.dataset.archivoid || null;

            let carrito = JSON.parse(localStorage.getItem("carrito")) || [];

            const existente = carrito.find(p => p.productoid === id);
            if (existente) {
                existente.cantidad += cantidad;
            } else {
                carrito.push({
                    productoid: id,
                    cantidad: cantidad,
                    titulo: titulo,
                    precio: precio,
                    categorias: categorias,
                    archivoId: archivoId
                });
            }

            localStorage.setItem("carrito", JSON.stringify(carrito));
            alert("Producto agregado al carrito");
        });
    });