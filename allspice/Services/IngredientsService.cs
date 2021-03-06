using System;
using System.Collections.Generic;
using allspice.Models;
using allspice.Repositories;

namespace allspice.Services
{
    public class IngredientsService
    {
        private readonly IngredientsRepository _repo;

        public IngredientsService(IngredientsRepository repo)
        {
            _repo = repo;
        }

        internal Ingredient Create(Ingredient ingredientData)
        {
            return _repo.Create(ingredientData);
        }

        internal List<Ingredient> GetIngredients(int id)
        {
            return _repo.GetIngredients(id);
        }

        internal void Delete(int id, string userId)
        {
            Ingredient ingredient = Get(id);
            if (ingredient.CreatorId != userId)
            {
                throw new Exception("you do not own this ingredient");
            }
            _repo.Delete(id);
        }

        internal Ingredient Get(int id)
        {
            Ingredient ingredient = _repo.Get(id);
            if (ingredient == null)
            {
                throw new Exception("Invalid ingredient ID");
            }
            return ingredient;
        }

        internal Ingredient Edit(Ingredient ingredientData)
        {
            Ingredient original = Get(ingredientData.Id);
            if (original.CreatorId != ingredientData.CreatorId)
            {
                throw new Exception("you do not have rights to change this ingredient.");
            }
            original.Name = ingredientData.Name ?? original.Name;
            original.Quantity = ingredientData.Quantity ?? original.Quantity;
            _repo.Edit(original);
            return Get(original.Id);
        }
    }
}