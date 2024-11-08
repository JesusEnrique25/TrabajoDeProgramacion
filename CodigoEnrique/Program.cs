using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionInventario
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Inventario inventario = new Inventario();
                Console.WriteLine("Bienvenido al sistema de gestion de inventario.");

                Console.WriteLine("Cuantos productos desea ingresar ?");
                int cantidad;
                while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0)
                {
                    Console.WriteLine("Ingrese un número valido y positivo para la cantidad.");
                }

                for (int i = 0; i < cantidad; i++)
                {
                    Console.WriteLine($"\nProducto {i + 1}:");

                    string nombre;
                    do
                    {
                        Console.Write("Nombre: ");
                        nombre = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nombre))
                        {
                            Console.WriteLine("El nombre del producto no puede estar vacío. Por favor, ingrese un nombre válido.");
                        }
                    } while (string.IsNullOrWhiteSpace(nombre));

                    decimal precio;
                    do
                    {
                        Console.Write("Precio: ");
                        if (!decimal.TryParse(Console.ReadLine(), out precio) || precio <= 0)
                        {
                            Console.WriteLine("Por favor, ingrese un precio válido y positivo.");
                        }
                    } while (precio <= 0);

                    Producto producto = new Producto(nombre, precio);
                    inventario.AgregarProductos(producto);
                }

                Console.WriteLine("\nIngrese el precio mínimo para filtrar los productos: ");
                decimal precioMinimo;
                while (!decimal.TryParse(Console.ReadLine(), out precioMinimo) || precioMinimo < 0)
                {
                    Console.WriteLine("Por favor, ingrese un precio mínimo válido (mayor o igual a 0).");
                }

                var productosFiltrados = inventario.FiltrarYOrdenarProductos(precioMinimo);

                Console.WriteLine("\nProductos filtrados y ordenados:");
                foreach (var producto in productosFiltrados)
                {
                    producto.MostrarInfo();
                }

                if (PreguntarUsuario("\n¿Desea actualizar el precio de algún producto? (s/n)"))
                {
                    Console.Write("Ingrese el nombre del producto que desea actualizar: ");
                    string nombreProducto = Console.ReadLine();

                    decimal nuevoPrecio;
                    do
                    {
                        Console.Write("Ingrese el nuevo precio: ");
                        if (!decimal.TryParse(Console.ReadLine(), out nuevoPrecio) || nuevoPrecio <= 0)
                        {
                            Console.WriteLine("Por favor, ingrese un precio válido y positivo.");
                        }
                    } while (nuevoPrecio <= 0);

                    bool actualizado = inventario.ActualizarPrecio(nombreProducto, nuevoPrecio);

                    if (actualizado)
                    {
                        Console.WriteLine($"El precio de '{nombreProducto}' ha sido actualizado a {nuevoPrecio:C}.");
                    }
                    else
                    {
                        Console.WriteLine($"Producto '{nombreProducto}' no encontrado.");
                    }
                }

                if (PreguntarUsuario("\n¿Desea eliminar algún producto? (s/n)"))
                {
                    Console.Write("Ingrese el nombre del producto que desea eliminar: ");
                    string nombreProductoEliminar = Console.ReadLine();

                    bool eliminado = inventario.EliminarProducto(nombreProductoEliminar);

                    if (eliminado)
                    {
                        Console.WriteLine($"El producto '{nombreProductoEliminar}' ha sido eliminado.");
                    }
                    else
                    {
                        Console.WriteLine($"Producto '{nombreProductoEliminar}' no encontrado.");
                    }
                }

                Console.WriteLine("\nConteo de productos por rango de precio:");
                var conteoPorRango = inventario.ContarProductosPorRangoDePrecio();

                foreach (var rango in conteoPorRango)
                {
                    Console.WriteLine($"{rango.Key}: {rango.Value}");
                }

                var (totalProductos, precioPromedio, productoPrecioMax, productoPrecioMin, nombresProductos) = inventario.GenerarReporteInventario();

                Console.WriteLine("\nResumen del Inventario:");
                Console.WriteLine($"Número total de productos: {totalProductos}");
                Console.WriteLine($"Precio promedio de los productos: {precioPromedio:C}");

                if (productoPrecioMax != null)
                {
                    Console.WriteLine($"Producto con el precio más alto: {productoPrecioMax.Nombre}, Precio: {productoPrecioMax.Precio:C}");
                }

                if (productoPrecioMin != null)
                {
                    Console.WriteLine($"Producto con el precio más bajo: {productoPrecioMin.Nombre}, Precio: {productoPrecioMin.Precio:C}");
                }

                Console.WriteLine("Productos disponibles:");
                foreach (var nombre in nombresProductos)
                {
                    Console.WriteLine($"- {nombre}");
                }

                Console.WriteLine();

            } while (PreguntarUsuario("¿Desea volver a usar el sistema? (s/n)"));

            Console.WriteLine("Ha salido del sistema");
        }

        static bool PreguntarUsuario(string mensaje)
        {
            string respuesta;
            do
            {
                Console.Write(mensaje);
                respuesta = Console.ReadLine().ToLower();
                if (respuesta != "s" && respuesta != "n")
                {
                    Console.WriteLine("Por favor, ingrese 's' para sí o 'n' para no.");
                }
            } while (respuesta != "s" && respuesta != "n");

            return respuesta == "s";
        }
    }
}
