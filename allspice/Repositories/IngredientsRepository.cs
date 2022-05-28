using System.Data;
using System.Linq;
using allspice.Models;
using System.Collections.Generic;
using Dapper;

namespace allspice.Repositories
{
    public class IngredientsRepository
    {
        private readonly IDbConnection _db;
        public IngredientsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Ingredient Create(Ingredient ingredientData)
        {
            string sql = @"
           INSERT INTO ingredients
           (name, quantity, recipeId, creatorId)
           VALUES
           (@Name, @Quantity, @RecipeId, @CreatorId);
           SELECT LAST_INSERT_ID();
           ";
            ingredientData.Id = _db.ExecuteScalar<int>(sql, ingredientData);
            return ingredientData;
        }

        internal List<Ingredient> GetIngredients(int id)
        {
            string sql = @"
            SELECT
            a.*,
            i.*
            FROM ingredients i
            JOIN accounts a ON i.creatorId = a.id
            WHERE i.recipeId = @id;
            ";
            return _db.Query<Account, Ingredient, Ingredient>(sql, (acct, ing) =>
            {
                ing.CreatorId = acct.Id;
                return ing;
            }, new { id }).ToList();
        }

        internal void Delete(int id)
        {
            string sql = @"
           DELETE FROM ingredients WHERE id = @id LIMIT 1";
            _db.Execute(sql, new { id });
        }

        internal Ingredient Get(int id)
        {
            string sql = @"
            SELECT
            i.*
            FROM ingredients i
            WHERE i.id = @id;
            ";
            return _db.Query<Ingredient>(sql, new { id }).FirstOrDefault();
        }

        internal void Edit(Ingredient original)
        {
            string sql = @"
            UPDATE ingredients
            SET
            name = @Name,
            quantity = @Quantity
            WHERE id = @Id;
            ";
            _db.Execute(sql, original);
        }
    }
}