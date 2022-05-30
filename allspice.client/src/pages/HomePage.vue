<template>
  <Navbar />
  <div class="container-fluid">
    <div class="row bg-img">
      <div class="col-md-12 text-color">
        <div class="d-flex justify-content-between">
          <div>
            <button @click="openRecipeForm()" class="btn btn-warning mt-3">
              Submit Recipe
            </button>
          </div>
          <form class="m-2 p-2" @submit.prevent="filterByCategory(category)">
            <div>
              <label for="categories" class="mx-2">Filter by category!</label>
            </div>
            <div class="d-flex flex-column">
              <select
                name="categories"
                id="categorySelect"
                required
                v-model="selectFilter.category"
              >
                <option value="breakfast">Breakfast</option>
                <option value="lunch">Lunch</option>
                <option value="dinner">Dinner</option>
                <option value="dessert" selected>Dessert</option>
              </select>
              <button class="btn btn-info">Go!</button>
            </div>
          </form>
        </div>
        <h1 class="my-5 p-3 text-center fw-bolder">ALLSPICE</h1>
        <div class="text-center mb-5">
          <h4>
            Create a recipe, then edit your instruction steps and ingredients.
          </h4>
          Share your love of cooking with the world!
        </div>
        <div class="d-flex justify-content-center">
          <div class="shadow rounded p-1 fw-bolder fs-5 position d-flex">
            <div
              class="mx-3 selectable pop"
              title="filter by type"
              @click="filterByNone()"
            >
              Home
            </div>
            <div
              class="mx-3 selectable pop"
              title="filter by type"
              @click="filterByOwned()"
            >
              My Recipes
            </div>
            <div
              class="mx-3 selectable pop"
              title="filter by type"
              @click="filterByFavorite()"
            >
              Favorites
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-4">
      <Recipe v-for="ar in activeRecipes" :key="ar.id" :recipe="ar" />
    </div>
  </div>
  <Modal id="recipe-form">
    <template #modal-header-slot>
      <span class="fs-2 fw-bold">Create a recipe!</span>
    </template>
    <template #modal-body-slot>
      <form @submit.prevent="handleSubmit()">
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
</template>

<script>
import { computed, onMounted, ref, watch, watchEffect } from '@vue/runtime-core'
import { logger } from '../utils/Logger'
import { recipesService } from '../services/RecipesService.js'
import Pop from '../utils/Pop'
import { AppState } from '../AppState'
import { Modal } from 'bootstrap'
export default {
  setup() {

    const recipeForm = ref({});
    const selectFilter = ref({})

    watchEffect(async () => {
      try {
        await recipesService.getRecipes();
        await recipesService.getFavorites();
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }

    })
    return {
      recipeForm,
      selectFilter,
      activeRecipes: computed(() => AppState.activeRecipes),

      async handleSubmit() {
        try {
          await recipesService.createRecipe(recipeForm.value);
          Modal.getOrCreateInstance(document.getElementById('recipe-form')).hide()
          Pop.toast("Recipe posted!", 'success')
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async filterByCategory() {
        const category = selectFilter.value
        try {
          await recipesService.getRecipesByCategory(category)
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }

      },

      async filterByNone() {
        try {
          await recipesService.getRecipes()
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async filterByOwned() {
        try {
          await recipesService.getRecipesIOwn()
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async filterByFavorite() {
        try {
          await recipesService.filterByFavorite()
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      openRecipeForm() {
        Modal.getOrCreateInstance(document.getElementById('recipe-form')).show()
      },

      closeModal() {
        Modal.getOrCreateInstance(document.getElementById('recipe-form')).hide()
        return
      }
    }
  }
}
</script>

<style scoped lang="scss">
.position {
  transform: translateY(1rem);
  z-index: 2;
}
.bg-img {
  background-image: url(https://images.unsplash.com/photo-1509358271058-acd22cc93898?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8OHx8c3BpY2UlMjByYWNrfGVufDB8fDB8fA%3D%3D&auto=format&fit=crop&w=600&q=60);
  background-size: cover;
  background-position-y: calc(0px);
  z-index: 1;
}
.text-color {
  color: white;
  text-shadow: 2px 2px 2px rgb(125, 3, 3);
}
.pop:hover {
  transform: scale(1.05);
}
</style>
