using System;
using System.Collections.Generic;
using allspice.Models;
using allspice.Repositories;

namespace allspice.Services
{
    public class RecipesService
    {
        private readonly RecipesRepository _repo;

        public RecipesService(RecipesRepository repo)
        {
            _repo = repo;
        }

        //GET ALL
        internal List<Recipe> Get()
        {
            return _repo.Get();
        }

        //GET BY ID
        internal Recipe Get(int id)
        {
            Recipe recipe = _repo.Get(id);
            if (recipe == null)
            {
                throw new Exception("Invalid recipe ID");
            }
            return recipe;
        }

        //CREATE NEW
        internal Recipe Create(Recipe recipeData)
        {
            return _repo.Create(recipeData);
        }
        internal void Delete(int id, string userId)
        {
            Recipe recipe = Get(id);
            if (recipe.CreatorId != userId)
            {
                throw new Exception("Cannot delete other people's recipes.");
            }
            _repo.Delete(id);
        }

    }
}