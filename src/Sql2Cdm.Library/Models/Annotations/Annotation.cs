using Sql2Cdm.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sql2Cdm.Library.Models.Annotations
{
    public abstract class Annotation : IEquatable<Annotation>
    {
        private readonly ISet<KeyValuePair<string, dynamic>> arguments;

        public string Value { get; }
        public IEnumerable<KeyValuePair<string, dynamic>> Arguments => arguments;

        public Annotation(string value) : this(value, Enumerable.Empty<KeyValuePair<string, dynamic>>()) { }

        public Annotation(string value, IEnumerable<KeyValuePair<string, dynamic>> arguments)
        {
            this.Value = value;
            this.arguments = new HashSet<KeyValuePair<string, dynamic>>();

            foreach (var argument in arguments)
            {
                AddArgument(argument.Key, argument.Value);
            }
        }

        public void AddArgument(string key, dynamic value)
        {
            if (value == null || value is string && string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var argument = new KeyValuePair<string, dynamic>(key, value);
            arguments.Add(argument);
        }

        public abstract void Process(IAnnotationProcessorVisitor processor);

        public override bool Equals(object obj)
        {
            return Equals(obj as Annotation);
        }

        public bool Equals(Annotation other)
        {
            return other != null &&
                   Value == other.Value &&
                   arguments.SetEquals(other.arguments);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(this.Value);
            hash.Add(this.arguments.Count > 0 ? this.arguments : null);
            return hash.ToHashCode();
        }
    }
}
