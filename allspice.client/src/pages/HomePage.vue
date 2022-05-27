<template>
  <Navbar />
  <div class="container">
    <div class="row">
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
    <div class="row">
      <!-- This will become the recipe component -->
      <div class="col-md-4">
        <div class="shadow rounded p-3 m-3">The recipes themselves</div>
      </div>
      <!-- end component -->
    </div>
  </div>
</template>

<script>
export default {
  name: 'Home'
}
</script>

<style scoped lang="scss">
.position {
  transform: translateY(1rem);
}
</style>
