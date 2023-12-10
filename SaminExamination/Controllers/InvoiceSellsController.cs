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
        public async Task<IActionResult> GetAllInvoiceSells(CancellationToken cancellation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await _invoiceSellRepository.GetSellInvoices(cancellation));
        }
        [HttpGet("GetAllInvoiceSellById/{invoiceId}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>  GetInvoiceSellById(int invoiceId , CancellationToken cancellationToken) {
            if(!await _invoiceSellRepository.IsExist(invoiceId))
                return BadRequest("فاکتوری با این شناسه یافت نشد");
        var invoice=await _invoiceSellRepository.GetSellInvoceById(invoiceId , cancellationToken);
            return Ok(invoice);
        }
        [HttpPost("AddedInvoicesSells")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>  AddSellsInvoice([FromQuery] int productId, [FromBody] InvoiceSellDto invoiceSell , CancellationToken cancellationToken)
        {
            if (!await _invoiceSellRepository.EnoughProduct(productId, invoiceSell))
                return BadRequest("میزان سفارش شما بیشتر از موجودی انبار میباشد");
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
           await _invoiceSellRepository.NewSellInvoices(productId, invoiceSell , cancellationToken);
            return Ok("عملیات با موفقیت انجام شد");
        }
    }
}
