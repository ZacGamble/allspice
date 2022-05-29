<template>
  <p class="stepStyle">
    {{ steps?.position }} - {{ steps?.body }}
    <i
      v-show="user.id == steps.creatorId"
      class="mdi mdi-delete text-danger"
    ></i>
  </p>
</template>


<script>
import { computed } from '@vue/reactivity'
import { AppState } from '../AppState'
export default {
  props: {
    steps: {
      type: Object,
      required: true
    }
  },
  setup() {
    return {
      user: computed(() => AppState.user)
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