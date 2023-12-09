using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class ProductController:Controller
    {
        
  
        public IMapper _mapper;
        public ICategoryRepository _categoryRepository;
        public IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository , IMapper mapper , ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("admin/AddProduct")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles ="Admin")]
        public IActionResult CrateProduct([FromQuery] int categoryId, [FromBody] ProductDto getProduct)
        {
            if (getProduct == null)
            {
                return BadRequest(" .لطفا پارامتر های ورودی را به درستی پر نمایید");
            }
            if (categoryId == null)
                return BadRequest("لطفا شAutoMapper.AutoMapperMappingException: 'Missing type map configuration or unsupported mapping.'ناسه گروه کالا را وارد نمایید");
            var category = _categoryRepository.GetCategory(categoryId);
            if (category == null)
                return NotFound("گروه کالایی با این شناس یافت نشد");
            var product = _productRepository.GetProducts().Where(p => p.Name.Trim().ToUpper() == getProduct.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (product != null)
            {
                ModelState.AddModelError("", "این کالا از قبل موجود میباشد");
                 return StatusCode(422, ModelState);
            }
                
            var productMap = _mapper.Map<Product>(getProduct);

            productMap.Category=_categoryRepository.GetCategory(categoryId);

            if(!_productRepository.AddProducts(productMap))
            {
                ModelState.AddModelError("", "سرور با مشکل مواجه شده است");
                return StatusCode(500, ModelState);
            }
            return Ok("کالا با موفقیت اضافه شد");


        }
       

        [HttpGet("getProducts")]
        [ProducesResponseType(200)]
        [Authorize(Roles ="Admin,User")]
        public IActionResult GetProducts()
        {
            var Products = _mapper.Map<ICollection<ProductDto>>(_productRepository.GetProducts());
            if (Products == null)
                return NotFound("کالایی یافت نشد");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Products);
        }

        [HttpGet("getProduct/{productId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetProductById(int productId)
        {
            if (!_productRepository.ProductIsExist(productId))
                return NotFound("کالایی با این شناسه یافت نشد");
            var Product = _mapper.Map<ProductDto>(_productRepository.GetProductById(productId));
            return Ok(Product);
        }
        [HttpDelete("admin/DeleteProduct/{productId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]
        public IActionResult deleteProduct(int productId)
        {
            if (!_productRepository.ProductIsExist(productId))
                return NotFound("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
            {
                return BadRequest( "فرمت ورودی نا معتبر میباشد");
            }
              

            var productToDelete = _productRepository.GetProductById(productId);
            if (! _productRepository.DeleteProducts(productToDelete))
            {
                return BadRequest("خطایی از سمت سرویس رخ داده است لطفا مجدد تلاش کنید");

            }

            return Ok("عملیات با موفقیت انجام شد");
            
        }
        [HttpPut("/admin/update/{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public IActionResult updateProducts(int productId , [FromBody] ProductUpdateDto updateProducrt)
        {
            if (updateProducrt == null)
                return BadRequest("پارامتر ورودی نا معتبر میباشد");
            if (!_productRepository.ProductIsExist(productId))
                return BadRequest("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
            {
                return BadRequest("فرمت پارامتر ورودی نا معتبر میباشد");
            }
            if(productId != updateProducrt.Id)
            {
                return BadRequest("ایدی تطابق  ندارد");
            }
            var myCategory = _productRepository.GetCategoryByProductId(productId);
            var ProductMap = _mapper.Map<Product>(updateProducrt);
            ProductMap.Category = myCategory;
            if (!_productRepository.UpdateProducts(ProductMap))
            {
                ModelState.AddModelError("", "مشکلی ازسمت سرویس رخ داده است لطفا مجدد تلاش نمایید");
                return StatusCode(500, ModelState);
            }
            return Ok("عملیات با موفقیت انجام شد");
        }
        [HttpGet("GetCategoryByProductId/{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCategoryByProductId(int productId)
        {
            if (!_productRepository.ProductIsExist(productId))
                return NotFound("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest("فرمت ورودی نا معتبر میباشد");
            var Category = _mapper.Map<CategoryDto>(_productRepository.GetCategoryByProductId(productId));
            return Ok(Category);
        }

        [HttpGet("GetProductsListByCategoryId/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetProductsListByCategoryId(int categoryId)
        {
            if (! _categoryRepository.CategoryExist(categoryId))
                return NotFound("گروه کلایی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest("فرمت ورودی نا معتبر میباشد");
            var products = _mapper.Map<ICollection<ProductDto>>(_productRepository.GetProductsListByCategoryId(categoryId));
            return Ok(products);
        }

    }
}
