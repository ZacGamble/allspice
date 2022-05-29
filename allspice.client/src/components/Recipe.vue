<template>
  <div class="col-sm-4 col-md-3 p-2">
    <div
      @click="openRecipe()"
      class="custom-card selectable"
      :title="'click to see details of ' + recipe.title"
    >
      <div class="p-2">
        <img
          class="img-fluid rounded"
          :src="recipe.picture"
          alt="photo of the recipe submission"
        />
      </div>
      <div class="p-1">
        <h2>
          {{ recipe.title }}
        </h2>
        <h5>{{ recipe.subtitle }}</h5>
      </div>
    </div>
  </div>
  <Modal id="open-recipe-modal">
    <template #modal-header-slot>
      <div class="d-flex flex-column">
        <h3>
          {{ focusRecipe?.title }}
        </h3>
        <h6>
          {{ focusRecipe?.subtitle }}
        </h6>
      </div>
    </template>
    <template #modal-body-slot>
      <!-- Ingredients and steps here TODO -->
      <div class="container">
        <div class="row">
          <div class="col-6">
            <span>
              <Steps v-for="s in activeSteps" :key="s.id" :steps="s" />
            </span>
          </div>
          <div class="col-6">
            <ul>
              <Ingredients
                v-for="i in activeIngredients"
                :key="i.id"
                :ingredients="i"
              />
            </ul>
          </div>
        </div>
      </div>
    </template>
  </Modal>
</template>


<script>
import { Modal } from 'bootstrap'
import { AppState } from '../AppState'
import { recipesService } from '../services/RecipesService'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { computed } from '@vue/reactivity'
export default {
  props: {
    recipe: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    return {
      async openRecipe(id) {
        try {

          AppState.openRecipe = props.recipe;
          await recipesService.getRecipeDetails(AppState.openRecipe.id);
          Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).show()

          //   logger.log("Recipe component > Appstate.openRecipe", AppState.openRecipe)
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      focusRecipe: computed(() => AppState.openRecipe),
      activeIngredients: computed(() => AppState.activeIngredients),
      activeSteps: computed(() => AppState.activeSteps)
    }
  }
}
</script>


<style lang="scss" scoped>
.custom-card {
  border-radius: 5%;
  border: 2px double black;
  box-shadow: 4px 3px 7px 3px rgb(24, 23, 23);
}
.custom-card:hover {
  transform: scale(1.02);
}
</style>