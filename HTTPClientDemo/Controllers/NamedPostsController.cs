using HTTPClientDemo.HttpClientServices;
using HTTPClientDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HTTPClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamedPostsController : ControllerBase
    {
        private readonly NamedCRUDHTTPService _service;
        public NamedPostsController(NamedCRUDHTTPService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {

            var result = await _service.GetAll<Post>();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var result = await _service.GetById<Post>(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Post post)
        {
            var result = await _service.Create<Post>(post);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Post post)
        {
            var result = await _service.Update<Post>(post);

            return Ok(result);
        }
    }
}
