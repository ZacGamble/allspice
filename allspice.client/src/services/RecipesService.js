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
        AppState.activeSteps = stepsRes.data        
    }
    async getRecipesByCategory(category){
        const res = await api.get('api/recipes')
        AppState.activeRecipes = res.data.filter(r => r.category == category.category)
        logger.log(AppState.activeRecipes)
    }
    async getRecipesIOwn(){
        const res = await api.get('api/recipes')
        AppState.activeRecipes = res.data.filter(r => r.creatorId == AppState.account.id)
    }
    async filterByFavorite(){
        const res = await api.get('api/recipes')
        const favoriteRecipes = AppState.favorites.filter(f => f.accountId == AppState.activeRecipes.creatorId)
        debugger
        AppState.activeRecipes = res.data.filter(r => r.id == favoriteRecipes.recipeId)
    }
    async createRecipe(formData){
        const res = await api.post('api/recipes', formData);
        AppState.activeRecipes = [res.data, ...AppState.activeRecipes];
    }
    async editRecipe(formData){
        const recipeId = AppState.openRecipe.id
        formData.id = AppState.openRecipe.id
        formData.creatorId = AppState.openRecipe.creatorId
        const res = await api.put('api/recipes/' + recipeId, formData)
        AppState.openRecipe = AppState.openRecipe
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
    // #endregion

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
    // #endregion steps

    // Favorite Actions
    async getFavorites(){
        const res = await api.get('accounts/favorites');
        AppState.favorites = res.data
        logger.log("favorites > ", AppState.favorites);
    }
}

export const recipesService = new RecipesService();