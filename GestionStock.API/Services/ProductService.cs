using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GestionStock.API.Data;
using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace GestionStock.API.Services
{
    public class ProductService(StockContext db, IConfiguration configuration)
    {
        public List<Product> Get()
        {
            return db.Products
                .Include(p => p.Prices)
                .Include(p => p.Categories).ToList();
        }
        public async Task<Product> Add(Product p, Stream? image = null, string? fileName = null)
        {
            // Reference COCA0002
            string fourLetters = p.Name[..4].ToUpper();
            int count = db.Products.Count(
                p => p.Reference.StartsWith(fourLetters)
            );
            string numbers = (count + 1).ToString().PadLeft(4, '0');
            p.Reference = $"{fourLetters}{numbers}";

            // pas Soft + Alcool 
            if(p.Categories != null && p.Categories.Any(c => c.Name == "Alcool") && p.Categories.Any(c => c.Name == "Soft"))
            {
                throw new ArgumentException("Incohérence des données (alcool, soft)");
            }

            // Image max 200ko
            if(image != null)
            {
                // reduire l'image à 200ko max
                using Stream stream = Resize(image, 200);
                string url = await UploadFile(stream, fileName ?? Guid.NewGuid().ToString());
                p.ImageUrl = url;
            }

            db.Products.Add(p);
            db.SaveChanges();

            return p;
        }

        public async Task<string> UploadFile(Stream stream, string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("products-images");
            await containerClient.CreateIfNotExistsAsync();

            // nom du blob qui sera créé
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/webp",
                },
            });

            return "https://technobel.blob.core.windows.net/products-images/" + fileName;
        }


        public Stream Resize(Stream stream, int maxSizeKb)
        {
            var original = SKBitmap.Decode(stream);

            int quality = 90;
            do
            {
                using var encoded = original.Encode(SKEncodedImageFormat.Webp, quality);
                var ms = new MemoryStream();
                using (var encodedStream = encoded.AsStream())
                {
                    encodedStream.CopyTo(ms);
                }
                ms.Position = 0;

                if (ms.Length / 1024 <= maxSizeKb)
                    return ms;
                quality -= 5;
            } while (quality >= 10);

            throw new Exception("Impossible de reduire l'image");
        }
    }
}
