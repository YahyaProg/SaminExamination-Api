using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaminExamination.Dto;
using SaminExamination.Interfaces;

namespace SaminExamination.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/admin/[Controller]")]
    public class InvoiceSellsController:Controller
    {
        public IInvoiceSellRepository _invoiceSellRepository { get; set; }
        public IMapper _mapper { get; set; }
        public IProductRepository _productRepository { get; set; }

        public InvoiceSellsController(IInvoiceSellRepository invoiceSellRepository , IProductRepository productRepository , IMapper mapper)
        {
            _mapper = mapper;
            _invoiceSellRepository = invoiceSellRepository;
            _productRepository = productRepository;
        }

        [HttpGet("GetAllInvoiceSells")]
        [ProducesResponseType(200)]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAllInvoiceSells()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_invoiceSellRepository.GetSellInvoices());
        }
        [HttpGet("GetAllInvoiceSellById/{invoiceId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetInvoiceSellById(int invoiceId) {
            if(!_invoiceSellRepository.IsExist(invoiceId))
                return BadRequest("فاکتوری با این شناسه یافت نشد");
        var invoice= _invoiceSellRepository.GetSellInvoceById(invoiceId);
            return Ok(invoice);
        }
        [HttpPost("AddedInvoicesSells")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSellsInvoice([FromQuery] int productId, [FromBody] InvoiceSellDto invoiceSell)
        {
            if (!_invoiceSellRepository.EnoughProduct(productId, invoiceSell))
                return BadRequest("میزان سفارش شما بیشتر از موجودی انبار میباشد");
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            _invoiceSellRepository.NewSellInvoices(productId, invoiceSell);
            return Ok("عملیات با موفقیت انجام شد");
        }
    }
}
