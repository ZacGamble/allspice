<template>
  <li>{{ ingredients?.quantity }} of {{ ingredients?.name }}</li>
</template>


<script>
export default {
  props: {
    ingredients: {
      type: Object,
      required: true
    }
  },

  setup() {
    return {}
  }
}
</script>


<style lang="scss" scoped>
</style>