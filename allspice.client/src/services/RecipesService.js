import { AppState } from "../AppState";
import { api } from "./AxiosService";


class RecipesService {
    async getRecipes(){
        const res = await api.get('api/recipes');
        AppState.activeRecipes = res.data
    }
}

export const recipesService = new RecipesService();