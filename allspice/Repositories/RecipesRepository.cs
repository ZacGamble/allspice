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
            FROM recipes r
            JOIN accounts acct ON r.creatorId = acct.id;
            ";
            return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
            {
                recipe.CreatorId = account.Id;
                return recipe;
            }).ToList();
        }
        internal Recipe Get(int id)
        {
            string sql = @"
            SELECT
            r.*,
            acct.*
            FROM recipes r
            JOIN accounts acct ON r.creatorId = acct.id
            WHERE r.id = @id";
            return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
            {
                recipe.CreatorId = account.Id;
                return recipe;
            }, new { id }).FirstOrDefault();
        }

        internal Recipe Create(Recipe recipeData)
        {
            string sql = @"
           INSERT INTO recipes
           (picture, title, subtitle, category, creatorId)
           VALUES
           (@Picture, @Title, @Subtitle, @Category, @CreatorId);
           SELECT LAST_INSERT_ID();
           ";
            recipeData.Id = _db.ExecuteScalar<int>(sql, recipeData);
            return recipeData;
        }

        internal void Delete(int id)
        {
            string sql = "DELETE FROM recipes WHERE id = @id LIMIT 1";
            _db.Execute(sql, new { id });
        }
    }
}