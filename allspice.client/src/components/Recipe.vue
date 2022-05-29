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
        <div class="d-flex justify-content-between">
          <h2>
            {{ recipe.title }}
          </h2>
          <i
            v-show="recipe.creatorId == accountId"
            class="mdi mdi-pencil action pop"
          ></i>
        </div>
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
        v-show="recipe.creatorId == accountId"
      >
        DESTROY
      </button>
    </template>
    <template #modal-body-slot>
      <div class="container">
        <div class="row">
          <div class="col-7">
            <div class="d-flex flex-column justify-content-between">
              <h6>
                <button
                  @click="createSteps()"
                  title="click to add"
                  class="btn btn-success"
                >
                  +
                </button>
                Steps
              </h6>
            </div>
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
            <div
              v-show="recipe.creatorId == accountId"
              class="mdi mdi-pencil-outline pop fs-5 action"
              title="edit ingredients"
            >
              Edit
            </div>
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

  <!-- Enter new Steps -->
  <Modal id="create-step">
    <template #modal-header-slot>Change your instructions here</template>
    <template #modal-body-slot>
      <form @submit.prevent="handleStepSubmit()">
        <div class="d-flex flex-column">
          <input
            type="number"
            placeholder="Which step in the recipe?"
            v-model="stepForm.position"
          />
          <input
            type="text"
            placeholder="Your instructions..."
            v-model="stepForm.body"
            minlength="5"
            required
          />
          <button class="btn success" type="submit">Submit</button>
        </div>
      </form>
    </template>
  </Modal>
  <!-- Enter new ingredients -->
  <Modal id="create-ingredients">
    <template #modal-header-slot>Change your ingredients here</template>
    <template #modal-body-slot
      ><form @submit.prevent="handleIngredientSubmit()">
        <div class="d-flex flex-column p-2">
          <input
            type="text"
            placeholder="Name of ingredient..."
            v-model="ingredientForm.name"
            class="my-2 rounded p-2"
          />
          <input
            type="text"
            placeholder="Amount or measurement..."
            v-model="ingredientForm.quantity"
            class="my-2 rounded p-2"
          />
          <button class="btn btn-success" type="submit">Submit</button>
        </div>
      </form></template
    >
  </Modal>
</template>


<script>
import { Modal } from 'bootstrap'
import { AppState } from '../AppState'
import { recipesService } from '../services/RecipesService'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { computed, ref } from '@vue/reactivity'
export default {
  props: {
    recipe: {
      type: Object,
      required: true
    }
  },

  setup(props) {

    const stepForm = ref({})
    const ingredientForm = ref({})
    return {
      stepForm,
      ingredientForm,
      user: computed(() => AppState.user),
      accountId: computed(() => AppState.account.id),
      recipeId: computed(() => AppState.openRecipe.creatorId),

      async handleStepSubmit() {
        try {
          await recipesService.submitNewStep(stepForm.value)

          await Modal.getOrCreateInstance(document.getElementById('create-step')).hide()
          Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).show()

        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async handleIngredientSubmit() {
        try {
          await recipesService.submitNewIngredient(ingredientForm.value)

          Modal.getOrCreateInstance(document.getElementById('create-ingredients')).hide()
          Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).show()
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async createSteps() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        Modal.getOrCreateInstance(document.getElementById('create-step')).show()
      },

      async createIngredients() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        Modal.getOrCreateInstance(document.getElementById('create-ingredients')).show()
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

          logger.log("Recipe component > Appstate.openRecipe", AppState.openRecipe)
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
.pop:hover {
  transform: scale(1.03);
}
</style>