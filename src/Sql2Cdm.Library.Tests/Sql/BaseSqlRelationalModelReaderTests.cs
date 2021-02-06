using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Tests.Sql.Sqlite.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql
{
    public abstract class BaseSqlRelationalModelReaderTests : IDisposable
    {
        protected DbConnection Connection { get; }
        protected DbTransaction Transaction { get; set; }

        public BaseSqlRelationalModelReaderTests(DbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Builder method to return the IRelationalModelReader SUT
        /// </summary>
        protected abstract IRelationalModelReader CreateRelationalModelReader();

        /// <summary>
        /// Setup Method
        /// </summary>
        /// <param name="sqlCommandText">SQL command to be executed inside Transaction context</param>
        protected void ExecuteSqlCommand(string sqlCommandText)
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            Transaction = Connection.BeginTransaction();
            
            var command = Connection.CreateCommand();
            command.CommandText = sqlCommandText;
            command.Transaction = Transaction;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// TearDown method to rollback all transaction changes
        /// </summary>
        public void Dispose()
        {
            Transaction.Rollback();
            Connection.Dispose();
        }

        [Fact]
        public void ReadsNonNullRelationalModel()
        {
            var sql = @"CREATE TABLE Customer (
				            CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();

            Assert.NotNull(model);
        }

        [Fact]
        public void ReadsNonNullTables()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            IEnumerable<Table> tables = model.Tables;

            Assert.NotNull(tables);
        }

        [Fact]
        public void ReadsSingleTable()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            IEnumerable<Table> tables = model.Tables;

            Assert.Single(tables);
        }

        [Fact]
        public void ReadsNonNullTable()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Table table = model.GetTable();

            Assert.NotNull(table);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("a1", "a1")]
        [InlineData("Customer", "Customer")]
        [InlineData("CUSTOMER", "CUSTOMER")]
        [InlineData("\"Special Customer\"", "Special Customer")]
        public void ReadsTableName(string name, string expected)
        {
            var sql = $@"CREATE TABLE {name} (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Table table = model.GetTable();

            Assert.Equal(expected, table.Name);
        }

        [Fact]
        public void ReadsNonNullColumns()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            IEnumerable<Column> columns = model.GetTable().Columns;

            Assert.NotNull(columns);
        }

        [Fact]
        public void ReadsSingleColumn()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            IEnumerable<Column> columns = model.GetTable().Columns;

            Assert.Single(columns);
        }

        [Theory]
        [InlineData("CUSTOMER_ID", "CUSTOMER_ID")]
        [InlineData("AGE", "AGE")]
        [InlineData("age", "age")]
        [InlineData("\"CUSTOMER ABC\"", "CUSTOMER ABC")]
        public void ReadsColumnName(string name, string expected)
        {
            var sql = $@"CREATE TABLE Customer (
							{name} INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            Column column = reader.ReadRelationalModel().GetColumn();

            Assert.Equal(expected, column.Name);
        }

        // TODO: add more cases
        [Theory]
        [InlineData("INT", SqlDbType.Int)]
        [InlineData("VARCHAR(50)", SqlDbType.VarChar)]
        public void ReadsColumnType(string type, SqlDbType expected)
        {
            var sql = $@"CREATE TABLE Customer (
							CUSTOMER_ID {type}
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            Column column = reader.ReadRelationalModel().GetColumn();

            Assert.Equal(expected, column.Type);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("NULL", true)]
        [InlineData("NOT NULL", false)]
        [InlineData("PRIMARY KEY", false)]
        public void ReadsColumnIsNullable(string statement, bool expected)
        {
            var sql = $@"CREATE TABLE Customer (
							CUSTOMER_ID INT {statement}
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            Column column = reader.ReadRelationalModel().GetColumn();

            Assert.Equal(expected, column.IsNullable);
        }

        // TODO: add more cases
        [Fact]
        public void ReadsColumnLength()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID VARCHAR(10)
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Column column = model.GetColumn();

            Assert.NotNull(column.Length);
            Assert.Equal(10, column.Length.MaxSize);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("PRIMARY KEY", true)]
        public void ReadsColumnIsPrimaryKey(string statement, bool expected)
        {
            var sql = $@"CREATE TABLE Customer (
							CUSTOMER_ID INT {statement}
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            Column column = reader.ReadRelationalModel().GetColumn();

            Assert.Equal(expected, column.IsPrimaryKey);
        }

        [Fact]
        public void ReadsColumnIsForeignKey()
        {
            var sql = @"CREATE TABLE Customer (
                            CUSTOMER_ID INT PRIMARY KEY
                        );
                        CREATE TABLE CustomerAddresses (
                            CUSTOMER_ADDRESS_ID INT PRIMARY KEY,
                            CUSTOMER_ID INT,
                            FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            Column column = reader.ReadRelationalModel().GetColumn("CustomerAddresses", "CUSTOMER_ID");

            Assert.True(column.IsForeignKey);
        }

        [Fact]
        public void ReadsColumnTable()
        {
            var sql = @"CREATE TABLE Customer (
							CUSTOMER_ID INT
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Table table = model.GetTable();
            Column column = model.GetColumn();

            Assert.NotNull(column.Table);
            Assert.Same(table, column.Table);
        }

        [Fact]
        public void ReadsColumnForeignKey()
        {
            var sql = @"CREATE TABLE Customer (
                            CUSTOMER_ID INT PRIMARY KEY
                        );
                        CREATE TABLE CustomerAddresses (
                            CUSTOMER_ADDRESS_ID INT PRIMARY KEY,
                            CUSTOMER_ID INT,
                            FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Column fromColumn = model.GetColumn("CustomerAddresses", "CUSTOMER_ID");
            Column toColumn = model.GetColumn("Customer", "CUSTOMER_ID");

            Assert.Same(toColumn, fromColumn.ForeignKey);
        }

        [Fact]
        public void ReadsColumnForeignKeyTable()
        {
            var sql = @"CREATE TABLE Customer (
                            CUSTOMER_ID INT PRIMARY KEY
                        );
                        CREATE TABLE CustomerAddresses (
                            CUSTOMER_ADDRESS_ID INT PRIMARY KEY,
                            CUSTOMER_ID INT,
                            FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
                        );";
            ExecuteSqlCommand(sql);
            IRelationalModelReader reader = CreateRelationalModelReader();

            RelationalModel model = reader.ReadRelationalModel();
            Column fromColumn = model.GetColumn("CustomerAddresses", "CUSTOMER_ID");
            Table toTable = model.GetTable("Customer");

            Assert.Same(toTable, fromColumn.ForeignKey.Table);
        }
    }
}
