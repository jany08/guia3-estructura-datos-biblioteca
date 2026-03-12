using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Libro
{
    public string Codigo { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string Categoria { get; set; }
    public int Anio { get; set; }

    public Libro(string codigo, string titulo, string autor, string categoria, int anio)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        Anio = anio;
    }

    public override string ToString()
    {
        return $"Código: {Codigo} | Título: {Titulo} | Autor: {Autor} | Categoría: {Categoria} | Año: {Anio}";
    }
}

class Program
{
    static Dictionary<string, Libro> biblioteca = new Dictionary<string, Libro>();
    static HashSet<string> categorias = new HashSet<string>();
    static HashSet<string> autores = new HashSet<string>();

    static void Main(string[] args)
    {
        CargarDatosIniciales();

        int opcion;
        do
        {
            Console.Clear();
            MostrarMenu();
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            Console.Clear();

            switch (opcion)
            {
                case 1:
                    RegistrarLibro();
                    break;
                case 2:
                    MostrarLibros();
                    break;
                case 3:
                    BuscarLibroPorCodigo();
                    break;
                case 4:
                    MostrarCategorias();
                    break;
                case 5:
                    MostrarAutores();
                    break;
                case 6:
                    EliminarLibro();
                    break;
                case 7:
                    MostrarReporteGeneral();
                    break;
                case 8:
                    AnalizarTiempoBusqueda();
                    break;
                case 9:
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            if (opcion != 9)
            {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 9);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("   SISTEMA DE REGISTRO DE LIBROS - BIBLIOTECA ");
        Console.WriteLine("==============================================");
        Console.WriteLine("1. Registrar libro");
        Console.WriteLine("2. Mostrar todos los libros");
        Console.WriteLine("3. Buscar libro por código");
        Console.WriteLine("4. Mostrar categorías");
        Console.WriteLine("5. Mostrar autores");
        Console.WriteLine("6. Eliminar libro");
        Console.WriteLine("7. Reporte general");
        Console.WriteLine("8. Analizar tiempo de búsqueda");
        Console.WriteLine("9. Salir");
        Console.WriteLine("==============================================");
    }

    static void RegistrarLibro()
    {
        Console.WriteLine("=== REGISTRAR LIBRO ===");

        Console.Write("Código: ");
        string codigo = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(codigo))
        {
            Console.WriteLine("El código no puede estar vacío.");
            return;
        }

        if (biblioteca.ContainsKey(codigo))
        {
            Console.WriteLine("Ya existe un libro con ese código.");
            return;
        }

        Console.Write("Título: ");
        string titulo = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Autor: ");
        string autor = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Categoría: ");
        string categoria = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Año de publicación: ");
        if (!int.TryParse(Console.ReadLine(), out int anio))
        {
            Console.WriteLine("Año inválido.");
            return;
        }

        Libro nuevoLibro = new Libro(codigo, titulo, autor, categoria, anio);

        biblioteca[codigo] = nuevoLibro;
        categorias.Add(categoria);
        autores.Add(autor);

        Console.WriteLine("Libro registrado correctamente.");
    }

    static void MostrarLibros()
    {
        Console.WriteLine("=== LISTA DE LIBROS ===");

        if (biblioteca.Count == 0)
        {
            Console.WriteLine("No hay libros registrados.");
            return;
        }

        foreach (var libro in biblioteca.Values)
        {
            Console.WriteLine(libro);
        }
    }

    static void BuscarLibroPorCodigo()
    {
        Console.WriteLine("=== BUSCAR LIBRO ===");
        Console.Write("Ingrese el código del libro: ");
        string codigo = Console.ReadLine()?.Trim() ?? "";

        if (biblioteca.TryGetValue(codigo, out Libro libro))
        {
            Console.WriteLine("Libro encontrado:");
            Console.WriteLine(libro);
        }
        else
        {
            Console.WriteLine("No se encontró un libro con ese código.");
        }
    }

    static void MostrarCategorias()
    {
        Console.WriteLine("=== CATEGORÍAS REGISTRADAS ===");

        if (categorias.Count == 0)
        {
            Console.WriteLine("No hay categorías registradas.");
            return;
        }

        foreach (var categoria in categorias.OrderBy(c => c))
        {
            Console.WriteLine("- " + categoria);
        }
    }

    static void MostrarAutores()
    {
        Console.WriteLine("=== AUTORES REGISTRADOS ===");

        if (autores.Count == 0)
        {
            Console.WriteLine("No hay autores registrados.");
            return;
        }

        foreach (var autor in autores.OrderBy(a => a))
        {
            Console.WriteLine("- " + autor);
        }
    }

    static void EliminarLibro()
    {
        Console.WriteLine("=== ELIMINAR LIBRO ===");
        Console.Write("Ingrese el código del libro a eliminar: ");
        string codigo = Console.ReadLine()?.Trim() ?? "";

        if (biblioteca.Remove(codigo))
        {
            ActualizarConjuntos();
            Console.WriteLine("Libro eliminado correctamente.");
        }
        else
        {
            Console.WriteLine("No existe un libro con ese código.");
        }
    }

    static void MostrarReporteGeneral()
    {
        Console.WriteLine("=== REPORTE GENERAL ===");
        Console.WriteLine($"Total de libros: {biblioteca.Count}");
        Console.WriteLine($"Total de categorías únicas: {categorias.Count}");
        Console.WriteLine($"Total de autores únicos: {autores.Count}");

        if (biblioteca.Count > 0)
        {
            var libroMasAntiguo = biblioteca.Values.OrderBy(l => l.Anio).First();
            var libroMasReciente = biblioteca.Values.OrderByDescending(l => l.Anio).First();

            Console.WriteLine("\nLibro más antiguo:");
            Console.WriteLine(libroMasAntiguo);

            Console.WriteLine("\nLibro más reciente:");
            Console.WriteLine(libroMasReciente);
        }
    }

    static void AnalizarTiempoBusqueda()
    {
        Console.WriteLine("=== ANÁLISIS DE TIEMPO DE BÚSQUEDA ===");

        if (biblioteca.Count == 0)
        {
            Console.WriteLine("No hay libros registrados para analizar.");
            return;
        }

        string codigoPrueba = biblioteca.Keys.First();

        Stopwatch reloj = new Stopwatch();
        reloj.Start();

        biblioteca.TryGetValue(codigoPrueba, out Libro libro);

        reloj.Stop();

        Console.WriteLine($"Código buscado: {codigoPrueba}");
        Console.WriteLine($"Resultado: {(libro != null ? "Encontrado" : "No encontrado")}");
        Console.WriteLine($"Tiempo de búsqueda: {reloj.ElapsedTicks} ticks");
        Console.WriteLine($"Tiempo aproximado: {reloj.Elapsed.TotalMilliseconds} ms");
    }

    static void ActualizarConjuntos()
    {
        categorias.Clear();
        autores.Clear();

        foreach (var libro in biblioteca.Values)
        {
            categorias.Add(libro.Categoria);
            autores.Add(libro.Autor);
        }
    }

    static void CargarDatosIniciales()
    {
        Libro l1 = new Libro("L001", "Cien años de soledad", "Gabriel García Márquez", "Novela", 1967);
        Libro l2 = new Libro("L002", "Don Quijote de la Mancha", "Miguel de Cervantes", "Clásico", 1605);
        Libro l3 = new Libro("L003", "El principito", "Antoine de Saint-Exupéry", "Infantil", 1943);
        Libro l4 = new Libro("L004", "La odisea", "Homero", "Épico", -700);

        biblioteca[l1.Codigo] = l1;
        biblioteca[l2.Codigo] = l2;
        biblioteca[l3.Codigo] = l3;
        biblioteca[l4.Codigo] = l4;

        ActualizarConjuntos();
    }
}

