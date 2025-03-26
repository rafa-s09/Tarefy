using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Tarefy.Enumeradores;
using Tarefy.Models;

namespace Tarefy.Repositorio
{
    public class TarefaRepository
    {
        private readonly string _connectionString;

        public TarefaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertTarefa(TarefaModel tarefa)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Descricao", tarefa.Descricao);
                    command.Parameters.AddWithValue("@Detalhes", (object)tarefa.Detalhes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DataLimite", (object)tarefa.DataLimite ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Prioridade", (int)tarefa.Prioridade);
                    command.Parameters.AddWithValue("@Status", (int)tarefa.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTarefa(TarefaModel tarefa)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", tarefa.Id);
                    command.Parameters.AddWithValue("@Descricao", tarefa.Descricao);
                    command.Parameters.AddWithValue("@Detalhes", (object)tarefa.Detalhes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DataLimite", (object)tarefa.DataLimite ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Prioridade", (int)tarefa.Prioridade);
                    command.Parameters.AddWithValue("@Status", (int)tarefa.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTarefa(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TarefaModel> GetTarefas()
        {
            List<TarefaModel> tarefas = new List<TarefaModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tarefas.Add(new TarefaModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Detalhes = reader.IsDBNull(reader.GetOrdinal("Detalhes")) ? null : reader.GetString(reader.GetOrdinal("Detalhes")),
                                DataCriacao = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                                DataLimite = reader.IsDBNull(reader.GetOrdinal("DataLimite")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataLimite")),
                                Prioridade = (PrioridadeEnum)reader.GetInt32(reader.GetOrdinal("Prioridade")),
                                Status = (StatusEnum)reader.GetInt32(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }

            return tarefas;
        }

        public TarefaModel GetTarefaById(int id)
        {
            TarefaModel tarefa = new TarefaModel();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_select_by_id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tarefa = new TarefaModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Detalhes = reader.IsDBNull(reader.GetOrdinal("Detalhes")) ? null : reader.GetString(reader.GetOrdinal("Detalhes")),
                                DataCriacao = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                                DataLimite = reader.IsDBNull(reader.GetOrdinal("DataLimite")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataLimite")),
                                Prioridade = (PrioridadeEnum)reader.GetInt32(reader.GetOrdinal("Prioridade")),
                                Status = (StatusEnum)reader.GetInt32(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }

            return tarefa;
        }

    }
}