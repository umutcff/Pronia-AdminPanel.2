using Microsoft.EntityFrameworkCore.Query.Internal;
using ProniaUmut.Models.Common;

namespace ProniaUmut.Models;

public class ProductRating:BaseEntity
{
    public Product Product { get; set; } = null!;
    public int ProductId {  get; set; }
    public Rating Rating { get; set; }=null!;
    public int RatingId { get; set; }

}
