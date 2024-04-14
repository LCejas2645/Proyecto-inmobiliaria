using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ABM_inmobiliaria.Models
{
    public class RepositorioUsuario
    {
        readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";
        public RepositorioUsuario()
        {
        }

        public IList<Usuario> GetUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM usuario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                Id = reader.GetInt32(nameof(Usuario.Id)),
                                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                                Email = reader.GetString(nameof(Usuario.Email)),
                                Password = reader.GetString(nameof(Usuario.Password)),
                                AvatarUrl = reader[nameof(Usuario.AvatarUrl)] as string ?? string.Empty,
                                Rol = (Roles)reader.GetInt32(nameof(Usuario.Rol)) // Conversión de entero a Roles
                            });

                        }
                    }
                }
                connection.Close();
            }

            return usuarios;
        }


        public Usuario? GetUsuario(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = $"SELECT * FROM Usuario WHERE {nameof(Usuario.Id)} = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32(nameof(Usuario.Id)),
                                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                                Email = reader.GetString(nameof(Usuario.Email)),
                                Password = reader.GetString(nameof(Usuario.Password)),
                                AvatarUrl = reader.GetString(nameof(Usuario.AvatarUrl)),
                                Rol = (Roles)reader.GetInt32(nameof(Usuario.Rol))
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return null;
        }

        public Usuario? GetUsuarioEmail(string email)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = $"SELECT * FROM Usuario WHERE {nameof(Usuario.Email)} = @Email";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32(nameof(Usuario.Id)),
                                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                                Email = reader.GetString(nameof(Usuario.Email)),
                                Password = reader.GetString(nameof(Usuario.Password)),
                                AvatarUrl = reader.GetString(nameof(Usuario.AvatarUrl)),
                                Rol = (Roles)reader.GetInt32(nameof(Usuario.Rol)) // Conversión de entero a Roles
                            };
                        }
                    }
                    connection.Close();
                }
            }
            return null;
        }


        public void InsertarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO usuario(Nombre, Apellido, Email, Password, Rol)
                    VALUES (@Nombre, @Apellido, @Email, @Password, @Rol)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Password", usuario.Password);
                    command.Parameters.AddWithValue("@Rol", (int)usuario.Rol); // Conversión de Roles a entero

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"UPDATE usuario SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Rol = @Rol WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Rol", (int)usuario.Rol);
                    command.Parameters.AddWithValue("@Id", usuario.Id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarPassword(Usuario usuario)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"UPDATE usuario SET Password = @Password WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@password", usuario.Password);
                    command.Parameters.AddWithValue("@id", usuario.Id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarAvatar(int id, string nuevaUrl)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = $"UPDATE usuario SET AvatarUrl = @AvatarUrl WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@AvatarUrl", nuevaUrl);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarUsuario(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @$"DELETE FROM usuario WHERE {nameof(Usuario.Id)} = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

    }
}