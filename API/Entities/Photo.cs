using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }

    // These 2 properties are needed to create 
    // a fully defined relationship 1:m with non-nullable fields
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
