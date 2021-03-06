﻿namespace Dapper.FastCrud.SqlStatements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper.FastCrud.SqlBuilders;

    /// <summary>
    /// SQL statement factory targeting relationships.
    /// </summary>
    /// <typeparam name="TMainEntity">Main entity type</typeparam>
    /// <typeparam name="TFirstJoinedEntity">Joined entity type</typeparam>
    internal class TwoEntitiesRelationshipSqlStatements<TMainEntity, TFirstJoinedEntity> :RelationshipSqlStatements<TMainEntity>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mainEntitySqlStatements">Main entity SQL statement builder</param>
        /// <param name="joinedEntitySqlStatements">Joined entity SQL statement builder</param>
        public TwoEntitiesRelationshipSqlStatements(ISqlStatements<TMainEntity> mainEntitySqlStatements, GenericStatementSqlBuilder joinedEntitySqlStatements)
            : base(mainEntitySqlStatements, joinedEntitySqlStatements)
        {
        }

        /// <summary>
        /// Combines the current instance with a joined entity.
        /// </summary>
        public override ISqlStatements<TMainEntity> CombineWith<TSecondJoinedEntity>(ISqlStatements<TSecondJoinedEntity> joinedEntitySqlStatements)
        {
            return new ThreeEntitiesRelationshipSqlStatements<TMainEntity, TFirstJoinedEntity, TSecondJoinedEntity>(this, joinedEntitySqlStatements.SqlBuilder);
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
            return connection.Query<TMainEntity, TFirstJoinedEntity, TMainEntity>(
                statement,
                (mainEntity, joinedEntity) =>
                {
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref mainEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref joinedEntity);
                    relationshipInstanceBuilder.EndResultSetRow();

                    return mainEntity;
                },
                parameters,
                buffered:buffered,
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
            return connection.QueryAsync<TMainEntity, TFirstJoinedEntity, TMainEntity>(
                statement,
                (mainEntity, joinedEntity) =>
                {
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref mainEntity);
                    relationshipInstanceBuilder.RegisterResultSetRowInstance(ref joinedEntity);
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
