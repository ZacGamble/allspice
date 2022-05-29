import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";


class RecipesService {
    async getRecipes(){
        const res = await api.get('api/recipes');
        AppState.activeRecipes = res.data
    }
    async getRecipeDetails(id){
        const ingredientsRes = await api.get('api/recipes/' + id + '/ingredients');
        const stepsRes = await api.get('api/recipes/' + id + '/steps')
        AppState.activeIngredients = ingredientsRes.data
        logger.log("RecipesService > active ingredients", AppState.activeIngredients)
        AppState.activeSteps = stepsRes.data
        logger.log("RecipesService > active Steps", AppState.activeSteps)

    }
    async deleteRecipe(recipeId){
        const res = await api.delete('api/recipes/' + recipeId);
        logger.log('recipes service > delete method', res.data)
    }
}

export const recipesService = new RecipesService();