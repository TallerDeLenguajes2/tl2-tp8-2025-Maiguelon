using Microsoft.Data.Sqlite;
using presupuestario;
using tl2_tp8_2025_Maiguelon.Interfaces;

public class PresupuestoRepository : IPresupuestoRepository
{
    // Cadena de conexión para SQLite
    private string connectionString = "Data Source=db/tiendadb.db";

    public int AltaPresupuesto(Presupuesto presupuesto)
    {
        int nuevoId = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // 1. Insertar Encabezado y Obtener el ID (Tu código)
            string sqlEncabezado = @"
            INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)
            VALUES (@nom, @fec); 
            SELECT last_insert_rowid()"; // Misma función que el select max

            using (var command = new SqliteCommand(sqlEncabezado, connection))
            {
                command.Parameters.AddWithValue("@nom", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@fec", presupuesto.FechaCreacion);
                // ExecuteScalar() ejecuta el INSERT y devuelve el resultado del SELECT (el ID)
                nuevoId = Convert.ToInt32(command.ExecuteScalar());
            }

            // 2. Insertar los Detalles (Bucle ADO.NET)
            string sqlDetalle = @"
            INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad)
            VALUES (@idPres, @idProd, @cant)";

            // El bucle foreach itera sobre la lista de Detalles del modelo
            foreach (var detalle in presupuesto.Detalle!)
            {
                // Creamos un NUEVO comando para cada detalle
                using (var commandDetalle = new SqliteCommand(sqlDetalle, connection))
                {
                    // Parámetros CRUCIALES para enlazar el detalle:
                    commandDetalle.Parameters.AddWithValue("@idPres", nuevoId); // Usamos el ID recién creado
                    commandDetalle.Parameters.AddWithValue("@idProd", detalle.Producto!.IdProducto);
                    commandDetalle.Parameters.AddWithValue("@cant", detalle.Cantidad);

                    // ExecuteNonQuery ejecuta el INSERT y no devuelve nada
                    commandDetalle.ExecuteNonQuery();
                }
            }
        }
        return nuevoId;
    }

    public List<Presupuesto> ListarPresupuestos()
    {
        var lista = new List<Presupuesto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";

            using (var command = new SqliteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Presupuesto
                        {
                            IdPresupuesto = reader.GetInt32(0),
                            NombreDestinatario = reader.GetString(1),
                            FechaCreacion = DateOnly.Parse(reader.GetString(2)),
                            // Inicializo la lista vacía para que no sea null
                            Detalle = new List<PresupuestoDetalle>()
                        };
                        lista.Add(p);
                    }
                }
            }
        }
        return lista;
    }

    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
        Presupuesto presupuesto = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // Usamos INNER JOIN para traer datos de las 3 tablas:
            // Presupuestos (P), PresupuestosDetalle (PD) y Productos (Prod)
            string sql = @"
            SELECT 
                P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion,
                PD.idProducto, PD.Cantidad,
                Prod.Descripcion, Prod.Precio
            FROM Presupuestos P
            INNER JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
            INNER JOIN Productos Prod ON PD.idProducto = Prod.idProducto
            WHERE P.idPresupuesto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 1. Crear el objeto Presupuesto solo una vez (con la primera fila)
                        if (presupuesto == null)
                        {
                            presupuesto = new Presupuesto
                            {
                                IdPresupuesto = reader.GetInt32(0),
                                NombreDestinatario = reader.GetString(1),
                                // Asumiendo que guardaste la fecha como string ISO
                                FechaCreacion = DateOnly.Parse(reader.GetString(2)),
                                Detalle = new List<PresupuestoDetalle>()
                            };
                        }

                        // 2. Por cada fila que leemos, agregamos un detalle a la lista
                        var detalle = new PresupuestoDetalle
                        {
                            Cantidad = reader.GetInt32(4),
                            Producto = new Producto
                            {
                                IdProducto = reader.GetInt32(3),
                                Descripcion = reader.GetString(5),
                                Precio = reader.GetInt32(6)
                            }
                        };
                        presupuesto.Detalle.Add(detalle);
                    }
                }
            }
        }
        // Si no encontró nada, devolverá null
        return presupuesto;
    }

    public void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = @"
            INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
            VALUES (@idPres, @idProd, @cant)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@idPres", idPresupuesto);
                command.Parameters.AddWithValue("@idProd", idProducto);
                command.Parameters.AddWithValue("@cant", cantidad);

                command.ExecuteNonQuery();
            }
        }
    }

    public bool EliminarPresupuesto(int id)
    {
        int filasAfectadas = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Paso 1: Borrar los detalles asociados a ese presupuesto
            string sqlDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            using (var command = new SqliteCommand(sqlDetalle, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            // Paso 2: Borrar el presupuesto (encabezado)
            string sqlPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            using (var command = new SqliteCommand(sqlPresupuesto, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                filasAfectadas = command.ExecuteNonQuery();
            }
        }
        return filasAfectadas > 0;
    }

    public bool ModificarPresupuesto(Presupuesto presupuesto)
    {
        int filasAfectadas = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql =
                @"UPDATE Presupuestos SET 
            NombreDestinatario = @nom, 
            FechaCreacion = @fec 
            WHERE IdPresupuesto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", presupuesto.IdPresupuesto);
                command.Parameters.AddWithValue("@nom", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@fec", presupuesto.FechaCreacion);

                filasAfectadas = command.ExecuteNonQuery();
            }
        }
        // Devuelve true si se afectó al menos una fila (la actualización fue exitosa)
        return filasAfectadas > 0;
    }
}

