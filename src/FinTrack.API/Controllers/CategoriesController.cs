using System.Security.Claims;
using FinTrack.API.Models;
using FinTrack.Application.DTOs.Categories;
using FinTrack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinTrack.API.Helpers;
namespace FinTrack.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController:ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService=categoryService;
        }

        [HttpPost]
        public async Task<IActionResult>Createcategory(CreateCategoryRequest request)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }
            var result=await _categoryService.CreateCategory(request,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> Getcategory(Guid categoryId)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

            var result = await _categoryService.GetCategory(
                categoryId,
                userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(
                    result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Getcategorys()
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }

            var result = await _categoryService.GetCategories(userId);

            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(
                    result.Error!.Code);

                return StatusCode(statusCode, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult>Updatecategory(Guid categoryId, UpdateCategoryRequest request)
        {
            if (!CurrentUserHelper.TryGetUserId(User, out var userId))
            {
                return Unauthorized();
            }
            var result=await _categoryService.UpdateCategory(categoryId, request,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return Ok(result.Value);
        }
        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult>Deactivatetecategory(Guid categoryId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim!.Value);
            var result=await _categoryService.DeactivateCategory(categoryId,userId);
            if (!result.IsSuccess)
            {
                var statusCode = ErrorMapping.GetStatusCode(result.Error!.Code);
                return StatusCode(statusCode, result.Error);
            }
            return NoContent();
        }

    }
}