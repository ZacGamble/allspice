using System.Collections.Generic;
using System.Linq;
using System.Data;
using allspice.Models;
using Dapper;

namespace allspice.Repositories
{
    public class StepsRepository
    {
        private readonly IDbConnection _db;
        public StepsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal List<Step> GetStepsByRecipe(int id)
        {
            string sql = @"
            SELECT
            a.*,
            st.*
            FROM steps st
            JOIN accounts a ON st.creatorId = a.id
            WHERE st.recipeId = @id;
            ";
            return _db.Query<Account, Step, Step>(sql, (acct, st) =>
            {
                st.CreatorId = acct.Id;
                return st;
            }, new { id }).ToList();
        }

        internal Step GetById(int id)
        {
            string sql = @"
            SELECT
            s.*
            FROM steps s
            WHERE s.id = @id;
            ";
            return _db.Query<Step>(sql, new { id }).FirstOrDefault();
        }

        internal Step Create(Step stepData)
        {
            string sql = @"
           INSERT INTO steps
           (position, body, recipeId, creatorId)
           VALUES
           (@Position, @Body, @RecipeId, @CreatorId);
           SELECT LAST_INSERT_ID();
           ";
            stepData.Id = _db.ExecuteScalar<int>(sql, stepData);
            return stepData;
        }

        internal void Edit(Step original)
        {
            string sql = @"
            UPDATE steps
            SET
            position = @Position,
            body = @Body
            WHERE id = @Id;
            ";
            _db.Execute(sql, original);
        }

        internal void Delete(int id)
        {
            string sql = @"
           DELETE FROM steps WHERE id = @id LIMIT 1";
            _db.Execute(sql, new { id });
        }
    }
}