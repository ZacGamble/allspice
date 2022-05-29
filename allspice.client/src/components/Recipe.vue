<template>
  <div class="col-sm-4 p-2">
    <div class="custom-card">
      <div class="p-2">
        <div class="category">{{ recipe.category }}</div>
        <img
          @click="openRecipe()"
          class="img-fluid rounded action"
          :src="recipe.picture"
          alt="photo of the recipe submission"
          :title="'click to see details of ' + recipe.title"
        />
      </div>
      <div class="p-2">
        <h2>
          {{ recipe.title }}
        </h2>
        <div class="d-flex justify-content-between">
          <h5>{{ recipe.subtitle }}</h5>
        </div>
      </div>
    </div>
  </div>
  <!-- #region recipe modal -->
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
      <button
        @click="deleteRecipe()"
        type="button"
        class="close bg-danger"
        data-dismiss="modal"
        aria-label="Delete recipe"
        title="Delete recipe"
      >
        DESTROY
      </button>
    </template>
    <template #modal-body-slot>
      <div class="container">
        <div class="row">
          <div class="col-7">
            <h6>
              <button
                @click="modifySteps()"
                title="click to add"
                class="btn btn-success"
              >
                +
              </button>
              Steps
            </h6>
            <span>
              <Steps v-for="s in activeSteps" :key="s.id" :steps="s" />
            </span>
          </div>
          <div class="col-5">
            <h6>
              <button
                @click="modifyIngredients()"
                title="click to add"
                class="btn btn-success"
              >
                +
              </button>
              Ingredients
            </h6>
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
    <template #modal-footer-slot>
      <button
        @click="closeModal()"
        type="button"
        class="btn btn-secondary"
        data-dismiss="open-recipe-modal"
      >
        Close
      </button>
    </template>
  </Modal>
  <!-- #endregion -->
  <Modal id="modify-step">
    <template #modal-header-slot>Change your instructions here</template>
    <template #modal-body-slot>Body</template>
  </Modal>
  <Modal id="modify-ingredients">
    <template #modal-header-slot>Change your ingredients here</template>
    <template #modal-body-slot>Body</template>
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
      user: computed(() => AppState.user),
      accountId: computed(() => AppState.account.id),
      recipeId: computed(() => AppState.openRecipe.creatorId),

      async modifySteps() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        Modal.getOrCreateInstance(document.getElementById('modify-step')).show()
      },

      async deleteRecipe(theRecipe) {
        try {
          if (AppState.account.id != AppState.openRecipe.creatorId) {
            throw new Error('You do not have permission to do that!')
          }
          if (await Pop.confirm()) {
            await recipesService.deleteRecipe(AppState.openRecipe.id);
            Pop.toast("Your recipe was deleted.", 'success');
            Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
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

      closeModal() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        return
      },

      focusRecipe: computed(() => AppState.openRecipe),
      activeIngredients: computed(() => AppState.activeIngredients),
      activeSteps: computed(() => AppState.activeSteps)
    }
  }
}
</script>


<style lang="scss" scoped>
.category {
  text-shadow: 1px 3px 2px black;
  color: white;
  transform: translateY(2em);
  padding-left: 1em;
  font-size: 20px;
  font-weight: bold;
}
.custom-card {
  border-radius: 5%;
  border: 2px double black;
  box-shadow: 4px 3px 7px 3px rgb(24, 23, 23);
}
.custom-card:hover {
  transform: scale(1.02);
}
</style>