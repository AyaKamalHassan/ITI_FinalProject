using Microsoft.Identity.Client.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementApp.Models
{
    [Table("Instructors")]
    public class Instructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string Specialization { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}