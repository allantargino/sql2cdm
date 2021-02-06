using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Sql.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Sqlite
{
    public class SqliteLengthParserTests
    {
        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("INT", 0)]
        [InlineData("INT IDENTITY(1,1)", 0)]
        [InlineData("VARCHAR(100)", 100)]
        [InlineData("NVARCHAR(100)", 100)]
        [InlineData("CHAR(2)", 2)]
        public void SqlTypeMaxSizeIsParsed(string type, int maxSize)
        {
            ColumnLength expected = new ColumnLength() { MaxSize = maxSize };

            ColumnLength actual = SqliteLengthParser.GetSqlLengthFromString(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, 0, 0, 0)]
        [InlineData("", 0, 0, 0)]
        [InlineData("DECIMAL(10,2)", 0, 10, 2)]
        public void SqlTypeMaxSizePrecisionAndDigitsAreParsed(string type, int maxSize, int precision, int digits)
        {
            ColumnLength expected = new ColumnLength() { MaxSize = maxSize, Digits = digits, Precision = precision };

            ColumnLength actual = SqliteLengthParser.GetSqlLengthFromString(type);

            Assert.Equal(expected, actual);
        }
    }
}
