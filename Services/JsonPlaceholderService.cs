namespace CSharpApi.Services;

public class PostDto
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class JsonPlaceholderService(HttpClient httpClient, ILogger<JsonPlaceholderService> logger)
{
    public async Task<List<PostDto>?> GetAllPostsAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("posts");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<PostDto>>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching posts from JSONPlaceholder API");
            throw;
        }
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        try
        {
            var response = await httpClient.GetAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PostDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching post {PostId} from JSONPlaceholder API", id);
            throw;
        }
    }

    public async Task<List<PostDto>?> GetPostsByUserIdAsync(int userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"posts?userId={userId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<PostDto>>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching posts for user {UserId} from JSONPlaceholder API", userId);
            throw;
        }
    }

    public async Task<PostDto?> CreatePostAsync(PostDto post)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("posts", post);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PostDto>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error creating post in JSONPlaceholder API");
            throw;
        }
    }
}
