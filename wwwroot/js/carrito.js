const apiBaseUrl = document.getElementById("carrito-wrapper").dataset.apiBaseUrl;

// Borrar solo si venimos de una compra real
    if (sessionStorage.getItem("compra_en_proceso") === "true") {
        localStorage.removeItem("carrito");
        sessionStorage.removeItem("compra_en_proceso");
    }    

    document.addEventListener("DOMContentLoaded", async function () {


        const carritoLocal = JSON.parse(localStorage.getItem("carrito")) || [];

        const respuesta = await fetch('/Carrito/ObtenerCarritoActualizado', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(carritoLocal)
        });

        if (respuesta.ok) {
            const carritoActualizado = await respuesta.json();
            console.log("Carrito actualizado:", carritoActualizado);
            localStorage.setItem("carrito", JSON.stringify(carritoActualizado));
        } else {
            const errorText = await respuesta.text();
            console.error("Error al obtener el carrito actualizado:", errorText);
        }
        

        const contenedor = document.getElementById("contenedor-carrito");
        const alerta = document.getElementById("alerta-vacio");
        const resumen = document.getElementById("resumen-carrito");
        const carritoJsonInput = document.getElementById("carritoJson");
        const btnFinalizar = document.querySelector("#form-carrito button");

        let carrito = JSON.parse(localStorage.getItem("carrito")) || [];

        if (carrito.length === 0) {
            alerta.style.display = "block";
            resumen.innerText = "Mostrando 0 elementos";
            if (btnFinalizar) btnFinalizar.style.display = "none";
            return;
        }

        alerta.style.display = "none";
        if (btnFinalizar) btnFinalizar.style.display = "inline-block";

        let totalGeneral = 0;

        carrito.forEach(producto => {
            const total = (producto.precio * producto.cantidad).toFixed(2);
            totalGeneral += parseFloat(total);

            const categoriasHTML = (producto.categorias || []).map(c =>
                `<span class="badge rounded-pill text-bg-warning">${c}</span>`
            ).join(" ");

            const card = document.createElement("div");
            card.className = "col";
            card.dataset.id = producto.productoid;

            let imagenUrl = "https://placehold.co/300x200/FFF/999?text=Artículo";
            if (producto.archivoId) {
                imagenUrl = `${apiBaseUrl}/api/archivos/${producto.archivoId}`;
            }

            card.innerHTML = `
                <div class="card h-100">
                    <img src="${imagenUrl}"
                         alt="${producto.titulo}"
                         class="portada card-img-top"
                         style="max-height: 200px;" />

                    <div class="card-body d-flex flex-column">
                        <p class="card-text">${producto.titulo}</p>
                        <h5 class="card-title">$${producto.precio.toFixed(2)}</h5>
                        <p class="card-text">${categoriasHTML}</p>
                        <p class="card-text fw-bold text-success mt-2">
                            Cantidad: ${producto.cantidad} <br/>
                            Total: $${total}
                        </p>
                        <p class="mt-auto card-text text-success fw-semibold small">
                            <i class="bi bi-lightning-fill"></i> Envío gratis
                        </p>
                    </div>

                    <div class="card-footer">
                        <a class="btn btn-danger btn-sm eliminar-producto" href="#" data-id="${producto.productoid}">Eliminar</a>
                    </div>
                </div>
            `;

            contenedor.appendChild(card);
        });

        resumen.innerText = `Mostrando ${carrito.length} producto(s) - Total: $${totalGeneral.toFixed(2)}`;

        contenedor.addEventListener("click", function (e) {
            if (e.target.classList.contains("eliminar-producto")) {
                e.preventDefault();

                const id = parseInt(e.target.dataset.id);
                carrito = carrito.filter(p => p.productoid !== id);
                localStorage.setItem("carrito", JSON.stringify(carrito));

                const card = e.target.closest(".col");
                if (card) {
                    card.remove();
                }

                // Recalcular total
                let nuevoTotal = 0;
                carrito.forEach(p => {
                    nuevoTotal += p.precio * p.cantidad;
                });

                resumen.innerText = `Mostrando ${carrito.length} producto(s) - Total: $${nuevoTotal.toFixed(2)}`;

                if (carrito.length === 0) {
                    alerta.style.display = "block";
                    if (btnFinalizar) btnFinalizar.style.display = "none";
                }
            }
        });

        // Preparar datos para enviar en el formulario
        document.getElementById("form-carrito").addEventListener("submit", function (e) {
            const carrito = JSON.parse(localStorage.getItem("carrito")) || [];

            // Convertir archivoId de string a número si existe
            carrito.forEach(p => {
                if (typeof p.archivoId === "string") {
                    const id = parseInt(p.archivoId);
                    p.archivoId = isNaN(id) ? null : id;
                }
            });

            document.getElementById("carritoJson").value = JSON.stringify(carrito);
            sessionStorage.setItem("compra_en_proceso", "true");
        });
    });
