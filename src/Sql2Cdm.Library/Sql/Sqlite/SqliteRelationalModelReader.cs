using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Sql2Cdm.Library.Sql.Sqlite
{
    public class SqliteRelationalModelReader : IRelationalModelReader
    {
        private readonly DbConnection dbConnection;

        public SqliteRelationalModelReader(DbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public RelationalModel ReadRelationalModel()
        {
            IEnumerable<Table> tables = ReadTables().ToList();
            SetRelationships(tables);

            return new RelationalModel()
            {
                Tables = tables
            };
        }

        private IEnumerable<Table> ReadTables()
        {
            var tableNames = ReadTableNames();

            foreach (var tableName in tableNames)
            {
                var table = new Table(tableName);
                table.Columns = ReadColumns(table).ToList();

                yield return table;
            }
        }

        private IEnumerable<string> ReadTableNames()
        {
            var getTablesQuery = @"SELECT name
                                   FROM sqlite_master 
                                   WHERE type ='table' AND name NOT LIKE 'sqlite_%';";

            using var reader = GetDataReader(getTablesQuery);

            while (reader.Read())
            {
                yield return reader["name"].ToString();
            }
        }

        private IEnumerable<Column> ReadColumns(Table table)
        {
            var getColumnsQuery = $"PRAGMA table_info('{table.Name}')";

            using var reader = GetDataReader(getColumnsQuery);

            while (reader.Read())
            {
                var name = reader["name"].ToString();
                var pk = reader["pk"].ToString();
                var notnull = reader["notnull"].ToString();
                var type = reader["type"].ToString();

                yield return new Column(name, table)
                {
                    IsPrimaryKey = pk == "1",
                    IsNullable = notnull != "1",
                    Type = SqliteSqlDbTypeParser.GetSqlTypeFromString(type),
                    Length = SqliteLengthParser.GetSqlLengthFromString(type)
                };
            }
        }

        private void SetRelationships(IEnumerable<Table> tables)
        {
            foreach (var fromTable in tables)
            {
                var getColumnsQuery = $"PRAGMA foreign_key_list('{fromTable.Name}')";

                using var reader = GetDataReader(getColumnsQuery);

                while (reader.Read())
                {
                    var fk = new
                    {
                        toTableName = reader["table"].ToString(),
                        toColumnName = reader["to"].ToString(),
                        fromColumnName = reader["from"].ToString()
                    };

                    var toTable = tables.First(t => t.Name == fk.toTableName);
                    var toColumn = toTable.Columns.First(c => c.Name == fk.toColumnName);
                    var fromColumn = fromTable.Columns.First(c => c.Name == fk.fromColumnName);

                    fromColumn.ForeignKey = toColumn;
                }
            }
        }

        private DbDataReader GetDataReader(string sqlQuery)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            var queryCommand = dbConnection.CreateCommand();
            queryCommand.CommandText = sqlQuery;
            return queryCommand.ExecuteReader();
        }
    }
}
