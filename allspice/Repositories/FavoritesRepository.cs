using System.Collections.Generic;
using System.Data;
using System.Linq;
using allspice.Models;
using Dapper;

namespace allspice.Repositories
{
    public class FavoritesRepository
    {
        private readonly IDbConnection _db;
        public FavoritesRepository(IDbConnection db)
        {
            _db = db;
        }
        internal Favorite Create(Favorite favoriteData)
        {
            string sql = @"
            INSERT INTO favorites
            (recipeId, accountId, isFavorite)
            VALUES
            (@RecipeId, @AccountId, @IsFavorite);
            SELECT LAST_INSERT_ID();
            ";
            favoriteData.IsFavorite = true;
            favoriteData.Id = _db.ExecuteScalar<int>(sql, favoriteData);
            return favoriteData;
        }

        internal List<Favorite> GetFavorites()
        {
            string sql = @"
            SELECT
            f.*
            FROM favorites f
            ";
            return _db.Query<Favorite>(sql).ToList();
        }
    }
}