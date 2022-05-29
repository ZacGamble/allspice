<template>
  <div
    class="modal fade"
    id="Modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="ModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <slot name="modal-header-slot"> </slot>
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
        </div>
        <div class="modal-body">
          <slot name="modal-body-slot"> </slot>
        </div>
        <div class="modal-footer">
          <button
            @click="closeModal()"
            type="button"
            class="btn btn-secondary"
            data-dismiss="open-recipe-modal"
          >
            Close
          </button>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed } from '@vue/reactivity'
import { recipesService } from '../services/RecipesService'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { AppState } from '../AppState'
import { Modal } from 'bootstrap'
export default {
  setup() {
    return {
      async deleteRecipe(theRecipe) {
        try {
          if (await Pop.confirm()) {
            await recipesService.deleteRecipe(AppState.openRecipe.id);
            Pop.toast("Your recipe was deleted.", 'success');
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      closeModal() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        return
      }
    }
  }
}
</script>


<style lang="scss" scoped>
</style>