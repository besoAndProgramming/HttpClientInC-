using HTTPClientDemo.HttpClientServices;
using HTTPClientDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly CRUDHTTPService _service;
        private readonly string _url = "https://jsonplaceholder.typicode.com/posts";
        public PostsController(CRUDHTTPService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _service.GetAll<Post>(_url);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var result = await _service.GetById<Post>(_url, id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Post post)
        {
            var result = await _service.Create<Post>(_url, post);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Post post)
        {
            var result = await _service.Update<Post>(_url, post);

            return Ok(result);
        }
    }
}
