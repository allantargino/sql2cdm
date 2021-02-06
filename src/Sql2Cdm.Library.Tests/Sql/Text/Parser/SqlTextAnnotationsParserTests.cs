using Sql2Cdm.Library.Sql.Annotations.Parser;
using System.Linq;
using Xunit;

namespace Sql2Cdm.Library.Tests.Sql.Text.Parser
{
    public class SqlTextAnnotationsParserTests
    {
        [Fact]
        public void ParsesNotNullAnnotations()
        {
            var sqlText = @"CREATE TABLE SpecialCustomer (
								CUSTOMER_ID INT
							);";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.NotNull(results);
            Assert.NotNull(results.TableAnnotationResults);
            Assert.NotNull(results.ColumnAnnotationResults);
        }

        [Fact]
        public void ParsesEmptyAnnotations()
        {
            var sqlText = @"CREATE TABLE SpecialCustomer (
								CUSTOMER_ID INT
							);";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Empty(results.TableAnnotationResults);
            Assert.Empty(results.ColumnAnnotationResults);
        }

        [Fact]
        public void ParsesSingleTableAnnotation()
        {
            var sqlText = @"CREATE TABLE SpecialCustomer /* {extends:Customer} */ (
								CUSTOMER_ID INT
							);";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Single(results.TableAnnotationResults);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetValidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidTableAnnotation(string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE SpecialCustomer{annotation}(
								CUSTOMER_ID INT
							);";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var tableAnnotation = results.TableAnnotationResults.FirstOrDefault();

            Assert.NotNull(tableAnnotation);
            Assert.Equal("SpecialCustomer", tableAnnotation.TableName);
            Assert.Equal(expectedTextAnnotation, tableAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetValidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidTableAnnotationWhenMissingSemiColon(string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE SpecialCustomer{annotation}(
								CUSTOMER_ID INT
							)";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var tableAnnotation = results.TableAnnotationResults.FirstOrDefault();

            Assert.NotNull(tableAnnotation);
            Assert.Equal("SpecialCustomer", tableAnnotation.TableName);
            Assert.Equal(expectedTextAnnotation, tableAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetInvalidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void NotParsesInvalidTableAnnotation(string annotation)
        {
            var sqlText = $@"CREATE TABLE SpecialCustomer{annotation}(
        	                    CUSTOMER_ID INT IDENTITY(1,1) PRIMARY KEY
                            );";

            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Empty(results.TableAnnotationResults);
        }

        [Fact]
        public void ParsesSingleColumnAnnotation()
        {
            var sqlText = @"CREATE TABLE SpecialCustomer (
								CUSTOMER_ID INT /* {trait:means.something} */ 
							);";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Single(results.ColumnAnnotationResults);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotationsWithExpectedValue), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidColumnAnnotation(string type, string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE Customer(
        	                    CUSTOMER_ID {type}{annotation}
                            );";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotation = results.ColumnAnnotationResults.FirstOrDefault();

            Assert.NotNull(columnAnnotation);
            Assert.Equal("Customer", columnAnnotation.TableName);
            Assert.Equal("CUSTOMER_ID", columnAnnotation.ColumnName);
            Assert.Equal(expectedTextAnnotation, columnAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotationsWithExpectedValue), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidColumnAnnotationWhenMissingSemiColon(string type, string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE Customer(
        	                    CUSTOMER_ID {type}{annotation}
                            )";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotation = results.ColumnAnnotationResults.FirstOrDefault();

            Assert.NotNull(columnAnnotation);
            Assert.Equal("Customer", columnAnnotation.TableName);
            Assert.Equal("CUSTOMER_ID", columnAnnotation.ColumnName);
            Assert.Equal(expectedTextAnnotation, columnAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotationsWithExpectedValue), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidColumnAnnotationWhenMultipleColumnsArePresent(string type, string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE Customer(
        	                    CUSTOMER_ID INT,
                                CUSTOMER_NAME {type}{annotation},
                                CUSTOMER_AGE INT
                            );";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotation = results.ColumnAnnotationResults.FirstOrDefault();

            Assert.NotNull(columnAnnotation);
            Assert.Equal("Customer", columnAnnotation.TableName);
            Assert.Equal("CUSTOMER_NAME", columnAnnotation.ColumnName);
            Assert.Equal(expectedTextAnnotation, columnAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotationsWithExpectedValue), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesValidColumnAnnotationWhenMultipleColumnsArePresentInline(string type, string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE Customer{annotation}(
        	                    CUSTOMER_ID INT, CUSTOMER_NAME {type}{annotation}, CUSTOMER_AGE INT
                            );";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotation = results.ColumnAnnotationResults.FirstOrDefault();

            Assert.NotNull(columnAnnotation);
            Assert.Equal("Customer", columnAnnotation.TableName);
            Assert.Equal("CUSTOMER_NAME", columnAnnotation.ColumnName);
            Assert.Equal(expectedTextAnnotation, columnAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesSingleColumnAnnotationWhenMultipleColumnsArePresent(string type, string annotation)
        {
            var sqlText = $@"CREATE TABLE Customer{annotation}(
        	                    CUSTOMER_ID INT,
                                CUSTOMER_NAME {type}{annotation},
                                CUSTOMER_AGE INT
                            );";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Single(results.ColumnAnnotationResults);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesSingleColumnFromTableAnnotationWhenMultipleTablesArePresent(string type, string annotation)
        {
            var sqlText = $@"
                     CREATE TABLE Customer{annotation}
                     (
                      CUSTOMER_ID INT IDENTITY(1,1) PRIMARY KEY,
                      CUSTOMER_NAME {type}{annotation} */
                     );

                     CREATE TABLE CustomerAddresses
                     (
                      CUSTOMER_ADDRESS_ID INT IDENTITY(1,1) PRIMARY KEY,
                      CUSTOMER_ID INT NOT NULL,
                      LINE VARCHAR(100) NOT NULL /* {annotation} */,
                      STATE CHAR(2),
                      COUNTRY CHAR(3),
                      FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
                     );

                     CREATE TABLE SpecialCustomer {annotation} (
                      SPECIAL_CARD_NUMBER CHAR(10) NOT NULL
                     );";

            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotations = results.ColumnAnnotationResults.Where(a => a.TableName == "Customer");

            Assert.Single(columnAnnotations);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetTypesAndValidAnnotationsWithExpectedValue), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void ParsesSingleColumnAnnotationWhenMultipleColumnsAndGoStatementArePresent(string type, string annotation, string expectedTextAnnotation)
        {
            var sqlText = $@"CREATE TABLE Customer(
        	                    CUSTOMER_ID INT,
                                CUSTOMER_NAME {type}{annotation},
                                CUSTOMER_AGE INT
                            )
                            GO";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);
            var columnAnnotation = results.ColumnAnnotationResults.FirstOrDefault();

            Assert.NotNull(columnAnnotation);
            Assert.Equal("Customer", columnAnnotation.TableName);
            Assert.Equal("CUSTOMER_NAME", columnAnnotation.ColumnName);
            Assert.Equal(expectedTextAnnotation, columnAnnotation.Result);
        }

        [Theory]
        [MemberData(nameof(SqlTextAnnotationParserDataGenerator.GetInvalidAnnotations), MemberType = typeof(SqlTextAnnotationParserDataGenerator))]
        public void DoesNotParseInvalidColumnAnnotation(string annotation)
        {
            var sqlText = $@"CREATE TABLE Customer(
        	                    CUSTOMER_ID INT{annotation}
                            );";
            var parser = new SqlTextAnnotationsParser();

            var results = parser.ParseTextAnnotations(sqlText);

            Assert.Empty(results.ColumnAnnotationResults);
        }
    }
}
