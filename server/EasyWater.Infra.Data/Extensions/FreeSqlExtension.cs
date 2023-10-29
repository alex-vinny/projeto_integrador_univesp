using FreeSql;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace EasyWater.Service.Core.Extensions
{
    public static class FreeSqlExtension
    {
        private static string GetConnectionString(this FreeSqlBuilder @this)
        {
            Type type = @this.GetType();
            FieldInfo fieldInfo = type.GetField("_masterConnectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo is null)
            {
                throw new ArgumentException("_masterConnectionString is null");
            }
            return fieldInfo.GetValue(@this).ToString();
        }

        #region CreateDatabaseIfNotExists
        /// <summary>
        /// UseConnectionString
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static FreeSqlBuilder CreateDatabaseIfNotExists(this FreeSqlBuilder @this)
        {
            FieldInfo dataTypeFieldInfo = @this.GetType().GetField("_dataType", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dataTypeFieldInfo is null)
            {
                throw new ArgumentException("_dataType is null");
            }

            string connectionString = GetConnectionString(@this);
            DataType dbType = (DataType)dataTypeFieldInfo.GetValue(@this);

            switch (dbType)
            {  
                case DataType.SqlServer:
                    return @this.CreateDatabaseIfNotExistsSqlServer(connectionString);                
                default:
                    break;
            }

            return @this;
        }
        #endregion

        #region SqlServer
        public static FreeSqlBuilder CreateDatabaseIfNotExistsSqlServer(this FreeSqlBuilder @this, string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = GetConnectionString(@this);
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string createDatabaseSql;
            if (!string.IsNullOrEmpty(builder.AttachDBFilename))
            {
                string fileName = ExpandFileName(builder.AttachDBFilename);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string logFileName = Path.ChangeExtension(fileName, ".ldf");
                createDatabaseSql = @$"CREATE DATABASE {builder.InitialCatalog}   on  primary   
                (
                    name = '{name}',
                    filename = '{fileName}'
                )
                log on
                (
                    name= '{name}_log',
                    filename = '{logFileName}'
                )";
            }
            else
            {
                createDatabaseSql = @$"CREATE DATABASE {builder.InitialCatalog}";
            }

            using SqlConnection cnn = new SqlConnection($"Data Source={builder.DataSource};Integrated Security = False;User ID={builder.UserID};Password={builder.Password};Initial Catalog=master;Min pool size=1");
            cnn.Open();
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = $"select * from sysdatabases where name = '{builder.InitialCatalog}'";

            SqlDataAdapter apter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            apter.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
            {
                cmd.CommandText = createDatabaseSql;
                cmd.ExecuteNonQuery();
            }

            return @this;
        }

        private static string ExpandFileName(string fileName)
        {
            if (fileName.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
            {
                var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                if (string.IsNullOrEmpty(dataDirectory))
                {
                    dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                string name = fileName.Replace("\\", "").Replace("/", "").Substring("|DataDirectory|".Length);
                fileName = Path.Combine(dataDirectory, name);
            }
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            return Path.GetFullPath(fileName);
        }

        #endregion
    }
}
