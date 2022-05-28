using System.Collections.Generic;
using allspice.Models;
using allspice.Repositories;

namespace allspice.Services
{
    public class StepsService
    {
        private readonly StepsRepository _repo;
        public StepsService(StepsRepository repo)
        {
            _repo = repo;
        }

        internal List<Step> GetStepsByRecipe(int id)
        {
            return _repo.GetStepsByRecipe(id);
        }
    }
}