using Sql2Cdm.Library.Sql.Sqlite;
using System.Data;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Sqlite
{
    public class SqliteSqlDbTypeParserTests
    {
        [Theory]
        [InlineData("int", SqlDbType.Int)]
        [InlineData("INT", SqlDbType.Int)]
        [InlineData("INT ", SqlDbType.Int)]
        [InlineData("INT IDENTITY(1,1)", SqlDbType.Int)]
        [InlineData("INT identity(1,1)", SqlDbType.Int)]
        [InlineData("int identity(1,1)", SqlDbType.Int)]
        [InlineData("VARCHAR", SqlDbType.VarChar)]
        [InlineData("varchar", SqlDbType.VarChar)]
        [InlineData("varCHAR", SqlDbType.VarChar)]
        [InlineData("VARCHAR(50)", SqlDbType.VarChar)]
        [InlineData("varchar(50)", SqlDbType.VarChar)]
        [InlineData("CHAR", SqlDbType.Char)]
        [InlineData("char", SqlDbType.Char)]
        [InlineData("CHAR(2)", SqlDbType.Char)]
        [InlineData("char(2)", SqlDbType.Char)]
        [InlineData("", null)]
        public void SqlTypeIsParsed(string type, SqlDbType? expected)
        {
            SqlDbType? actual = SqliteSqlDbTypeParser.GetSqlTypeFromString(type);
            
            Assert.Equal(expected, actual);
        }
    }
}
