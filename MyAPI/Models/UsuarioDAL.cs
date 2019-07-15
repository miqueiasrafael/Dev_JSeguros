using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyAPI.Models
{
    public class UsuarioDAL : IUsuario
    {
        readonly string _connectionString;

        public UsuarioDAL(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        //Método que obtem todos os usuários
        public IEnumerable<Usuario> ObterUsuarios()
        {
            var userList = new List<Usuario>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("ObterUsuarios", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NomeUsuario = reader["NomeUsuario"].ToString(),
                        Idade = Convert.ToInt32(reader["Idade"]),
                        Tipo = Convert.ToInt32(reader["Tipo"])
                    };

                    userList.Add(employee);
                }
                con.Close();
            }
            return userList;
        }

        //Inclusao de novo usuário 
        public int IncluirUsuario(Usuario user)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand("IncluirUsuario", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@NomeUsuario", user.NomeUsuario);
                    cmd.Parameters.AddWithValue("@Idade", user.Idade);
                    cmd.Parameters.AddWithValue("@Tipo", user.Tipo);              

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        //Atualizar um Usuário
        public int AtualizarUsuario(Usuario user)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("AtualizarSelecao", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Selecao", user.NomeUsuario);
                cmd.Parameters.AddWithValue("@Idade", user.Idade);
                cmd.Parameters.AddWithValue("@Tipo", user.Tipo);
            
                con.Open();
                return cmd.ExecuteNonQuery();
            }

        }

        //Obter Usuario por id
        public Usuario ObterUsuarioPorId(int id)
        {
            var user = new Usuario();

            using (var con = new SqlConnection(_connectionString))
            {
                var query =
                    $"SELECT Id, NomeUsuario, Idade, Tipo FROM CrudAspNetCore WHERE Id = {id}";
                var cmd = new SqlCommand(query, con);

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.Id = Convert.ToInt32(reader["Id"]);
                        user.NomeUsuario = reader["NomeUsuario"].ToString();
                        user.Idade = Convert.ToInt32(reader["Idade"]);
                        user.Tipo = Convert.ToInt32(reader["Tipo"]);
                       
                    }
                }
                else
                    return null;
            }
            return user;
        }

        //Excluir usuário por id
        public int ExcluirUsuario(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("ExcluirUsuario", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                return cmd.ExecuteNonQuery();
            }

        }


    }
}
