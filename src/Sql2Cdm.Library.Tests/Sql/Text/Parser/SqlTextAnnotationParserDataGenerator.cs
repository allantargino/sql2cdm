using System.Collections.Generic;

namespace Sql2Cdm.Library.Tests.Sql.Text.Parser
{
    public class SqlTextAnnotationParserDataGenerator
    {
        static readonly string[] types = {
               "INT",
                "INT PRIMARY KEY",
                "INT IDENTITY(1,1) PRIMARY KEY",
                "INT IDENTITY(1, 1) PRIMARY KEY",
                "INT IDENTITY( 1, 1) PRIMARY KEY",
                "INT IDENTITY( 1, 1 ) PRIMARY KEY",
                "VARCHAR(50)",
                "VARCHAR(50) NOT NULL",
                "DECIMAL(15,5)",
                "DECIMAL(15, 5)",
                "DECIMAL(15,5) NOT NULL",
            };
        static readonly string[] innerAnnotationsPatterns = {
                "extends:Customer; trait:means.entityName.specific",
                "extends: Customer; trait: means.entityName.specific",
                "extends : Customer; trait : means.entityName.specific",
                "extends :Customer; trait :means.entityName.specific",
                "extends:Customer;trait:means.entityName.specific",
                "extends:Customer ;trait:means.entityName.specific",
                "extends:Customer;trait:something(abc)",
                "extends:Customer;trait:something(123)",
                "extends:Customer;trait:something(key=123)",
                "extends:Customer;trait:something(abc,123)",
                "extends:Customer;trait:something(min=1,max=4)",
                ".",
                "something",
                "something {else}",
                "another { annotation }"
            };
        public static IEnumerable<object[]> GetValidAnnotations()
        {
            foreach (var data in GenerateAnnotationsWithExpectedValue(innerAnnotationsPatterns))
            {
                yield return data;
            }
        }
        public static IEnumerable<object[]> GetTypesAndValidAnnotations()
        {
            foreach (var data in GenerateTypesAndAnnotations(types, innerAnnotationsPatterns))
            {
                yield return data;
            }
        }
        public static IEnumerable<object[]> GetTypesAndValidAnnotationsWithExpectedValue()
        {
            foreach (var data in GenerateTypesAndAnnotationsWithExpectedValue(types, innerAnnotationsPatterns))
            {
                yield return data;
            }
        }
        public static IEnumerable<object[]> GetInvalidAnnotations()
        {
            yield return new object[] { "/**/" };
            yield return new object[] { "/* */" };
            yield return new object[] { " /* */" };
            yield return new object[] { " /* */ " };
            yield return new object[] { "/* */\n" };
            yield return new object[] { "/* */ \n" };
            yield return new object[] { "/*{}*/" };
            yield return new object[] { "/* {} */" };
            yield return new object[] { "/* { */" };
            yield return new object[] { "/* } */" };
            yield return new object[] { "/* x */" };
            yield return new object[] { "/* {x */" };
            yield return new object[] { "/* x} */" };

        }
        private static IEnumerable<object[]> GenerateAnnotationsWithExpectedValue(string[] innerAnnotationsPatterns)
        {
            foreach (var pattern in innerAnnotationsPatterns)
            {
                yield return new object[] { "/* {" + pattern + "} */", pattern };
                yield return new object[] { " /* {" + pattern + "} */", pattern };
                yield return new object[] { "/*{" + pattern + "} */ ", pattern };
                yield return new object[] { "/* {" + pattern + "}*/", pattern };
                yield return new object[] { " /* {" + pattern + "}*/", pattern };
                yield return new object[] { "/*{" + pattern + "}*/", pattern };
                yield return new object[] { "\n/*{" + pattern + "}*/\n", pattern };
                yield return new object[] { "\t/*{" + pattern + "}*/\n", pattern };
                yield return new object[] { "/*\t{" + pattern + "}\n*/", pattern };
            };
        }
        private static IEnumerable<object[]> GenerateTypesAndAnnotations(string[] types, string[] innerAnnotationsPatterns)
        {
            foreach (var type in types)
            {
                foreach (var pattern in innerAnnotationsPatterns)
                {
                    yield return new object[] { type, "/* {" + pattern + "} */" };
                    yield return new object[] { type, " /* {" + pattern + "} */" };
                    yield return new object[] { type, "/*{" + pattern + "} */ " };
                    yield return new object[] { type, "/* {" + pattern + "}*/" };
                    yield return new object[] { type, " /* {" + pattern + "}*/" };
                    yield return new object[] { type, "/*{" + pattern + "}*/" };
                    yield return new object[] { type, "\n/*{" + pattern + "}*/\n" };
                    yield return new object[] { type, "\t/*{" + pattern + "}*/\n" };
                    yield return new object[] { type, "/*\t{" + pattern + "}\n*/" };
                };
            };
        }
        private static IEnumerable<object[]> GenerateTypesAndAnnotationsWithExpectedValue(string[] types, string[] innerAnnotationsPatterns)
        {
            foreach (var type in types)
            {
                foreach (var pattern in innerAnnotationsPatterns)
                {
                    yield return new object[] { type, "/* {" + pattern + "} */", pattern };
                    yield return new object[] { type, " /* {" + pattern + "} */", pattern };
                    yield return new object[] { type, "/*{" + pattern + "} */ ", pattern };
                    yield return new object[] { type, "/* {" + pattern + "}*/", pattern };
                    yield return new object[] { type, " /* {" + pattern + "}*/", pattern };
                    yield return new object[] { type, "/*{" + pattern + "}*/", pattern };
                    yield return new object[] { type, "\n/*{" + pattern + "}*/\n", pattern };
                    yield return new object[] { type, "\t/*{" + pattern + "}*/\n", pattern };
                    yield return new object[] { type, "/*\t{" + pattern + "}\n*/", pattern };
                };
            };
        }
    }
}
