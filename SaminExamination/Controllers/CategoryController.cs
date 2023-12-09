using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaminExamination.Dto;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[Controller]")]
    public class CategoryController:Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper , ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpPost("admin/AddCategory")]
        [ProducesResponseType(204)]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCraeted)
         {
            if (categoryCraeted == null)
                return BadRequest("لطفا پارامتر های ورودی را پر نمایید!");
            var category = _categoryRepository.GetCategories()
               .Where(c => c.CategoryName.Trim().ToUpper() == categoryCraeted.CategoryName.TrimEnd().ToUpper())
               .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "دسته انتخابی شما از قبل موجودمیباشد.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest("پارامتر های ورودی معتبر نمیباشند");

            var categoryMap = new Category()
            {
                CategoryName = categoryCraeted.CategoryName
            };

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "مشکلی در سرویس پیش امده است");
                return StatusCode(500, ModelState);
            }

            return Ok("عملیات با موفقیت انجام شد");
        }

        [HttpGet("getCategories")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin,User")]
      
        public IActionResult GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Categories = _mapper.Map<ICollection<CategoryDto>>(_categoryRepository.GetCategories());
            return Ok(Categories);
        }
        [HttpGet("admin/DeleteCategory/{categoryId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("فرمت ورودی نا معتبر میباشد");
            }
            if (!_categoryRepository.IsExist(categoryId))
                return NotFound("گروه کالا با این شناسه یافت نشد");

            var category = _categoryRepository.GetCategory(categoryId);
            _categoryRepository.DeleteCategory(category);
            return Ok("عملیات با موفقیت انجام شد");
        }
        [HttpGet("GetCategoryById/{categoryId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCategoryById(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound("گروه کالایی با این شناسه یافت نشد");
            if(!ModelState.IsValid)
                return BadRequest("فرمت  ورودی  معتبر نمیباشد");
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            return Ok(category);
        }
        [HttpPut("admin/update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCategory (int CategoryId, [FromBody] CategoryUpdateDto updateProducrt)
        {
            if (!_categoryRepository.IsExist(CategoryId))
                return NotFound("گروه کالایی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest("فرمت  ورودی  معتبر نمیباشد");
            var categoryUpdated = _mapper.Map<Category>(updateProducrt);
            _categoryRepository.UpdateCategory(categoryUpdated);
            return Ok("عملیات با موفقیت انجام شد");
        }

    }
}

