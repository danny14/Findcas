using Microsoft.AspNetCore.Mvc;
using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;


namespace Findcas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {

        private readonly IPropertyService _propertyService;
        private readonly IImageService _imageService;

        public PropertiesController(IPropertyService propertyService, IImageService imageService)
        {
            _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
            _imageService = imageService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Property property)
        {
            try
            {
                var imageFile = Request.Form.Files.GetFile("ImageFile") ?? Request.Form.Files.FirstOrDefault();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var (url, publicId) = await _imageService.UploadImageAsync(imageFile);

                    // Vinculamos la foto a la finca
                    property.Images = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Url = url,
                            PublicId = publicId,
                            IsMain = true
                        }
                    };
                }

                var propertyId = await _propertyService.CreatePropertyAsync(property);

                return Ok(propertyId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        [HttpPost("test-upload")]
        // ¡SIN el [FromForm]!
        public async Task<IActionResult> TestUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("El servidor no recibió ningún archivo.");

            try
            {
                var (url, publicId) = await _imageService.UploadImageAsync(file);
                return Ok(new { Mensaje = "¡Éxito!", Url = url, PublicId = publicId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno de Cloudinary: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);

                if (property == null)
                {
                    return NotFound($"No se encontró ninguna finca con el ID {id}");
                }

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


    }
}
