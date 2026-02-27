using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Villa")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a villa.")]
        public int VillaId { get; set; }

        public Villa? Villa { get; set; }
    }
}
