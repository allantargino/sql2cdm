using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Sql.Annotations.Loader
{
    public class SqlAnnotationValueParser
    {
        private const char ArgumentsSeparator = ',';
        private const char KeyValueSeparator = '=';

        private readonly string annotationValueText;

        public SqlAnnotationValueParser(string annotationValueText)
        {
            this.annotationValueText = annotationValueText;
        }

        public string ParseValue()
        {
            var split = annotationValueText.Split('(', count: 2);
            return split[0];
        }

        public IEnumerable<KeyValuePair<string, dynamic>> ParseValueArguments()
        {
            var matches = Regex.Match(annotationValueText, @"\((.*)\)");

            if (matches.Length < 2)
            {
                yield break;
            }

            var arguments = matches.Groups[1].Value;

            if (string.IsNullOrWhiteSpace(arguments))
            {
                yield break;
            }

            var splitArguments = arguments
                                    .Split(ArgumentsSeparator, StringSplitOptions.TrimEntries)
                                    .Where(a => !string.IsNullOrWhiteSpace(a));

            foreach (var argument in splitArguments)
            {
                var keyValueSplit = argument.Split(KeyValueSeparator, count: 2, StringSplitOptions.TrimEntries);

                if (keyValueSplit.Length == 1)
                {
                    yield return new KeyValuePair<string, dynamic>(string.Empty, keyValueSplit[0]);
                }
                else
                {
                    yield return new KeyValuePair<string, dynamic>(keyValueSplit[0], keyValueSplit[1]);
                }
            }
        }
    }
}
