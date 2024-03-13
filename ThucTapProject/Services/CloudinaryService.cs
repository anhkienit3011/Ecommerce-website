using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ThucTapProject.Services
{
    public class CloudinaryService
    {
        static private readonly Random rnd = new Random();
        static private Account account = new Account(
              "dift2vpcj",
              "561511787319433",
              "mbsTouvLkL_9p23eSe-Ydda0r6E");

        static public Cloudinary cloudinary = new Cloudinary(account);

        public static async Task<string> uploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                await Console.Out.WriteLineAsync("null image");
                return null;
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = "cloudavt"+ rnd.Next(34569,435436546) + DateTime.Now.ToShortDateString() // ID công khai tùy ý cho file
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    // Xử lý lỗi tải lên
                    throw new Exception(uploadResult.Error.Message);
                }

                // Lấy URL công khai của file tải lên
                string imageUrl = uploadResult.SecureUrl.ToString();

                return imageUrl;
            }
        }

        public async static Task<string> deleteImage(string url)
        {
            string publicId = await getPublicId(url);
            var DeleteResult = await cloudinary.DeleteResourcesAsync(
                new DelResParams
                {
                    PublicIds = new List<string> { publicId },
                    ResourceType = ResourceType.Image
                });

            if (DeleteResult.Deleted.Count > 0)
            {
                return "ok";
            }
            else
            {
                return "Fail";
            }
        }

        private async static Task<string> getPublicId(string url)
        {
            int startIndex = url.IndexOf("cloudavt");
            int endIndex = url.LastIndexOf('.');
            string publicId = url.Substring(startIndex, endIndex-startIndex);
            return publicId;
        }
    }
}
