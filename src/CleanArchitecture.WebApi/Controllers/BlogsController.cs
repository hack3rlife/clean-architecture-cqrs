using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Delete;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogsController : ApiController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public BlogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Blogs
        /// <summary>
        /// Gets a list of blogs
        /// </summary>
        /// <response code="200">Returns a list of existing blogs</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IAsyncEnumerable<GetBlogsQueryResponse>>> Get([FromQuery] GetBlogsRequestQuery query)
        {
            var blogs = await _mediator.Send(query);

            if (blogs == null)
            {
                return BadRequest();
            }

            return Ok(blogs);
        }

        // GET: api/Blogs/5
        /// <summary>
        /// Gets a single blog details
        /// </summary>
        /// <param name="getByBlogQuery">Blog's guid</param>
        /// <response code="200">When a valid <see cref="Guid"/> value is passed, the related Blog details will be returned.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <response code="404">When the provided <see cref="Guid"/> does not exist or is not related any <see cref="Blog"/> in the database.</response>
        /// <returns></returns>
        [HttpGet("{BlogId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetByBlogIdQueryResponse>> Get([FromRoute] GetByBlogIdRequestQuery getByBlogQuery)
        {
            var blog = await _mediator.Send(getByBlogQuery);

            if (blog == null)
            {
                return NotFound(getByBlogQuery);
            }

            return Ok(blog);

        }

        // POST: api/Blogs
        /// <summary>
        /// Adds a new blog
        /// </summary>
        /// <param name="addBlogCommand"></param>
        /// <response code="201">When the blog is added successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AddBlogRequestCommand addBlogCommand)
        {
            var newBlog = await _mediator.Send(addBlogCommand);

            if (newBlog == null)
            {
                return BadRequest(addBlogCommand);
            }

            return CreatedAtAction(nameof(Post), newBlog);
        }

        // PUT: api/Blogs/5
        /// <summary>
        /// Updates an existing blog
        /// </summary>
        /// <param name="updateBlogCommand"></param>
        /// <response code="204">When the blog is updated successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] UpdateBlogRequestCommand updateBlogCommand)
        {
            await _mediator.Send(updateBlogCommand);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Deletes an existing blog
        /// </summary>
        /// <param name="deleteBlogCommand"></param>
        /// <response code="204">When the blog is deleted successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpDelete("{BlogId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeleteBlogRequestCommand deleteBlogCommand)
        {
            await _mediator.Send(deleteBlogCommand);

            return NoContent();
        }
    }
}