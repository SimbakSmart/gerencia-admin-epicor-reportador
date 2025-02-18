﻿
using Core.Models;
using Infraesctructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Infraesctructure.Data;

namespace Infraesctructure.Services
{
    public class CallsInQueuesProvider : ICallsInQueuesProvider
    {
        private string connectionString = DBContext.GetConnectionString;

        private string query = string.Empty;

        public (bool, string) TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Conexión establecida correctamente.");
                    return (true, "Conexión establecida correctamente.");
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("Error al conectar con la base de datos: " + ex.Message);
                return (false, "Error al conectar con la base de datos: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error general: " + ex.Message);
                return (false, "Error general: " + ex.Message);
            }
        }


        public async Task<List<CallsInQueues>> FetchAllAsync(string queryParams = "")
        {
            List<CallsInQueues> list = new List<CallsInQueues>();
            try
            {
       
                query = CallIsInQueuesSQL.Query(queryParams);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand com = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = await com.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new CallsInQueues.CallsInQueuesBuilder()
                                    .WithSupportCallID(reader["SupportCallID"].ToString())
                                    .WithNumber(Convert.ToInt32(reader["Number"]))
                                    .WithTypes(reader["Types"].ToString())
                                    .WithSummary(reader["Summary"].ToString())
                                    .WithQueue(reader["Queue"].ToString())
                                    .WithStatus(reader["Status"].ToString())
                                    .WithOpenDate(Convert.ToDateTime(reader["OpenDate"]))
                                    .WithDueDate(Convert.ToDateTime(reader["DueDate"]))
                                    .WithStartDate(Convert.ToDateTime(reader["StartDate"]))
                                    .WithDateAssignTo(Convert.ToDateTime(reader["DateAssignedTo"]))
                                    .WithPriority(reader["Priority"].ToString())
                                    .WithAttribute(reader["Attribute"].ToString())
                                    .WithValue(reader["Value"].ToString())
                                    .WithEventSummary(reader["EventSummary"].ToString())
                                    .WithDetail(reader["Detail"].ToString())
                                    .WithProduct(reader["Product"].ToString())
                                    .Build());
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        public async Task<List<Queue>> FetchQueuesAsync()
        {
            List<Queue> list = new List<Queue>();
            try
            {
                query = CallIsInQueuesSQL.QueuesQuey();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand com = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = await com.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new Queue.QueueBuilder()
                                        .WithQueueID(reader["QueueID"].ToString())
                                        .WithName(reader["Name"].ToString())
                                        .Build());
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        
    }
}
