<template>
  <div class="col-sm-4 p-2">
    <div class="custom-card">
      <div class="p-2">
        <div class="category d-flex justify-content-between">
          {{ recipe.category }} <i class="mdi mdi-heart-outline me-2 fs-4"></i>
        </div>
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
          <!-- TODO Swap instances based on isFavorite -->
          <i class="mdi mdi-heart-outline me-2 fs-4 action"></i>
          <i class="mdi mdi-heart-outline me-2 fs-4 action"></i>
        </h3>
        <h6>
          {{ focusRecipe?.subtitle }}
        </h6>
        <i
          v-show="recipe.creatorId == accountId"
          class="mdi mdi-pencil action pop fs-3"
          title="edit recipe"
          @click="editRecipeModal()"
          >Edit Recipe</i
        >
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
                @click="createIngredients()"
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
      <div class="d-flex justify-content-between">
        <button
          v-if="selectedStep"
          class="btn btn-success"
          @click="openStepEditModal()"
        >
          Edit selected
        </button>
        <button
          @click="closeModal()"
          type="button"
          class="btn btn-secondary"
          data-dismiss="open-recipe-modal"
        >
          Close
        </button>
      </div>
    </template>
  </Modal>
  <!-- #endregion -->

  <!-- Edit Recipe Modal -->
  <Modal id="edit-recipe-form">
    <template #modal-header-slot>
      <span class="fs-2 fw-bold">Edit your recipe!</span>
    </template>
    <template #modal-body-slot>
      <form @submit.prevent="editRecipe()">
        <div class="d-flex flex-column">
          <input
            class="p-1 my-2 rounded"
            type="text"
            minlength="3"
            maxlength="18"
            placeholder="title..."
            required
            v-model="recipeForm.title"
          />
          <input
            class="p-1 my-2 rounded"
            type="text"
            placeholder="subtitle..."
            minlength="3"
            maxlength="20"
            v-model="recipeForm.subtitle"
          />
          <select
            name="category"
            class="p-1 my-2 rounded"
            minlength="3"
            placeholder="category..."
            required
            v-model="recipeForm.category"
          >
            <option value="breakfast">Breakfast</option>
            <option value="lunch">Lunch</option>
            <option value="dinner" selected>Dinner</option>
            <option value="dessert">Dessert</option>
          </select>
          <input
            class="p-1 my-2 rounded"
            type="url"
            placeholder="Image URL..."
            v-model="recipeForm.picture"
          />
        </div>
        <button type="submit" class="btn btn-info">Submit</button>
      </form>
      <div class="d-flex justify-content-end">
        <button
          @click="closeModal()"
          type="button"
          class="btn btn-secondary"
          data-dismiss="open-recipe-modal"
        >
          Close
        </button>
      </div>
    </template>
  </Modal>

  <!-- Enter new Steps -->
  <Modal id="create-step">
    <template #modal-header-slot>Change your instructions here</template>
    <template #modal-body-slot>
      <form @submit.prevent="handleStepSubmit()">
        <div class="d-flex flex-column">
          <input
            class="mb-2 rounded p-2 border"
            type="number"
            placeholder="Which step in the recipe?"
            v-model="stepForm.position"
          />
          <input
            class="mb-2 rounded p-2 border"
            type="text"
            placeholder="Your instructions..."
            v-model="stepForm.body"
            minlength="5"
            required
          />
          <button class="btn btn-success" type="submit">Submit</button>
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
          <button class="btn btn-success" type="submit" title="submit">
            Submit
          </button>
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
import Stepsvue from './Steps.vue'
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
    const recipeForm = ref({})
    return {
      stepForm,
      ingredientForm,
      recipeForm,
      user: computed(() => AppState.user),
      accountId: computed(() => AppState.account.id),
      recipeId: computed(() => AppState.openRecipe.creatorId),
      selectedStep: computed(() => AppState.selectedStep),

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

      async openStepEditModal() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        Modal.getOrCreateInstance(document.getElementById(`edit-step-modal`)).show()
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
      editRecipeModal() {
        Modal.getOrCreateInstance(document.getElementById('edit-recipe-form')).show();
      },

      async editRecipe() {
        try {
          await recipesService.editRecipe(recipeForm.value)
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
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