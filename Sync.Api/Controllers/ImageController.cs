using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sync.Core.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<ImageController>
        [HttpGet("field/{fieldId}")]
        public async Task<IActionResult> GetSortedImagesByFieldIdAsync(Guid fieldId)
        {
            var images = await _mediator.Send(new GetDatesForFieldQuery(fieldId));
            return Ok(images);
        }

        // GET api/<ImageController>/5
        [HttpGet("GroupedByDate")]
        public async Task<IActionResult> GetGroupedImagesAsync()
        {
            var images = await _mediator.Send(new GetGroupedImagesQuery());
            return Ok(images);
        }

        [HttpGet("GetValidField")]
        public async Task<IActionResult> GetGroupedImagesAsync([FromQuery] DateTime date)
        {
            var images = await _mediator.Send(new GetValidFieldQuery(date));
            return Ok(images);
        }
    }
}
