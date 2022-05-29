import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";


class RecipesService {
    // #region Recipe Actions
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
    async createRecipe(formData){
        const res = await api.post('api/recipes', formData);
        AppState.activeRecipes = [res.data, ...AppState.activeRecipes];
    }
    async deleteRecipe(recipeId){
        const res = await api.delete('api/recipes/' + recipeId);
        const index = AppState.activeRecipes.findIndex(i => i.id == recipeId);
        AppState.activeRecipes.splice(index, 1);
    }
    // #endregion

    // #region Ingredient actions
    async submitNewIngredient(formData){
        formData.creatorId = AppState.openRecipe.creatorId
        formData.recipeId = AppState.openRecipe.id
        const res = await api.post('api/ingredients', formData)
        AppState.activeIngredients = [...AppState.activeIngredients, res.data]
    }
    async deleteIngredient(){
        const id = AppState.selectedIngredient.id
        await api.delete('api/ingredients/' + id)
        const index = AppState.activeIngredients.findIndex(i => i.id == id);
        AppState.activeIngredients.splice(index, 1);
    }
    // #region Step actions
    async submitNewStep(formData){
        formData.creatorId = AppState.openRecipe.creatorId
        formData.recipeId = AppState.openRecipe.id
        logger.log("the form data", formData)
        const res = await api.post('api/steps', formData)
        AppState.activeSteps = [...AppState.activeSteps, formData]
        logger.log("the Appstate for steps >", AppState.activeSteps, "the response for step submit > ", res.data)
    }
    async editStep(formData){
        formData.creatorId = AppState.openRecipe.creatorId;
        formData.recipeId = AppState.openRecipe.id
        const res = await api.post('api/steps/' )
    }
    async deleteStep(){
        const id = AppState.selectedStep.id
        await api.delete('api/steps/' + id)
       const index = AppState.activeSteps.findIndex(i => i.id == id);
        AppState.activeSteps.splice(index, 1);
    }
}

export const recipesService = new RecipesService();