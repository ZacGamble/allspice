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
        // AppState.activeIngredients = ingredientsRes.data REVIEW
        logger.log("RecipesService > active ingredients", AppState.activeIngredients)
        AppState.activeSteps = stepsRes.data
        // logger.log("RecipesService > active Steps", AppState.activeSteps) REVIEW
    }
    async createRecipe(formData){
        const res = await api.post('api/recipes', formData);
        AppState.activeRecipes = [res.data, ...AppState.activeRecipes];
        // logger.log("recipe Service > create >", res.data); REVIEW
    }
    async deleteRecipe(recipeId){
        const res = await api.delete('api/recipes/' + recipeId);
        const index = AppState.activeRecipes.findIndex(i => i.id == recipeId);
        AppState.activeRecipes.splice(index, 1);
        logger.log('recipes service > delete method', res.data)
    }
}

export const recipesService = new RecipesService();