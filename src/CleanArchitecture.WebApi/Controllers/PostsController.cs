using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Delete;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Update;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;


        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Posts
        /// <summary>
        /// Gets a list of posts
        /// </summary>
        /// <response code="200">Returns a list of existing posts</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IAsyncEnumerable<GetPostsQueryResponse>>> Get([FromQuery] GetPostsQuery getPostsQuery)
        {
            var posts = await _mediator.Send(getPostsQuery);

            if (posts == null)
            {
                return BadRequest();
            }

            return Ok(posts);
        }

        // GET: api/Posts/5
        /// <summary>
        /// Gets a single post details
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200">When a valid <see cref="Guid"/> value is passed, the related Blog details will be returned.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <response code="404">When the provided <see cref="Guid"/> does not exist or is not related any <see cref="Blog"/> in the database.</response>
        /// <returns></returns>
        [HttpGet("{PostId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetByPostIdQueryResponse>> Get([FromRoute] GetByPostIdQuery query)
        {
            var blog = await _mediator.Send(query);

            if (blog == null)
            {
                return NotFound(query);
            }

            return Ok(blog);
        }

        // POST: api/Posts
        /// <summary>
        /// Adds a new post
        /// </summary>
        /// <param name="addPostCommand"></param>
        /// <response code="201">When the post is added successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddPostCommandResponse>> Post([FromBody] AddPostRequestCommand addPostCommand)
        {
            var newPost = await _mediator.Send(addPostCommand);

            if (newPost == null)
            {
                return BadRequest(addPostCommand);
            }

            return CreatedAtAction(nameof(Post), newPost);
        }

        // PUT: api/Posts/5
        /// <summary>
        /// Updates an existing post
        /// </summary>
        /// <param name="post"></param>
        /// <param name="updatePostCommand"></param>
        /// <response code="204">When the post is updated successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put([FromBody] UpdatePostRequestCommand updatePostCommand)
        {
            await _mediator.Send(updatePostCommand);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Deletes an existing post
        /// </summary>
        /// <param name="deletePostCommand"></param>
        /// <response code="204">When the post is deleted successfully.</response>
        /// <response code="400">When there is an error during the execution of the request.</response>
        /// <returns></returns>
        [HttpDelete("{PostId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeletePostRequestCommand deletePostCommand)
        {
            await _mediator.Send(deletePostCommand);

            return NoContent();
        }
    }
}
