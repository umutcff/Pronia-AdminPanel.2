using System.ComponentModel.DataAnnotations;

namespace ProniaUmut.Models.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
