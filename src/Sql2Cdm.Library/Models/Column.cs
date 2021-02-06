using Newtonsoft.Json;
using Sql2Cdm.Library.Models.Annotations;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Sql2Cdm.Library.Models
{
    [DebuggerDisplay("{Name} ({Type})")]
    public class Column
    {
        private bool isNullable;

        public string Name { get; set; }
        [JsonIgnore]
        public Table Table { get; set; }

        public SqlDbType? Type { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable
        {
            get { return !IsPrimaryKey && isNullable; }
            set { isNullable = value; }
        }

        public bool IsForeignKey => ForeignKey != null;
        public Column ForeignKey { get; set; }

        public ColumnLength Length { get; set; }

        public ISet<Annotation> Annotations { get; set; }


        public Column(string name, Table table)
        {
            Name = name;
            Table = table;
            Annotations = new HashSet<Annotation>();
            Length = new ColumnLength();
        }
    }
}
