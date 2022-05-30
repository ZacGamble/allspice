import { reactive } from 'vue'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  user: {},
  account: {},
  favorites: {},
  activeRecipes: [],
  backupRecipes: [],
  openRecipe: null,
  activeIngredients: {},
  selectedIngredient: null,
  activeSteps: {},
  selectedStep: null
})
