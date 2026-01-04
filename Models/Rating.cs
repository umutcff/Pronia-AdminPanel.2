using ProniaUmut.Models.Common;

namespace ProniaUmut.Models;

public class Rating:BaseEntity
{
    public int StarCount {  get; set; }
    public ICollection<ProductRating> ProductRatings { get; set; } = [];
}
