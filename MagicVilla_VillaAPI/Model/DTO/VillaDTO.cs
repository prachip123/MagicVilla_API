using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Model.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]//Annotations
        public string Name { get; set; }
        public string Sqmeter { get; set; }
        public string Occupancy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
