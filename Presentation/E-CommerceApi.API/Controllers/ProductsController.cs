using Azure.Core;
using E_CommerceApi.Application.Abstractions;
using E_CommerceApi.Application.Abstractions.Storage;
using E_CommerceApi.Application.Features.Commands.Product.CreateProduct;
using E_CommerceApi.Application.Features.Commands.Product.RemoveProduct;
using E_CommerceApi.Application.Features.Commands.Product.UpdateProduct;
using E_CommerceApi.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage;
using E_CommerceApi.Application.Features.Commands.ProductImageFile.RemoveProductImageFile;
using E_CommerceApi.Application.Features.Commands.ProductImageFile.UpdateProductImageFile;
using E_CommerceApi.Application.Features.Queries.Product.GetAllProduct;
using E_CommerceApi.Application.Features.Queries.Product.GetByIdProduct;
using E_CommerceApi.Application.Features.Queries.ProductImageFile.GetProductImages;
using E_CommerceApi.Application.Repositories;
using E_CommerceApi.Application.RequestParameters;
using E_CommerceApi.Application.ViewModels.Products;
using E_CommerceApi.Domain.Entities;
using E_CommerceApi.Domain.Entities.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace E_CommerceApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes="Admin")]
    public class ProductsController : ControllerBase
    {
     

        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;


        //
        readonly IMediator _mediator;

        public ProductsController(IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileWriteRepository fileWriteRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,
            IMediator mediator,
            IProductImageFileReadRepository productImageFileReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
            _productImageFileReadRepository = productImageFileReadRepository;
        }

        

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
           GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
          
        }
        
        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
          GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [HttpPost("[Action]")]
        //id queryString
        public async Task<IActionResult> Upload([FromQuery]UpdateProductImageFileCommandRequest updateProductImageFileCommandRequest)
        {           
            updateProductImageFileCommandRequest.Files = Request.Form.Files;
            await _mediator.Send(updateProductImageFileCommandRequest);

            return Ok() ;         
        }


        [HttpPost("[Action]/{id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[Action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]RemoveProductImageFileCommandRequest
            removeProductImageFileCommandRequest, [FromQuery] string imageId)
        {
            removeProductImageFileCommandRequest.imageId = imageId;
            RemoveProductImageFileCommandResponse response = await _mediator.Send(removeProductImageFileCommandRequest);

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeShowCaseImage([FromQuery]ChangeShowCaseImageCommandRequest changeShowCaseImageCommandRequest)
        {
           ChangeShowCaseImageCommandResponse response = await _mediator.Send(changeShowCaseImageCommandRequest);
            return Ok(response);
        }
    }
}
