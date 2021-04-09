using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.External
{
    public class ImageManager : IImageManager
    {
        private readonly string cloudName;
        private readonly string apiKey;
        private readonly string apiSecret;

        public ImageManager(string cloudName, string apiKey, string apiSecret)
        {
            this.cloudName = cloudName;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        /// <summary>
        /// Uploads new image with selected parameters
        /// </summary>
        /// <param name="fileName">Name of the file to upload</param>
        /// <param name="imageMemoryStream">Memory stream containing the image data</param>
        /// <returns>Image uri or null if not successful or Error message starting with "Error"</returns>
        public async Task<string> UploadImageAsync(MemoryStream imageMemoryStream, string fileName)
        {
            imageMemoryStream.Position = 0;
            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);

            string publicId = Guid.NewGuid().ToString() + fileName;
            var file = new FileDescription(fileName, imageMemoryStream);

            var uploadParams = new ImageUploadParams
            {
                File = file,
                Format = "jpg",
                PublicId = publicId,
                UseFilename = true,
            };

            uploadParams.Check();
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            var uri = uploadResult.SecureUrl;
            return uri?.AbsoluteUri;
        }
    }
}
