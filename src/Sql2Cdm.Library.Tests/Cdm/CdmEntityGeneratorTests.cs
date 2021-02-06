using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Sql2Cdm.Library.Cdm;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Models.Annotations;
using Sql2Cdm.Library.Tests.Cdm.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace Sql2Cdm.Library.Tests.Cdm
{
    public class CdmEntityGeneratorTests
    {
        private readonly CdmCorpusDefinition cdmCorpus;
        private readonly CdmDocumentDefinition doc;
        private readonly CdmReferenceResolver resolver;

        public CdmEntityGeneratorTests()
        {
            cdmCorpus = new CdmCorpusDefinition();
            resolver = new CdmReferenceResolver();
            doc = cdmCorpus.MakeObject<CdmDocumentDefinition>(CdmObjectType.DocumentDef);
        }

        [Fact]
        public void CdmEntityDefinitionIsNotNull()
        {
            CdmEntityGenerator generator = BuildCdmEntityGenerator();
            var table = new Table("Customer");

            CdmEntityDefinition entity = generator.GenerateEntity(table);

            Assert.NotNull(entity);
        }

        [Fact]
        public void CdmEntityDefinitionNameIsEqualToTableName()
        {
            var generator = BuildCdmEntityGenerator();
            var table = new Table("Customer");

            CdmEntityDefinition entity = generator.GenerateEntity(table);

            Assert.Equal("Customer", entity.EntityName);
        }

        [Fact]
        public void CdmEntityDefinitionNameIsValid()
        {
            var generator = BuildCdmEntityGenerator();
            var table = new Table("Customer")
                                .WithColumn("Name", SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            bool isValid = entity.Validate();

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void TableWithNColumnsGeneratesCdmEntityDefinitionWithNAttributes(int n)
        {
            var generator = BuildCdmEntityGenerator();
            var table = new Table("Customer");
            for (int i = 0; i < n; i++)
                table.WithColumn($"Attr{i}", SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            var count = entity.Attributes.Count;

            Assert.Equal(n, count);
        }

        [Fact]
        public void CdmTypeAttributeDefinitionNotNull()
        {
            var generator = BuildCdmEntityGenerator();
            var Attributename = "Name";
            var table = new Table("Customer")
                                .WithColumn(Attributename, SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(Attributename);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void CdmTypeAttributeDefinitionIsValid()
        {
            var generator = BuildCdmEntityGenerator();
            var Attributename = "Name";
            var table = new Table("Customer")
                                .WithColumn(Attributename, SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(Attributename);
            bool isValid = attribute.Validate();

            Assert.True(isValid);
        }

        [Fact]
        public void CdmTypeAttributeDefinitionHasName()
        {
            var generator = BuildCdmEntityGenerator();
            var attributeName = "Name";
            var table = new Table("Customer")
                                .WithColumn(attributeName, SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(attributeName);

            Assert.Equal(attributeName, attribute.Name);
        }

        [Fact]
        public void CdmTypeAttributeDefinitionHasDataType()
        {
            var generator = BuildCdmEntityGenerator();
            var attributeName = "Name";
            var attributeType = cdmCorpus.CreateDataType(CdmDataFormat.String);
            var table = new Table("Customer")
                                .WithColumn(attributeName, SqlDbType.VarChar);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(attributeName);

            Assert.NotNull(attribute);
            Assert.Equal(attributeType.NamedReference, attribute.DataType.NamedReference);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CdmTypeAttributeDefinitionHasNullability(bool isNullable)
        {
            var generator = BuildCdmEntityGenerator();
            var attributeName = "Name";
            var table = new Table("Customer")
                                .WithColumn(attributeName, SqlDbType.VarChar, isNullable: isNullable);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(attributeName);

            Assert.Equal(isNullable, attribute.IsNullable);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CdmTypeAttributeDefinitionHasPrimaryKey(bool isPrimaryKey)
        {
            var generator = BuildCdmEntityGenerator();
            var attributeName = "Name";
            var table = new Table("Customer")
                                .WithColumn(attributeName, SqlDbType.VarChar, isPrimaryKey: isPrimaryKey);

            CdmEntityDefinition entity = generator.GenerateEntity(table);
            CdmTypeAttributeDefinition attribute = entity.GetAttribute(attributeName);

            Assert.Equal(isPrimaryKey, attribute.IsPrimaryKey);
        }

        [Fact]
        public void CdmEntityDefinitionCanExtendOtherCdmEntity()
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new ExtendsAnnotation("Customer");
            var table = new Table("SpecialCustomer")
                            .WithColumn("ID", SqlDbType.Int, isPrimaryKey: true)
                            .WithAnnotation(annotation);

            CdmEntityDefinition entity = generator.GenerateEntity(table);

            Assert.NotNull(entity.ExtendsEntity);
            Assert.Equal("Customer", entity.ExtendsEntity.NamedReference);
        }

        [Fact]
        public void CdmEntityDefinitionAttributeHasSingleTrait()
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new TraitAnnotation("something");
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int, annotation: annotation);

            var entity = generator.GenerateEntity(table);
            var traits = entity.Attributes.First().AppliedTraits;

            Assert.NotNull(traits);
            Assert.Single(traits);
        }

        [Fact]
        public void CdmEntityDefinitionAttributeHasTraitWithNamedReference()
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new TraitAnnotation("something");
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int, annotation: annotation);

            var entity = generator.GenerateEntity(table);
            var traits = entity.Attributes.First().AppliedTraits;
            var trait = traits.Single();

            Assert.NotNull(trait);
            Assert.Equal("something", trait.NamedReference);
        }

        [Fact]
        public void CdmEntityDefinitionAttributeHasTraitWithNoArguments()
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new TraitAnnotation("Customer");
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int, annotation: annotation);

            var entity = generator.GenerateEntity(table);
            var traits = entity.Attributes.First().AppliedTraits;
            var trait = traits.Single();

            Assert.Empty(trait.Arguments);
        }

        [Fact]
        public void CdmEntityDefinitionAttributeHasTraitContainsSingleArgument()
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new TraitAnnotation("something");
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int, annotation: annotation);
            annotation.AddArgument("key", "value");

            var entity = generator.GenerateEntity(table);
            var traits = entity.Attributes.First().AppliedTraits;
            var trait = traits.Single();

            Assert.Single(trait.Arguments);
        }

        [InlineData("key", "value")]
        [InlineData("key", 3)]
        [InlineData("key", 3.14)]
        [InlineData(null, "value")]
        [InlineData("", "value")]
        [Theory]
        public void CdmEntityDefinitionAttributeHasTraitContainsArgument(string key, dynamic value)
        {
            var generator = BuildCdmEntityGenerator();
            var annotation = new TraitAnnotation("something");
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int, annotation: annotation);
            annotation.AddArgument(key, value);

            var entity = generator.GenerateEntity(table);
            var traits = entity.Attributes.First().AppliedTraits;
            var argument = traits.Single().Arguments.Single();

            Assert.Equal(key, argument.Name);
            Assert.Equal(value, argument.Value);
        }

        [InlineData("1")]
        [InlineData("1.2")]
        [InlineData("1.2.3")]
        [InlineData("1.0.0")]
        [InlineData("10.20.30")]
        [InlineData("100.200.300")]
        [Theory]
        public void CdmEntityDefinitionIsVersioned(string version)
        {
            var generator = BuildCdmEntityGenerator(version);
            var table = new Table("Customer");

            var entity = generator.GenerateEntity(table);

            Assert.Equal(version, entity.Version);
        }

        [InlineData("1")]
        [InlineData("1.2")]
        [InlineData("1.2.3")]
        [InlineData("1.0.0")]
        [InlineData("10.20.30")]
        [InlineData("100.200.300")]
        [Theory]
        public void CdmEntityDefinitionHasCdmVersionTrait(string version)
        {
            var generator = BuildCdmEntityGenerator(version);
            var table = new Table("Customer");

            var entity = generator.GenerateEntity(table);
            var trait = entity.ExhibitsTraits.FirstOrDefault(t => t.NamedReference == "is.CDM.entityVersion");
            var traitArg = trait?.Arguments?.FirstOrDefault(t => t.Name == "versionNumber");

            Assert.NotNull(trait);
            Assert.NotNull(traitArg);
            Assert.Equal(version, traitArg.Value);
        }

        [InlineData(1)]
        [InlineData(100000)]
        [Theory]
        public void CdmEntityDefinitionHasMaximumLength(int size)
        {
            var generator = BuildCdmEntityGenerator();
            var table = new Table("Customer")
                                .WithColumn("ID", SqlDbType.Int);
            var column = table.GetColumn();
            column.Length = new ColumnLength() { MaxSize = size };

            var entity = generator.GenerateEntity(table);
            var attribute = entity.Attributes.First() as CdmTypeAttributeDefinition;

            Assert.NotNull(attribute.MaximumLength);
            Assert.Equal(size, attribute.MaximumLength);
        }

        private CdmEntityGenerator BuildCdmEntityGenerator(string version = "")
        {
            return new CdmEntityGenerator(cdmCorpus, resolver, doc, version);
        }
    }
}
