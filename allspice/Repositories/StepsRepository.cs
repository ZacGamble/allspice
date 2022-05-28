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
    }
}