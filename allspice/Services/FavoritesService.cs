using System.Collections.Generic;
using allspice.Models;
using allspice.Repositories;

namespace allspice.Services
{
    public class FavoritesService
    {
        private readonly FavoritesRepository _repo;
        public FavoritesService(FavoritesRepository repo)
        {
            _repo = repo;
        }
        internal Favorite Create(Favorite favoriteData)
        {
            return _repo.Create(favoriteData);
        }

        internal List<Favorite> GetFavorites()
        {
            return _repo.GetFavorites();
        }
    }
}