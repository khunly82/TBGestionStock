using GestionStock.API.Data;
using GestionStock.API.Services;
using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GestionStock.UnitTests.Services
{
    public class ProductServiceTests
    {

        private List<Product> products = [
            new Product { Id = 1, Name = "Coca cola 33 cl", Reference = "COCA0001", Stock = 50 },
            new Product { Id = 1, Name = "Coca cola 50 cl", Reference = "COCA0002", Stock = 50 },
        ];

        private List<Category> categories = [
            new Category { Id =1, Name = "Biere" },
            new Category { Id =2, Name = "Vin" },
            new Category { Id =3, Name = "Alcool" },
            new Category { Id =4, Name = "Soft" },
        ];

        private ProductService productService;

        public ProductServiceTests()
        {
            // ARRANGE
            DbContextOptions options = new DbContextOptionsBuilder().Options;
            Mock<StockContext> dbMock = new Mock<StockContext>(options);


            var inMemorySettings = new Dictionary<string, string?>
            {
                { "ConnectionStrings:BlobStorage", "removed for security" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            productService = new(dbMock.Object, configuration);

            Mock<DbSet<Product>> dbSetMockProduct = new(); // mock du DbSet<Product>
            Mock<DbSet<Category>> dbSetMockCategory = new(); // mock du DbSet<Categorie>

            dbSetMockProduct.As<IQueryable<Product>>()
                .Setup(m => m.Provider).Returns(products.AsQueryable().Provider);
            dbSetMockProduct.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
            dbSetMockProduct.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
            dbSetMockProduct.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            dbSetMockCategory.As<IQueryable<Category>>()
                .Setup(m => m.Provider).Returns(categories.AsQueryable().Provider);
            dbSetMockCategory.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.AsQueryable().Expression);
            dbSetMockCategory.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.AsQueryable().ElementType);
            dbSetMockCategory.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            dbMock.Setup(c => c.Products).Returns(dbSetMockProduct.Object);
            dbMock.Setup(c => c.Categories).Returns(dbSetMockCategory.Object);
            dbMock.Setup(c => c.SaveChanges());
        }

        [Fact]
        public async void AddProductAndCheckReference()
        {
            // ACT
            Product p1 = await productService.Add(new Product { Name = "Coca Cola 1l", Reference = "", Stock = 30 });

            Product p2 = await productService.Add(new Product { Name = "Fanta orange 1l", Reference = "", Stock = 30 });

            // ASSERT
            Assert.Equal("COCA0003", p1.Reference);
            Assert.Equal("FANT0001", p2.Reference);
        }

        [Fact]
        public async void AddProductAndCheckImageSize()
        {
            // ACT
            FileStream stream = File.OpenRead("Images/imageTest.jpg");
            Product p = await productService.Add(new Product { 
                Name = "vxvdfgfh", 
                Reference = "", 
                Stock = 0 
            }, stream, "image-test");

            // ASSERT

            if(p.ImageUrl != null)
            {
                var client = new HttpClient();
                var result = await client.GetAsync(p.ImageUrl);
                var bytes = await result.Content.ReadAsByteArrayAsync();
                Assert.True(bytes.Length <= 200 * 1024);
            } 
        }

        [Fact]
        public async void AddProductAndCheckCategories()
        {
            // ACT
            Product toAdd = new Product
            {
                Name = "vxvdfgfh",
                Reference = "",
                Stock = 0,
                Categories = [
                    categories[2], categories[3]
                ]
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Product p = await productService.Add(toAdd);

            });
        } 
    }
}
