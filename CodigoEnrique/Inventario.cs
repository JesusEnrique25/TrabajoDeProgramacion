using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionInventario
{
    public class Inventario
    {
        private List<Producto> productos;

        public Inventario()
        {
            productos = new List<Producto>();
        }

        public void AgregarProductos(Producto producto)
        {
            productos.Add(producto);
        }

        public bool ActualizarPrecio(string nombre, decimal nuevoPrecio)
        {
            var producto = productos.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (producto != null)
            {
                producto.Precio = nuevoPrecio;
                return true;
            }
            return false;
        }

        public bool EliminarProducto(string nombre)
        {
            var producto = productos.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (producto != null)
            {
                productos.Remove(producto);
                return true;
            }
            return false;
        }

        public IEnumerable<Producto> FiltrarYOrdenarProductos(decimal precioMinimo)
        {
            return productos
                .Where(p => p.Precio > precioMinimo)
                .OrderBy(p => p.Precio);
        }

        public Dictionary<string, int> ContarProductosPorRangoDePrecio()
        {
            return new Dictionary<string, int>
            {
                { "Menores a 100", productos.Count(p => p.Precio < 100) },
                { "Entre 100 y 500", productos.Count(p => p.Precio >= 100 && p.Precio <= 500) },
                { "Mayores a 500", productos.Count(p => p.Precio > 500) }
            };
        }

        public (int totalProductos, decimal precioPromedio, Producto productoPrecioMax, Producto productoPrecioMin, List<string> nombresProductos) GenerarReporteInventario()
        {
            int totalProductos = productos.Count;
            decimal precioPromedio = productos.Any() ? productos.Average(p => p.Precio) : 0;
            Producto productoPrecioMax = productos.OrderByDescending(p => p.Precio).FirstOrDefault();
            Producto productoPrecioMin = productos.OrderBy(p => p.Precio).FirstOrDefault();
            List<string> nombresProductos = productos.Select(p => p.Nombre).ToList();

            return (totalProductos, precioPromedio, productoPrecioMax, productoPrecioMin, nombresProductos);
        }
    }
}


