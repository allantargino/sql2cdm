using Microsoft.CommonDataModel.ObjectModel.Enums;
using System.Data;

namespace Sql2Cdm.Library.Cdm
{
    public static class CdmSqlTypeMapper
    {
        public static CdmDataFormat Map(SqlDbType? type)
        {
            return type switch
            {
                SqlDbType.BigInt => CdmDataFormat.Int64,
                SqlDbType.Binary => CdmDataFormat.Binary,
                SqlDbType.Bit => CdmDataFormat.Boolean,
                SqlDbType.Char => CdmDataFormat.Char,
                SqlDbType.Date => CdmDataFormat.Date,
                SqlDbType.DateTime => CdmDataFormat.DateTime,
                SqlDbType.DateTime2 => CdmDataFormat.DateTime,
                SqlDbType.DateTimeOffset => CdmDataFormat.DateTimeOffset,
                SqlDbType.Decimal => CdmDataFormat.Decimal,
                SqlDbType.Float => CdmDataFormat.Float,
                SqlDbType.Image => CdmDataFormat.Binary,
                SqlDbType.Int => CdmDataFormat.Int32,
                SqlDbType.Money => CdmDataFormat.Decimal,
                SqlDbType.NChar => CdmDataFormat.Char,
                SqlDbType.NText => CdmDataFormat.String,
                SqlDbType.NVarChar => CdmDataFormat.String,
                SqlDbType.Real => CdmDataFormat.Decimal,
                SqlDbType.SmallDateTime => CdmDataFormat.Date,
                SqlDbType.SmallInt => CdmDataFormat.Int16,
                SqlDbType.SmallMoney => CdmDataFormat.Decimal,
                SqlDbType.Structured => CdmDataFormat.Unknown,
                SqlDbType.Text => CdmDataFormat.String,
                SqlDbType.Time => CdmDataFormat.Time,
                SqlDbType.Timestamp => CdmDataFormat.DateTime,
                SqlDbType.TinyInt => CdmDataFormat.Byte,
                SqlDbType.Udt => CdmDataFormat.Unknown,
                SqlDbType.UniqueIdentifier => CdmDataFormat.Guid,
                SqlDbType.VarBinary => CdmDataFormat.Binary,
                SqlDbType.VarChar => CdmDataFormat.String,
                SqlDbType.Variant => CdmDataFormat.Unknown,
                SqlDbType.Xml => CdmDataFormat.String,
                _ => CdmDataFormat.Unknown,
            };
        }
    }
}
