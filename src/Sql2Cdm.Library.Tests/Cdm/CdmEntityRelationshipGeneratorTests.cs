using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Sql2Cdm.Library.Cdm;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Tests.Cdm.Extensions;
using System.Data;
using System.Linq;
using Xunit;

namespace Sql2Cdm.Library.Tests.Cdm
{
    public class CdmEntityRelationshipGeneratorTests
    {
        private readonly CdmCorpusDefinition cdmCorpus;
        private readonly CdmReferenceResolver resolver;

        public CdmEntityRelationshipGeneratorTests()
        {
            cdmCorpus = new CdmCorpusDefinition();
            resolver = new CdmReferenceResolver();
        }

        [Fact]
        public void RelationshipsIsNotNull()
        {
            CdmEntityRelationshipGenerator generator = BuildCdmEntityRelationshipGenerator();
            var table = new Table("Customer");

            var relationships = generator.GenerateRelationships(table);

            Assert.NotNull(relationships);
        }

        [Fact]
        public void RelationshipsFromTableWithNoColumnsIsEmpty()
        {
            CdmEntityRelationshipGenerator generator = BuildCdmEntityRelationshipGenerator();
            var table = new Table("Customer");

            var relationships = generator.GenerateRelationships(table);

            Assert.Empty(relationships);
        }

        [Fact]
        public void RelationshipsFromTableWithSingleColumnContainsSingleRelationship()
        {
            CdmEntityRelationshipGenerator generator = BuildCdmEntityRelationshipGenerator();
            var fk = new Table("Customer").WithColumn("ID", SqlDbType.Int).GetColumn();
            var table = new Table("CustomerAddresses").WithColumn("C_ID", SqlDbType.Int, foreignKey: fk);

            var relationships = generator.GenerateRelationships(table);

            Assert.Single(relationships);
        }

        [Fact]
        public void RelationshipMatchesTableColumnForeignKeyConstraint()
        {
            CdmEntityRelationshipGenerator generator = BuildCdmEntityRelationshipGenerator();
            var fk = new Table("Customer").WithColumn("ID", SqlDbType.Int).GetColumn();
            var table = new Table("CustomerAddresses").WithColumn("C_ID", SqlDbType.Int, foreignKey: fk);

            CdmE2ERelationship relationship = generator.GenerateRelationships(table).First();

            Assert.Equal("C_ID", relationship.FromEntityAttribute);
            Assert.Equal("ID", relationship.ToEntityAttribute);
        }

        [Fact]
        public void RelationshipMatchesTableColumnForeignTableConstraint()
        {
            CdmEntityRelationshipGenerator generator = BuildCdmEntityRelationshipGenerator();
            var fk = new Table("Customer").WithColumn("ID", SqlDbType.Int).GetColumn();
            var table = new Table("CustomerAddresses").WithColumn("C_ID", SqlDbType.Int, foreignKey: fk);

            CdmE2ERelationship relationship = generator.GenerateRelationships(table).First();
            
            Assert.Matches(".*.cdm.json/CustomerAddresses", relationship.FromEntity);
            Assert.Matches(".*.cdm.json/Customer", relationship.ToEntity);
        }

        private CdmEntityRelationshipGenerator BuildCdmEntityRelationshipGenerator()
        {
            return new CdmEntityRelationshipGenerator(cdmCorpus, resolver);
        }
    }
}
