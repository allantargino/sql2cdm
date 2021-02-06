using Sql2Cdm.Library.Models.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sql2Cdm.Library.Models
{
    [DebuggerDisplay("{Schema}.{Name}")]
    public class Table
    {
        public string Schema { get; set; }
        public string Name { get; set; }

        public IList<Column> Columns { get; set; }
        public ISet<Annotation> Annotations { get; set; }

        public Table(string name) : this(name, string.Empty) { }

        public Table(string name, string schema)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            Schema = schema;
            Name = name;
            Columns = new List<Column>();
            Annotations = new HashSet<Annotation>();
        }
    }
}
