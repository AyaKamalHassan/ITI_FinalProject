using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementApp.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int DurationInHours { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<Trainee> Trainees { get; set; } = new List<Trainee>();
    }
}