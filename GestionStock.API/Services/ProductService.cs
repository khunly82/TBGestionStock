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
            if (image != null)
            {
                // ETAPE 1 : On convertit tout le flux en tableau d'octets (byte[])
                // Cela coupe tout lien avec le flux HTTP entrant qui peut être instable
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                // On passe le tableau d'octets, plus de Stream ici
                using (var resizedStream = Resize(imageBytes, 200))
                {
                    // UploadFile prend un Stream, c'est OK ici car resizedStream est un MemoryStream propre
                    string url = await UploadFile(resizedStream, fileName ?? Guid.NewGuid().ToString());
                    p.ImageUrl = url;
                }
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


        public Stream Resize(byte[] fileBytes, int maxSizeKb)
        {
            using var original = SKBitmap.Decode(fileBytes);

            if (original == null)
            {
                throw new ArgumentException("Le format de l'image est invalide ou non supporté par SkiaSharp.");
            }

            int quality = 90;
            do
            {
                using var data = original.Encode(SKEncodedImageFormat.Webp, quality);

                byte[] resultBytes = data.ToArray();

                if (resultBytes.Length / 1024 <= maxSizeKb)
                {
                    return new MemoryStream(resultBytes);
                }
                quality -= 5;
            } while (quality >= 10);

            throw new Exception("Impossible de réduire l'image en dessous de la taille demandée.");
        }
    }
}
