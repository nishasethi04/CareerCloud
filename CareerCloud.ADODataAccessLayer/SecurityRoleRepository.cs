﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;

namespace CareerCloud.ADODataAccessLayer
{
    public class SecurityRoleRepository : IDataRepository<SecurityRolePoco>
    {
        private readonly string _connStr;
        public SecurityRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;

        }
        public void Add(params SecurityRolePoco[] items)
        {
            foreach (SecurityRolePoco poco in items)
            {
                using (SqlConnection connection = new SqlConnection(_connStr))
                {

                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Roles]
           ([Id]
           ,[Role]
           ,[Is_Inactive])
     VALUES
           (@Id
           ,@Role
           ,@Is_Inactive) ";

                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Role", poco.Role);
                    comm.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    connection.Open();
                    int rowEffected = comm.ExecuteNonQuery();
                    connection.Close();



                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityRolePoco> GetAll(params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT [Id]
                      ,[Role]
                      ,[Is_Inactive]
                  FROM [dbo].[Security_Roles]";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SecurityRolePoco [] pocos = new SecurityRolePoco[500];
                int index = 0;
                while (reader.Read())
                {
                    SecurityRolePoco poco = new SecurityRolePoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Role = reader.GetString(1);
                    poco.IsInactive = reader.GetBoolean(2);
                    
                    pocos[index] = poco;
                    index++;


                }

                conn.Close();
                return pocos.Where(a => a != null).ToList();

            }
        }

        public IList<SecurityRolePoco> GetList(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityRolePoco GetSingle(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {

            IQueryable<SecurityRolePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }
    

        public void Remove(params SecurityRolePoco[] items)
        {
        using (SqlConnection conn = new SqlConnection(_connStr))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            foreach (SecurityRolePoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Security_Roles]
                                 where Id=@Id";
                cmd.Parameters.AddWithValue("@Id", poco.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }
    }

        public void Update(params SecurityRolePoco[] items)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                foreach (var poco in items)
                {
                    cmd.CommandText = @"UPDATE [dbo].[Security_Roles]
                       SET 
                          [Role] =@Role
                          ,[Is_Inactive] = @Is_Inactive
                          WHERE[Id]=@Id";

                    cmd.Parameters.AddWithValue("@Id", poco.Id);
                    cmd.Parameters.AddWithValue("@Role", poco.Role);
                    cmd.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);

                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }
    }
}
