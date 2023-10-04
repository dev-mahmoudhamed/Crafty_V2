using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {

        private readonly IGenericRepository<Product> _ProductsRepo;
        private readonly IGenericRepository<ProductBrand> _ProductBrandsRepo;
        private readonly IGenericRepository<ProductType> _ProductTypesRepo;
        private readonly IMapper _mapper;
        public ProductController(
            IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> productBrandsRepo,
            IGenericRepository<ProductType> productTypesRepo,
            IMapper mapper)
        {
            _ProductsRepo = productsRepo;
            _ProductBrandsRepo = productBrandsRepo;
            _ProductTypesRepo = productTypesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var spec = new ProductWithTypesAndBrandsSpecification();

            var products = await _ProductsRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _ProductsRepo.GetEntityWithSpec(spec);
            if (product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            return _mapper.Map<Product, ProductDto>(product);
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _ProductBrandsRepo.GetAllAsync());
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _ProductTypesRepo.GetAllAsync());
        }
    }
}