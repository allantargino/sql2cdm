using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Sql.SqlServer
{
    public class SqlServerRelationalModelReader : IRelationalModelReader
    {
        private readonly DbConnection connection;
        private readonly DbTransaction transaction;
        private readonly SqlRelationalModelReaderOptions options;

        public SqlServerRelationalModelReader(DbConnection connection, IOptions<SqlRelationalModelReaderOptions> options)
            : this(connection, null, options) { }

        public SqlServerRelationalModelReader(DbConnection connection, DbTransaction transaction, IOptions<SqlRelationalModelReaderOptions> options)
            : this(connection, transaction, options.Value) { }

        public SqlServerRelationalModelReader(DbConnection connection, DbTransaction transaction, SqlRelationalModelReaderOptions options)
        {
            this.connection = connection;
            this.transaction = transaction;
            this.options = options;
        }

        public RelationalModel ReadRelationalModel()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            List<Table> tables = new List<Table>();

            AddColumns(tables);

            AddPrimaryKeys(tables);

            AddForeignKeys(tables);

            FilterSchemas(tables);

            return new RelationalModel()
            {
                Tables = tables
            };
        }

        private void AddColumns(List<Table> tables)
        {
            var getColumnsCommand = @"SELECT
                                        TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                                    FROM
                                        INFORMATION_SCHEMA.COLUMNS
                                    WHERE
                                        TABLE_NAME NOT IN (SELECT name FROM sys.views)";

            using var reader = GetDataReader(getColumnsCommand);

            while (reader.Read())
            {
                string tableSchema = reader["TABLE_SCHEMA"].ToString();
                string tableName = reader["TABLE_NAME"].ToString();
                string columnName = reader["COLUMN_NAME"].ToString();
                string isNullable = reader["IS_NULLABLE"].ToString();
                string dataType = reader["DATA_TYPE"].ToString();
                string characterMaximumLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString();

                Table table = tables.FirstOrDefault(t => t.Name == tableName);

                if (table == null)
                {
                    table = new Table(tableName) { Schema = tableSchema };
                    tables.Add(table);
                }

                Column column = new Column(columnName, table);

                column.IsNullable = isNullable.Equals("YES", StringComparison.OrdinalIgnoreCase);
                column.Type = SqlServerSqlDbTypeParser.GetSqlTypeFromString(dataType);

                if (!string.IsNullOrWhiteSpace(characterMaximumLength))
                {
                    column.Length = new ColumnLength()
                    {
                        MaxSize = int.Parse(characterMaximumLength)
                    };
                }

                table.Columns.Add(column);
            }
        }

        private void AddPrimaryKeys(List<Table> tables)
        {
            var getPkCommand = @"SELECT
	                                C.TABLE_NAME, CU.COLUMN_NAME
                                FROM
	                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS C
                                INNER JOIN
	                                INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CU
                                ON
	                                C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                                WHERE
	                                C.CONSTRAINT_TYPE = 'PRIMARY KEY'";

            using var reader = GetDataReader(getPkCommand);

            while (reader.Read())
            {
                string tableName = reader["TABLE_NAME"].ToString();
                string columnName = reader["COLUMN_NAME"].ToString();

                Table table = tables.First(t => t.Name == tableName);
                Column column = table.Columns.First(c => c.Name == columnName);

                column.IsPrimaryKey = true;
            }
        }

        private void AddForeignKeys(List<Table> tables)
        {
            foreach (Table toTable in tables)
            {
                var getFkCommand = $@"EXEC sp_fkeys @pktable_name = '{toTable.Name}', @pktable_owner = '{toTable.Schema}';";

                using var reader = GetDataReader(getFkCommand);
                
                while (reader.Read())
                {
                    string toColumnName = reader["PKCOLUMN_NAME"].ToString();
                    string fromTableName = reader["FKTABLE_NAME"].ToString();
                    string fromColumnName = reader["FKCOLUMN_NAME"].ToString();

                    var fromTable = tables.First(t => t.Name == fromTableName);
                    var fromColumn = fromTable.Columns.First(c => c.Name == fromColumnName);
                    var toColumn = toTable.Columns.First(c => c.Name == toColumnName);

                    fromColumn.ForeignKey = toColumn;
                }
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

        private void FilterSchemas(List<Table> tables)
        {
            if (string.IsNullOrWhiteSpace(options.SchemaFilterRegexPattern))
                return;

            var schemaFilter = new Regex(options.SchemaFilterRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            tables.RemoveAll(t => !schemaFilter.IsMatch(t.Schema));
        }
    }
}
