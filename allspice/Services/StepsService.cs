using System;
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

        internal Step Create(Step stepData)
        {
            return _repo.Create(stepData);
        }


        internal Step GetById(int id)
        {
            Step step = _repo.GetById(id);
            if (step == null)
            {
                throw new Exception("Invalid step ID");
            }
            return step;
        }
        internal Step Edit(Step stepData)
        {
            Step original = GetById(stepData.Id);
            if (original.CreatorId != stepData.CreatorId)
            {
                throw new Exception("You do not have rights to change this step.");
            }
            original.Position = stepData.Position;
            original.Body = stepData.Body ?? original.Body;
            _repo.Edit(original);
            return GetById(original.Id);
        }

        internal void Delete(int id, string userId)
        {
            Step step = GetById(id);
            if (step.CreatorId != userId)
            {
                throw new Exception("you do not own this ingredient");
            }
            _repo.Delete(id);
        }
    }
}