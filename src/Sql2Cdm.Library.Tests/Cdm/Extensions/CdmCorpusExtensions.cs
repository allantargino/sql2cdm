using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql2Cdm.Library.Tests.Cdm.Extensions
{
    public static class CdmCorpusExtensions
    {
        public static CdmDataTypeReference CreateDataType(this CdmCorpusDefinition cdmCorpus, CdmDataFormat dataFormat)
        {
            return cdmCorpus.MakeObject<CdmDataTypeReference>(CdmObjectType.DataTypeRef, dataFormat.ToString(), true);
        }
    }
}
