using System.ComponentModel.DataAnnotations;

namespace CourseManagementApp.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(10, 300, ErrorMessage = "Duration must be between 10 and 300 hours")]
        [Display(Name = "Duration (Hours)")]
        public int? DurationInHours { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 50000, ErrorMessage = "Price must be a positive value")]
        [Display(Name = "Tuition Fee (EGP)")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Please assign an instructor")]
        [Display(Name = "Assigned Instructor")]
        public int? InstructorId { get; set; }
    }
}