using System.Data;
using System.Collections.Generic;
using System.Linq;
using allspice.Models;
using Dapper;

namespace allspice.Repositories
{
    public class RecipesRepository
    {
        private readonly IDbConnection _db;
        public RecipesRepository(IDbConnection db)
        {
            _db = db;
        }
        internal List<Recipe> Get()
        {
            string sql = @"
            SELECT
            r.*,
            acct.*
            FROM rec r
            JOIN accounts acct ON r.creatorId = acct.id;
            ";
            return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
            {
                recipe.CreatorId = account.Id;
                return recipe;
            }).ToList();
        }
    }
}