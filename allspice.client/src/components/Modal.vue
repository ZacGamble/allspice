<template>
  <div
    class="modal fade"
    id="Modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="ModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <slot name="modal-header-slot"> </slot>
        </div>
        <div class="modal-body">
          <slot name="modal-body-slot"> </slot>
        </div>
        <div class="modal-footer">
          <slot name="modal-footer-slot"></slot>
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

    }
  }
}
</script>


<style lang="scss" scoped>
</style>