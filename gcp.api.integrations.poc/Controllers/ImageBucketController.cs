using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gcp.api.cloud.storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace gcp.api.integrations.poc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageBucketController : ControllerBase
    {
        private readonly IGCPBucketService _gcpBucketService;
        private const string BucketName = "zorum-poc-images";

        public ImageBucketController(IGCPBucketService gcpBucketService)
        {
            _gcpBucketService = gcpBucketService;
        }
        
        [HttpGet]
        [Route("[Action]")]
        public ActionResult List()
        {
            return Ok(_gcpBucketService.List(BucketName).ToList());
        }
        
        [HttpGet]
        public ActionResult Get(string imageName)
        {
            var imageStream = _gcpBucketService.Get(BucketName, imageName);
            return File(imageStream, "image/png");
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile image)
        {
            if (image.Length <= 0) 
                return BadRequest("Error with Image - File is Empty");

            /* TODO - Implement image format validator, currently you can send any file and it will be stored*/
        
            try
            {
                var result = _gcpBucketService.Upload(BucketName, image);
                if(result != null)
                    return BadRequest(result);
            
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace ?? e.Message);
                return BadRequest(e.Message);
            }
        
        
        }
        
    }
}
