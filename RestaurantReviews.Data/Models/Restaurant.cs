using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviews.Data.Models
{
    [Table("Restaurant", Schema = "Restaurant")]
    public class Restaurant : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage = "Zipcode is required")]
        [RegularExpression("[0-9]{5}", ErrorMessage = "Invalid input")]
        [DataType(DataType.PostalCode)]
        public string Zipcode { get; set; }
        [RegularExpression("[(]{1}[0-9]{3}[)]{1}[ ]{1}[0-9]{3}[-]{1}[0-9]{4}", ErrorMessage = "Format must be (###) ###-####")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.Url)]
        public string Website { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        [NotMapped]
        public double AverageRating { get; set; }
        [NotMapped]
        public string Address { get { return $"{Street}" +"\n"+ $"{City}, {State} {Zipcode}"; } }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
    }
}
