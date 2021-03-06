﻿namespace Dapper.FastCrud.SqlStatements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper.FastCrud.SqlBuilders;

    /// <summary>
    /// SQL statement factory targeting relationships.
    /// </summary>
    internal class ThreeEntitiesRelationshipSqlStatements<TMainEntity,
                                                          TFirstJoinedEntity,
                                                          TSecondJoinedEntity>
        : RelationshipSqlStatements<TMainEntity>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreeEntitiesRelationshipSqlStatements(TwoEntitiesRelationshipSqlStatements<TMainEntity, TFirstJoinedEntity> relationshipSqlStatements, GenericStatementSqlBuilder joinedEntitySqlStatements)
            : base(relationshipSqlStatements, joinedEntitySqlStatements)
        {
        }

        /// <summary>
        /// Combines the current instance with a joined entity.
        /// </summary>
        public override ISqlStatements<TMainEntity> CombineWith<TThirdJoinedEntity>(ISqlStatements<TThirdJoinedEntity> joinedEntitySqlStatements)
        {
            return new FourEntitiesRelationshipSqlStatements<TMainEntity, TFirstJoinedEntity, TSecondJoinedEntity, TThirdJoinedEntity>(this, joinedEntitySqlStatements.SqlBuilder);
        }

        protected override IEnumerable<TMainEntity> Query(
            IDbConnection connection,
            string statement,
            string splitOnCondition,
            object parameters,
            bool buffered,
            IDbTransaction transaction,
            int? commandTimeout,
            RelationshipEntityInstanceBuilder relationshipInstanceBuilder)
        {
            return connection.Query<TMainEntity, TFirstJoinedEntity, TSecondJoinedEntity, TMainEntity>(
                statement,
                (mainEntity, firstJoinedEntity, secondJoinedEntity) =>
                {
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref mainEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref firstJoinedEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref secondJoinedEntity);
                    relationshipInstanceBuilder.EndResultSetRow();

                    return mainEntity;
                },
                parameters,
                buffered: buffered,
                splitOn: splitOnCondition,
                transaction: transaction,
                commandTimeout: commandTimeout);
        }

        protected override Task<IEnumerable<TMainEntity>> QueryAsync(
            IDbConnection connection,
            string statement,
            string splitOnCondition,
            object parameters,
            bool buffered,
            IDbTransaction transaction,
            int? commandTimeout,
            RelationshipEntityInstanceBuilder relationshipInstanceBuilder)
        {
            return connection.QueryAsync<TMainEntity, TFirstJoinedEntity, TSecondJoinedEntity, TMainEntity>(
                statement,
                (mainEntity, firstJoinedEntity, secondJoinedEntity) =>
                {
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref mainEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref firstJoinedEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref secondJoinedEntity);
                    relationshipInstanceBuilder.EndResultSetRow();

                    return mainEntity;
                },
                parameters,
                buffered: buffered,
                splitOn: splitOnCondition,
                transaction: transaction,
                commandTimeout: commandTimeout);
        }
    }
}