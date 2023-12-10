using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Interfaces;
using SaminExamination.Models;
using System.Threading;

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
        public async Task<IActionResult> CrateProduct([FromQuery] int categoryId, [FromBody] ProductDto getProduct , CancellationToken cancellationToken)
        {
            if (getProduct == null)
            {
                return BadRequest(" .لطفا پارامتر های ورودی را به درستی پر نمایید");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("فرمت ورودی نا معتبر است");
            }
            if (categoryId == null)
                return BadRequest("لطفا شAutoMapper.AutoMapperMappingException: 'Missing type map configuration or unsupported mapping.'ناسه گروه کالا را وارد نمایید");
            var category =await _categoryRepository.GetCategory(categoryId);
            if (category == null)
                return NotFound("گروه کالایی با این شناس یافت نشد");
            var product = await _productRepository.GetProducts(cancellationToken);
            var fileterProduct = product.Where(p => p.Name.Trim().ToUpper() == getProduct.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (fileterProduct != null)
            {
                ModelState.AddModelError("", "این کالا از قبل موجود میباشد");
                 return StatusCode(422, ModelState);
            }
                
            var productMap = _mapper.Map<Product>(getProduct);

            productMap.Category= await _categoryRepository.GetCategory(categoryId);

            if(!await _productRepository.AddProducts(productMap , cancellationToken))
            {
                ModelState.AddModelError("", "سرور با مشکل مواجه شده است");
                return StatusCode(500, ModelState);
            }
            return Ok("کالا با موفقیت اضافه شد");


        }
       

        [HttpGet("getProducts")]
        [ProducesResponseType(200)]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetProducts(GetProductDto product,CancellationToken cancellationToken)
        {
            var Products = _mapper.Map<ICollection<ProductDto>>(_productRepository.GetProducts(product,cancellationToken));

            if (Products == null)
                return NotFound("کالایی یافت نشد");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Products);
        }

        [HttpGet("getProduct/{productId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult>GetProductById(int productId , CancellationToken cancellationToken)
        {
            if (!await _productRepository.ProductIsExist(productId))
                return NotFound("کالایی با این شناسه یافت نشد");
            var Product = _mapper.Map<ProductDto>(await _productRepository.GetProductById(productId, cancellationToken));
            return Ok(Product);
        }
        [HttpDelete("admin/DeleteProduct/{productId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteProduct(int productId , CancellationToken cancellationToken)
        {
            if (!await _productRepository.ProductIsExist(productId))
                return NotFound("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
            {
                return BadRequest( "فرمت ورودی نا معتبر میباشد");
            }
              

            var productToDelete =await _productRepository.GetProductById(productId , cancellationToken);
            if (!await _productRepository.DeleteProducts(productToDelete, cancellationToken))
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
        public async Task<IActionResult> updateProducts(int productId , [FromBody] ProductUpdateDto updateProducrt , CancellationToken cancellationToken)
        {
            if (updateProducrt == null)
                return BadRequest("پارامتر ورودی نا معتبر میباشد");
            if (!await _productRepository.ProductIsExist(productId))
                return BadRequest("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
            {
                return BadRequest("فرمت پارامتر ورودی نا معتبر میباشد");
            }
            if(productId != updateProducrt.Id)
            {
                return BadRequest("ایدی تطابق  ندارد");
            }
            var myCategory =await _productRepository.GetCategoryByProductId(productId, cancellationToken);
            var ProductMap = _mapper.Map<Product>(updateProducrt);
            ProductMap.Category = myCategory;
            if (!await _productRepository.UpdateProducts(ProductMap , cancellationToken))
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
        public async Task<IActionResult> GetCategoryByProductId(int productId, CancellationToken cancellationToken)
        {
            if (!await _productRepository.ProductIsExist(productId))
                return NotFound("محصولی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest("فرمت ورودی نا معتبر میباشد");
            var Category = _mapper.Map<CategoryDto>(await _productRepository.GetCategoryByProductId(productId, cancellationToken));
            return Ok(Category);
        }

        [HttpGet("GetProductsListByCategoryId/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetProductsListByCategoryId(int categoryId , CancellationToken cancellationToken)
        {
            if (!await _categoryRepository.CategoryExist(categoryId))
                return NotFound("گروه کلایی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest("فرمت ورودی نا معتبر میباشد");
            var products = _mapper.Map<ICollection<ProductDto>>( await _productRepository.GetProductsListByCategoryId(categoryId, cancellationToken));
            return Ok(products);
        }

    }
}
