using Admin.Controllers;
using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.ProductDto;
using DataAccess.Model;
using DataAccess.Profiles.Category;
using DataAccess.Profiles.Product;
using DataAccess.Profiles.ProductImg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class ProductControllerTests
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductControllerTests()
        {
            _db = new ApplicationDbContext(dbContextOptions);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductCreateDtoProfile());
                mc.AddProfile(new ProductReadDtoProfile());
                mc.AddProfile(new ProductUpdateDtoProfile());
                mc.AddProfile(new ProductImgCreateDtoProfile());
                mc.AddProfile(new ProductImgReadDtoProfile());
                mc.AddProfile(new CategoryCreateDtoProfile());
                mc.AddProfile(new CategoryReadDtoProfile());
                mc.AddProfile(new CategoryUpdateDtoProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            SeedDatabase();
        }

        [Fact]
        public async Task GetProducts_ReturnResult()
        {
            //Arrange
            int page = 1;
            var controller = new ProductController(_db, _mapper);

            //Act
            var result = await controller.GetProducts(page);

            //Assert
            var responseTest = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductResponseDto>(responseTest.Value);
            Assert.Equal(1, returnValue.Pages);
            Assert.Equal(1, returnValue.CurrentPage);
            Assert.Equal(2, returnValue.Products.Count());
        }

        [Fact]
        public async Task CreateProduct_WithModelStateError()
        {
            // Arrage
            var controller = new ProductController(_db, _mapper);
            controller.ModelState.AddModelError("error", "some error.");

            // Act
            var result = await controller.CreateProduct(It.IsAny<ProductCreateDto>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var modelState = controller.ModelState;

            Assert.Equal(1, (int)modelState.Count);

            Assert.True(modelState.ContainsKey("error"));
            Assert.True(modelState["error"].Errors.Count == 1);
            Assert.Equal("some error.", modelState["error"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task CreateProduct_WithValidRequest()
        {
            //Arrange
            var product = new ProductCreateDto
            {
                ProductName = "string",
                ProductDescription = "string",
                Price = 0,
                ProductQuantity = 0,
                CreateDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                TotalRating = 0,
                CategoryId = Guid.Parse("ab7e840f-d481-41a2-a579-773afb2e6d36")
            };

            var controller = new ProductController(_db, _mapper);

            //Act
            var result = await controller.CreateProduct(product);

            //Assert
            var responseTest = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Product>(responseTest.Value);
            Assert.Equal(product.ProductName, returnValue.ProductName);
            Assert.Equal(product.ProductDescription, returnValue.ProductDescription);
            Assert.Equal(product.Price, returnValue.Price);
            Assert.Equal(product.ProductQuantity, returnValue.ProductQuantity);
            Assert.Equal(product.CreateDate, returnValue.CreateDate);
            Assert.Equal(product.LastModifyDate, returnValue.LastModifyDate);
            Assert.Equal(product.TotalRating, returnValue.TotalRating);
            Assert.Equal(product.CategoryId, returnValue.CategoryId);
        }

        private void SeedDatabase()
        {
            var categories = new List<Category>
            {
                new Category()
                {
                    CategoryId = Guid.Parse("ab7e840f-d481-41a2-a579-773afb2e6d36"),
                    CategoryName = "Test Category for unit test",
                    CategoryDescription = "abcd"
                }
            };
            _db.Categories.AddRange(categories);

            _db.Products.AddRange(new List<Product>(){
                new Product()
                {
                    ProductId = Guid.Parse("418f2b32-5cc9-43a9-bc63-b2ed2c1317c6"),
                    ProductName = "Test Product 1",
                    ProductDescription = "abcd",
                    Price = 10,
                    ProductQuantity =0,
                    CreateDate = DateTime.Now,
                    LastModifyDate = DateTime.Now,
                    TotalRating = 0,
                    CategoryId = categories[0].CategoryId
                },
                new Product()
                {
                    ProductId = Guid.Parse("7797add9-97d2-4d90-9427-81f20c851703"),
                    ProductName = "Test Product 1",
                    ProductDescription = "abcd",
                    Price = 10,
                    ProductQuantity =0,
                    CreateDate = DateTime.Now,
                    LastModifyDate = DateTime.Now,
                    TotalRating = 0,
                    CategoryId = categories[0].CategoryId
                },
            });

            _db.SaveChanges();
        }

        ~ProductControllerTests()
        {
            _db.Dispose();
        }
    }
}