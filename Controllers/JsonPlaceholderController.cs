using CSharpApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JsonPlaceholderController(JsonPlaceholderService jsonPlaceholderService, ILogger<JsonPlaceholderController> logger) : ControllerBase
{
    [HttpGet("posts")]
    public async Task<ActionResult<List<PostDto>>> GetAllPosts()
    {
        try
        {
            var posts = await jsonPlaceholderService.GetAllPostsAsync();

            if (posts == null || posts.Count == 0)
            {
                return NotFound("Nenhum post encontrado");
            }

            return Ok(posts);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Erro ao comunicar com a API externa");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao buscar posts");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("posts/{id}")]
    public async Task<ActionResult<PostDto>> GetPostById(int id)
    {
        try
        {
            var post = await jsonPlaceholderService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound($"Post com ID {id} não encontrado");
            }

            return Ok(post);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Erro ao comunicar com a API externa");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao buscar post {PostId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("users/{userId}/posts")]
    public async Task<ActionResult<List<PostDto>>> GetPostsByUserId(int userId)
    {
        try
        {
            var posts = await jsonPlaceholderService.GetPostsByUserIdAsync(userId);

            if (posts == null || posts.Count == 0)
            {
                return NotFound($"Nenhum post encontrado para o usuário {userId}");
            }

            return Ok(posts);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Erro ao comunicar com a API externa");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao buscar posts do usuário {UserId}", userId);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPost("posts")]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto post)
    {
        try
        {
            var createdPost = await jsonPlaceholderService.CreatePostAsync(post);

            if (createdPost == null)
            {
                return StatusCode(500, "Erro ao criar post");
            }

            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Erro ao comunicar com a API externa");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao criar post");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
