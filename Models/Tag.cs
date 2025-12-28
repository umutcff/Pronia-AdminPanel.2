using ProniaUmut.Models.Common;

namespace ProniaUmut.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}
