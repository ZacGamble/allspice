<template>
  <p class="stepStyle action">
    {{ steps?.position }} - {{ steps?.body }}
    <i
      v-show="account.id == steps.creatorId"
      class="mdi mdi-delete text-danger on-hover"
      title="delete step"
      @click="deleteStep()"
    ></i>
  </p>
  <!-- Edit steps -->
  <Modal id="edit-step">
    <template #modal-header-slot>Change your instructions here</template>
    <template #modal-body-slot>
      <form @submit.prevent="handleStepEdit()">
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
          />
          <button class="btn success" type="submit">Submit</button>
        </div>
      </form>
    </template>
  </Modal>
</template>

<script>
import { computed, ref } from '@vue/reactivity'
import { AppState } from '../AppState'
import { Modal } from 'bootstrap'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { recipesService } from '../services/RecipesService'
export default {
  props: {
    steps: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const stepForm = ref({})

    return {
      stepForm,
      user: computed(() => AppState.user),
      account: computed(() => AppState.account),

      openEdit() {
        Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).hide()
        Modal.getOrCreateInstance(document.getElementById('edit-step')).show()
      },
      async handleStepEdit() {
        try {
          AppState.selectedStep = props.steps;
          logger.log(AppState.selectedStep)
          await recipesService.editStep(stepForm.value)
          await Modal.getOrCreateInstance(document.getElementById('edit-step')).hide()
          Modal.getOrCreateInstance(document.getElementById('open-recipe-modal')).show()
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async deleteStep() {
        try {
          AppState.selectedStep = props.steps
          logger.log(AppState.selectedStep)
          await recipesService.deleteStep()
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
.stepStyle {
  padding: 5px;
  margin: 0px;
}
</style>