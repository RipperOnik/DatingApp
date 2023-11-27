namespace API.Helpers;

// pagination params from user 
public class UserParams : PaginationParams
{
    // Filtering 
    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
}
