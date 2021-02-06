using Microsoft.CommonDataModel.ObjectModel.Cdm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sql2Cdm.Library.Tests.Cdm.Extensions
{
    public static class CdmEntityExtensions
    {
        public static CdmTypeAttributeDefinition GetAttribute(this CdmEntityDefinition entity, string name)
        {
            return (CdmTypeAttributeDefinition)entity.Attributes.FirstOrDefault(a => ((CdmTypeAttributeDefinition)a).Name == name);
        }
    }
}
