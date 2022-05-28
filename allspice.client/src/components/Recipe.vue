<template>
  <div class="col-md-4 p-2">
    <div
      class="custom-card selectable"
      :title="'click to see details of ' + recipe.title"
    >
      <div class="p-2">
        <img
          class="img-fluid rounded"
          :src="recipe.picture"
          alt="photo of the recipe submission"
        />
      </div>
      <div class="p-1">
        <h2>
          {{ recipe.title }}
        </h2>
        <h5>{{ recipe.subtitle }}</h5>
      </div>
    </div>
  </div>
</template>


<script>
export default {
  props: {
    recipe: {
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
.custom-card {
  border-radius: 5%;
  border: 2px double black;
  box-shadow: 4px 3px 7px 3px rgb(24, 23, 23);
}
.custom-card:hover {
  transform: scale(1.02);
}
</style>