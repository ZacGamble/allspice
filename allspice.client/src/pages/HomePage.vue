<template>
  <Navbar />
  <div class="container-fluid">
    <div class="row bg-img">
      <h1 class="my-5 p-3 text-center">ALLSPICE DESTROY</h1>
      <div class="text-center">
        Cherish all of your family, and their cooking.
      </div>
    </div>
    <div class="row">
      <div class="col-12">
        <div class="d-flex justify-content-center">
          <div class="shadow rounded bg-gray p-3 fw-bolder position">
            <span class="mx-3 selectable">My Recipes</span>
            <span class="mx-3 selectable">Favorites</span>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-4">
      <Recipe v-for="ar in activeRecipes" :key="ar.id" :recipe="ar" />
    </div>
  </div>
</template>

<script>
import { computed, onMounted } from '@vue/runtime-core'
import { logger } from '../utils/Logger'
import { recipesService } from '../services/RecipesService.js'
import Pop from '../utils/Pop'
import { AppState } from '../AppState'
export default {
  name: 'Home',
  setup() {
    onMounted(async () => {
      try {
        await recipesService.getRecipes();
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      activeRecipes: computed(() => AppState.activeRecipes)
    }
  }
}
</script>

<style scoped lang="scss">
.position {
  transform: translateY(1rem);
}
.bg-img {
  background-image: url(https://images.unsplash.com/photo-1509358271058-acd22cc93898?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8OHx8c3BpY2UlMjByYWNrfGVufDB8fDB8fA%3D%3D&auto=format&fit=crop&w=600&q=60);
  background-size: cover;
  background-position-y: calc(-100px);
  color: white;
  text-shadow: 2px 2px 2px rgb(125, 3, 3);
}
</style>
