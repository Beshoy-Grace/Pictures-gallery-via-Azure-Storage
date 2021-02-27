using System.Threading.Tasks;
using ImageGalleryAPI.Errors;
using ImageGalleryAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageGalleryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageGalleryController : ControllerBase
    {
         private readonly IAzureBlobService _azureBlobService;

		public ImageGalleryController(IAzureBlobService azureBlobService)
		{
			_azureBlobService = azureBlobService;
		}

        [HttpGet]
        public async Task<ActionResult> Index()
		{
		
				var allBlobs = await _azureBlobService.ListAsync();
				return Ok(allBlobs);
			
		}


        [HttpPost]
		public async Task<ActionResult> UploadAsync()
		{
			
				var request = await HttpContext.Request.ReadFormAsync();
				if (request.Files == null)
				{
                   
					return BadRequest( );
				}
				var files = request.Files;
				if(files.Count == 0)
				{
					return BadRequest(new ApiResponse(400, "Could not upload empty files"));
				}

				await _azureBlobService.UploadAsync(files);
				return RedirectToAction("Index");
			
		
		}

		[HttpGet("Delete")]
		public async Task<ActionResult> DeleteImage(string fileUri)
		{
			
				await _azureBlobService.DeleteAsync(fileUri);
				return RedirectToAction("Index");
			
			
		}

		[HttpGet("DeleteAll")]
		public async Task<ActionResult> DeleteAll()
		{
			
				await _azureBlobService.DeleteAllAsync();
				return RedirectToAction("Index");
			
			
		}

    }
}