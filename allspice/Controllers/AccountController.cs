using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using allspice.Models;
using allspice.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace allspice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly FavoritesService _favoritesService;

        public AccountController(AccountService accountService, FavoritesService favoritesService)
        {
            _accountService = accountService;
            _favoritesService = favoritesService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Account>> Get()
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                return Ok(_accountService.GetOrCreateProfile(userInfo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("/accounts/favorites")]
        // [Authorize]
        public async Task<ActionResult<List<Favorite>>> GetFavorites()
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                List<Favorite> favorites = _favoritesService.GetFavorites();
                return Ok(favorites);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("/accounts/favorites")]
        [Authorize]
        public async Task<ActionResult<Favorite>> Create([FromBody] Favorite favoriteData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                favoriteData.AccountId = userInfo.Id;
                Favorite favorite = _favoritesService.Create(favoriteData);
                return Ok(favorite);


            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }


}