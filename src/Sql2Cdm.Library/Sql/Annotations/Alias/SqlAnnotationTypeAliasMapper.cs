using Microsoft.Extensions.Options;
using Sql2Cdm.Library.Sql.Annotations.DataStructures;
using System;
using System.Collections.Generic;

namespace Sql2Cdm.Library.Sql.Annotations.Alias
{
    public class SqlAnnotationTypeAliasMapper : SqlAnnotationMapper<SqlTypeValueAnnotation, SqlTypeValueAnnotation>
    {
        private readonly SqlAnnotationTypeAliasOptions options;
        private readonly Dictionary<string, string> aliasMapping;

        public SqlAnnotationTypeAliasMapper(IOptions<SqlAnnotationTypeAliasOptions> options) : this(options.Value) { }

        public SqlAnnotationTypeAliasMapper(SqlAnnotationTypeAliasOptions options)
        {
            this.options = options;
            this.aliasMapping = CreateAliasMappingDictionary(options.Alias);
        }

        private Dictionary<string, string> CreateAliasMappingDictionary(IEnumerable<string> mappings)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (string item in mappings)
            {
                string[] split = item.Split(':', count: 2, StringSplitOptions.TrimEntries);
                if (split.Length == 2)
                {
                    string from = split[0];
                    string to = split[1];

                    if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                    {
                        dictionary.Add(from, to);
                    }
                }
            }

            return dictionary;
        }

        protected override SqlTypeValueAnnotation Map(SqlTypeValueAnnotation annotation)
        {
            var mappedAliasType = MapAliasType(annotation.Type);
            return annotation with { Type = mappedAliasType };
        }

        private string MapAliasType(string type)
        {
            if (aliasMapping.ContainsKey(type))
                return aliasMapping[type];
            return type;
        }
    }
}