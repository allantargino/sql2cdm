using System;
using System.Collections.Generic;
using System.Linq;

namespace Sql2Cdm.Library.Sql.Annotations.Loader
{
    public class SqlAnnotationAssemblyLoader<BaseAnnotationT, DefaultAnnotationT>
    {
        private const string formatPattern = "{0}Annotation";
        private readonly IEnumerable<Type> annotationTypes;

        public SqlAnnotationAssemblyLoader()
        {
            Type baseAnnotation = typeof(BaseAnnotationT);
            Type defaultAnnotation = typeof(DefaultAnnotationT);

            this.annotationTypes = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => baseAnnotation.IsAssignableFrom(p) && p != baseAnnotation && p != defaultAnnotation);
        }

        public BaseAnnotationT LoadAnnotation(string annotationName, string annotationValueText)
        {
            var patternFormatted = string.Format(formatPattern, annotationName);
            var type = annotationTypes.FirstOrDefault(t => t.Name.Equals(patternFormatted, StringComparison.OrdinalIgnoreCase));

            var valueParser = new SqlAnnotationValueParser(annotationValueText);
            string value = valueParser.ParseValue();
            IEnumerable<KeyValuePair<string, dynamic>> arguments = valueParser.ParseValueArguments();

            if (type != null)
            {
                if (arguments.Any())
                {
                    return (BaseAnnotationT)Activator.CreateInstance(type, value, arguments);
                }
                else
                {
                    return (BaseAnnotationT)Activator.CreateInstance(type, value);
                }
            }
            else
            {
                return (BaseAnnotationT)Activator.CreateInstance(typeof(DefaultAnnotationT), annotationName);
            }
        }
    }
}
