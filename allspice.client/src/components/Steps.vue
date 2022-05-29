<template>
  <p class="stepStyle">{{ steps?.position }} - {{ steps?.body }}</p>
</template>


<script>
export default {
  props: {
    steps: {
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
.stepStyle {
  padding: 5px;
  margin: 0px;
}
</style>