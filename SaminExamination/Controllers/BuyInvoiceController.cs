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
    [Route("/api/admin/[Controller]")]
    [Authorize]
    public class BuyInvoiceController : Controller
    {


        public IMapper _mapper;
        public IBuyInvoicesRepository _buyInvoicesRepository;
        public IProductRepository _productRepository;
        public BuyInvoiceController(IMapper mapper, IBuyInvoicesRepository buyInvoicesRepository , IProductRepository productRepository )
        {
            _mapper = mapper;
            _buyInvoicesRepository = buyInvoicesRepository;
            _productRepository = productRepository;
        }
        [HttpPost("AddedBuyInvoices")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public IActionResult AddedBuyInvoices([FromQuery] int productId, [FromBody] BuyInvoiceDto buyInvoice)
        {
            if (!_productRepository.ProductIsExist(productId))
                return NotFound("کالایی با این شناسه یافت نشد");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _buyInvoicesRepository.BuyInvoices(productId, buyInvoice);
            return Ok("عملیات با موفقیت انجام شد" +
                "");
        }

        [HttpPost("GetAllBuyInvoices")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBuyInvoices()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_buyInvoicesRepository.GetBuyInvoices());
        }
        [HttpGet("GetBuyInvoice/{invoiceId}")]
        [ProducesResponseType(200, Type = typeof(BuyInvoice))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBuyInvoice(int invoiceId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!_buyInvoicesRepository.IsExist(invoiceId))
                return BadRequest("فاکتور خریدی با این شناسه یافت نشد");
            return Ok(_buyInvoicesRepository.GetBuyInvoicesById(invoiceId));
        }
    }
}
