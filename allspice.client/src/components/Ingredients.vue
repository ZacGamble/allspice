<template>
  <li>
    {{ ingredients?.quantity }} of {{ ingredients?.name }}
    <i
      v-show="account.id == ingredients?.creatorId"
      class="mdi mdi-delete text-danger on-hover"
      title="delete ingredient"
      @click="deleteIngredient()"
    ></i>
  </li>
</template>


<script>
import { computed } from '@vue/reactivity'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { AppState } from '../AppState'
import { recipesService } from '../services/RecipesService'
export default {
  props: {
    ingredients: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    return {
      account: computed(() => AppState.account),

      async deleteIngredient() {
        try {
          AppState.selectedIngredient = props.ingredients
          logger.log(AppState.selectedIngredient)
          if (await Pop.confirm()) {

            await recipesService.deleteIngredient()
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
    }
  }
}
</script>


<style lang="scss" scoped>
</style>