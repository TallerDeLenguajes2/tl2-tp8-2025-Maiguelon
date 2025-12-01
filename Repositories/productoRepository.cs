using Microsoft.Data.Sqlite;
using presupuestario;
using tl2_tp8_2025_Maiguelon.Interfaces;

public class ProductoRepository : IProductoRepository
{
    // Cadena de conexión para SQLite
    private string connectionString = "Data Source=db/tiendadb.db";

    public int Alta(Producto producto)
    {
        int nuevoId = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"INSERT INTO Productos (Descripcion, Precio) 
                            VALUES (@desc, @prec); 
                            SELECT MAX(idProducto) FROM Productos";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@desc", producto.Descripcion);
                command.Parameters.AddWithValue("@prec", producto.Precio);
                nuevoId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return nuevoId;
    }

    public bool Modificar(int id, Producto producto)
    {
        int filasAfectadas = 0; // para poder devolver bool al final
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"UPDATE Productos SET Descripcion = @desc, Precio = @prec 
                Where idProducto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@desc", producto.Descripcion);
                command.Parameters.AddWithValue("@prec", producto.Precio);
                command.Parameters.AddWithValue("@id", id);

                filasAfectadas = command.ExecuteNonQuery();
            }
        }
        return filasAfectadas > 0;
    }

    public List<Producto> Listar()
    {
        // Devuelvo una lista de productos, no int o bool
        List<Producto> listaProductos = new List<Producto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"SELECT idProducto, Descripcion, Precio 
                    FROM Productos";

            using (var command = new SqliteCommand(sql, connection))
            {
                // Comando que devuelve las filas
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) // mientras haya filas para leer
                    {
                        // Mapear la fila del reader al objeto producto
                        Producto producto = new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Precio = reader.GetInt32(2),
                        };

                        listaProductos.Add(producto);
                    }
                }
            }
        }
        return listaProductos;
    }

    public Producto obtenerProducto(int ide)
    {
        Producto product = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"SELECT idProducto, Descripcion, Precio
                            FROM Productos
                            WHERE idProducto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", ide);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // si lee algo
                    {
                        product = new Producto()
                        {
                            IdProducto = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Precio = reader.GetInt32(2),
                        };
                    }
                }
            }
        }
        return product;
    }

    public bool EliminarProducto(int ide)
    {
        int filaAfectada = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"DELETE FROM PRODUCTOS
                    WHERE idProducto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", ide);
                filaAfectada = command.ExecuteNonQuery();
            }
        }
        return filaAfectada > 0;
    }
}
