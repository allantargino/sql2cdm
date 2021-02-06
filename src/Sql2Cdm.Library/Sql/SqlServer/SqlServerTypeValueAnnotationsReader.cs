using Microsoft.Extensions.Options;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using System.Data;
using System.Data.Common;

namespace Sql2Cdm.Library.Sql.SqlServer
{
    public class SqlServerTypeValueAnnotationsReader : ITypeValueAnnotationsReader
    {
        private readonly DbConnection connection;
        private readonly DbTransaction transaction;
        private readonly SqlRelationalModelReaderOptions options;

        public SqlServerTypeValueAnnotationsReader(DbConnection connection, IOptions<SqlRelationalModelReaderOptions> options)
            : this(connection, null, options) { }

        public SqlServerTypeValueAnnotationsReader(DbConnection connection, DbTransaction transaction, IOptions<SqlRelationalModelReaderOptions> options)
            : this(connection, transaction, options.Value) { }

        public SqlServerTypeValueAnnotationsReader(DbConnection connection, DbTransaction transaction, SqlRelationalModelReaderOptions options)
        {
            this.connection = connection;
            this.transaction = transaction;
            this.options = options;
        }

        public SqlAnnotationsCollection<SqlTypeValueAnnotation> ReadTypeValueAnnotations()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            var annotations = new SqlAnnotationsCollection<SqlTypeValueAnnotation>();

            ReadTablesAnnotations(annotations);

            ReadColumnsAnnotations(annotations);

            return annotations;
        }

        private void ReadTablesAnnotations(SqlAnnotationsCollection<SqlTypeValueAnnotation> annotations)
        {
            var getColumnsAnnotations = @"SELECT
                                           SCHEMA_NAME(tbl.schema_id) AS SchemaName,	
                                           tbl.name AS TableName, 
                                           p.name AS ExtendedPropertyName,
                                           CAST(p.value AS sql_variant) AS ExtendedPropertyValue
                                        FROM
                                           sys.tables AS tbl
                                           INNER JOIN sys.extended_properties AS p ON p.major_id=tbl.object_id
                                        WHERE
	                                        p.class=1 AND p.minor_id = 0";

            using var reader = GetDataReader(getColumnsAnnotations);

            while (reader.Read())
            {
                string tableSchema = reader["SchemaName"].ToString();
                string tableName = reader["TableName"].ToString();
                string propertyName = reader["ExtendedPropertyName"].ToString();
                string propertyValue = reader["ExtendedPropertyValue"].ToString();

                annotations.AddTableAnnotation(tableName, new() { Type = propertyName, Value = propertyValue });
            }
        }

        private void ReadColumnsAnnotations(SqlAnnotationsCollection<SqlTypeValueAnnotation> annotations)
        {
            var getColumnsAnnotations = @"SELECT
                                           SCHEMA_NAME(tbl.schema_id) AS SchemaName,	
                                           tbl.name AS TableName, 
                                           col.name AS ColumnName,
                                           p.name AS ExtendedPropertyName,
                                           CAST(p.value AS sql_variant) AS ExtendedPropertyValue
                                        FROM
                                           sys.tables AS tbl
                                           INNER JOIN sys.all_columns AS col ON col.object_id=tbl.object_id
                                           INNER JOIN sys.extended_properties AS p ON p.major_id=tbl.object_id
                                        WHERE
	                                        p.minor_id=col.column_id AND p.class=1";

            using var reader = GetDataReader(getColumnsAnnotations);

            while (reader.Read())
            {
                string tableSchema = reader["SchemaName"].ToString();
                string tableName = reader["TableName"].ToString();
                string columnName = reader["ColumnName"].ToString();
                string propertyName = reader["ExtendedPropertyName"].ToString();
                string propertyValue = reader["ExtendedPropertyValue"].ToString();

                annotations.AddColumnAnnotation(tableName, columnName, new() { Type = propertyName, Value = propertyValue });
            }
        }

        private DbDataReader GetDataReader(string sqlQuery)
        {
            var queryCommand = connection.CreateCommand();

            if (transaction != null)
            {
                queryCommand.Transaction = transaction;
            }

            queryCommand.CommandText = sqlQuery;

            return queryCommand.ExecuteReader();
        }
    }
}
