using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Cdm;
using System.Data;
using Xunit;

namespace Sql2Cdm.Library.Tests.Cdm
{
    public class CdmSqlTypeMapperTests
    {
        [Theory]
        [InlineData(SqlDbType.BigInt, CdmDataFormat.Int64)]
        [InlineData(SqlDbType.Binary, CdmDataFormat.Binary)]
        [InlineData(SqlDbType.Bit, CdmDataFormat.Boolean)]
        [InlineData(SqlDbType.Char, CdmDataFormat.Char)]
        [InlineData(SqlDbType.Date, CdmDataFormat.Date)]
        [InlineData(SqlDbType.DateTime, CdmDataFormat.DateTime)]
        [InlineData(SqlDbType.DateTime2, CdmDataFormat.DateTime)]
        [InlineData(SqlDbType.DateTimeOffset, CdmDataFormat.DateTimeOffset)]
        [InlineData(SqlDbType.Decimal, CdmDataFormat.Decimal)]
        [InlineData(SqlDbType.Float, CdmDataFormat.Float)]
        [InlineData(SqlDbType.Image, CdmDataFormat.Binary)]
        [InlineData(SqlDbType.Int, CdmDataFormat.Int32)]
        [InlineData(SqlDbType.Money, CdmDataFormat.Decimal)]
        [InlineData(SqlDbType.NChar, CdmDataFormat.Char)]
        [InlineData(SqlDbType.NText, CdmDataFormat.String)]
        [InlineData(SqlDbType.NVarChar, CdmDataFormat.String)]
        [InlineData(SqlDbType.Real, CdmDataFormat.Decimal)]
        [InlineData(SqlDbType.SmallDateTime, CdmDataFormat.Date)]
        [InlineData(SqlDbType.SmallInt, CdmDataFormat.Int16)]
        [InlineData(SqlDbType.SmallMoney, CdmDataFormat.Decimal)]
        [InlineData(SqlDbType.Structured, CdmDataFormat.Unknown)]
        [InlineData(SqlDbType.Text, CdmDataFormat.String)]
        [InlineData(SqlDbType.Time, CdmDataFormat.Time)]
        [InlineData(SqlDbType.Timestamp, CdmDataFormat.DateTime)]
        [InlineData(SqlDbType.TinyInt, CdmDataFormat.Byte)]
        [InlineData(SqlDbType.Udt, CdmDataFormat.Unknown)]
        [InlineData(SqlDbType.UniqueIdentifier, CdmDataFormat.Guid)]
        [InlineData(SqlDbType.VarBinary, CdmDataFormat.Binary)]
        [InlineData(SqlDbType.VarChar, CdmDataFormat.String)]
        [InlineData(SqlDbType.Variant, CdmDataFormat.Unknown)]
        [InlineData(SqlDbType.Xml, CdmDataFormat.String)]
        [InlineData(null, CdmDataFormat.Unknown)]
        public void CdmTypeIsMapped(SqlDbType? type, CdmDataFormat expected)
        {
            CdmDataFormat actual = CdmSqlTypeMapper.Map(type);

            Assert.Equal(expected, actual);
        }
    }
}
