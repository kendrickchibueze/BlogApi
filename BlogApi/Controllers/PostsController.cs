using BlogApi.Data;
using BlogApi.Models.Dto;
using BlogApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PostsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts =  await _dbContext.Posts.ToListAsync();

            return Ok(posts);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(Guid id)
        {

            var post = await _dbContext.Posts.FirstOrDefaultAsync(x=>x.Id == id);   
            if(post != null)
            {
                return Ok(post);
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            //convert DTO to entity
            //we achieve this with automapper
            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Summary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visible = addPostRequest.Visible,

            };

            post.Id = Guid.NewGuid();
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();


            //access the location of this post by using the header field of the location
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatePostRequest)
        {


            //check if the post exists in the db
           var existingPost =  await _dbContext.Posts.FindAsync(id);
            if(existingPost != null)
            {

                existingPost.Title = updatePostRequest.Title;
                existingPost.Content = updatePostRequest.Content;
                existingPost.Author = updatePostRequest.Author;
                existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
                existingPost.PublishDate = updatePostRequest.PublishDate;
                existingPost.UpdatedDate = updatePostRequest.UpdatedDate;
                existingPost.Summary = updatePostRequest.Summary;
                existingPost.UrlHandle = updatePostRequest.UrlHandle;
                existingPost.Visible = updatePostRequest.Visible;

                await _dbContext.SaveChangesAsync();

                return Ok(existingPost);

            }
            return NotFound();

        }


        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeletePost(Guid id)
        {

            var existingPost = await _dbContext.Posts.FindAsync(id);
            if(existingPost != null)
            {
                _dbContext.Remove(existingPost);
                await _dbContext.SaveChangesAsync();

                return Ok(existingPost);
            }

            return NotFound();

        }
    }
}
