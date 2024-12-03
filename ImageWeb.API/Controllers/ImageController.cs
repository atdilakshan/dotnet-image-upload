using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController()
        {

        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            try
            {
                var cloudinaryUrl = "cloudinary://<API_KEY>:<API_SECRET>@<CLOUD_NAME>";

                Cloudinary cloudinary = new Cloudinary(cloudinaryUrl);

                using (var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = true
                    };

                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    // Return only necessary details
                    return Ok(new
                    {
                        Url = uploadResult.SecureUrl,
                        PublicId = uploadResult.PublicId,
                        Format = uploadResult.Format
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
