using Microsoft.Data.Sqlite;
using MVC.Models;
using MVC.Interfaces;

namespace MVC.Repositorios;
    public class ProductoRepository : IProductoRepository
    {
        private readonly string CadenaConexion = "Data Source=./DB/tienda.db"; 

        // Constructor Síncrono (sin parámetros, necesario para la instanciación directa en el Controller)
        public ProductoRepository()
        {
        }

        public List<Producto> GetAll()
        {
            var listaProductos = new List<Producto>();

            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos";
            using var comando = new SqliteCommand(sql, conexion);
            
            // ExecuteReader se usa para consultas que devuelven filas (SELECT)
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                listaProductos.Add(new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetDecimal(2)
                });
            }
            return listaProductos;
        }

        public Producto GetById(int id)
        {
            Producto producto = null;

            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @Id";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                producto = new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetDecimal(2)
                };
            }
            return producto;
        }

        public void Add(Producto producto)
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
            using var comando = new SqliteCommand(sql, conexion);

            // Uso de parámetros para prevenir la inyección SQL (práctica esencial de seguridad)
            comando.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            comando.Parameters.AddWithValue("@Precio", producto.Precio);

            // ExecuteNonQuery se usa para comandos que no devuelven filas (INSERT, UPDATE, DELETE)
            comando.ExecuteNonQuery();
        }

        public void Update(Producto producto)
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE IdProducto = @Id";
            using var comando = new SqliteCommand(sql, conexion);

            comando.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            comando.Parameters.AddWithValue("@Precio", producto.Precio);
            comando.Parameters.AddWithValue("@Id", producto.IdProducto);

            comando.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();

            string sql = "DELETE FROM Productos WHERE IdProducto = @Id";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);

            comando.ExecuteNonQuery();
        }
    }
