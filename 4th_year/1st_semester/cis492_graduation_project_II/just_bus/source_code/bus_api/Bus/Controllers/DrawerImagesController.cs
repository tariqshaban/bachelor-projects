using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bus.Models;
using WebApi.Authorization;
using System.IO;
using System.Collections.Generic;
using System;

namespace Bus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DrawerImagesController : ControllerBase
    {
        // GET: api/Image
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDrawerImages()
        {
            try
            {
                DirectoryInfo directory = new(@"images/drawer");
                List<string> imageNames = directory.GetFiles().Select(x => x.Name).ToList();
                string image = imageNames[new Random().Next(imageNames.Count)];
                return Ok(new { image });
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
