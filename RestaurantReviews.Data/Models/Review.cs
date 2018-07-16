using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviews.Data.Models
{
    [Table("Review", Schema = "Restaurant")]
    public class Review : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        [Range(0, 10, ErrorMessage = "Rating should be between 0 and 10")]
        public int Rating { get; set; }
        public string User { get; set; }
        [StringLength(500, ErrorMessage = "Comment cannot be more than 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
    }
}
