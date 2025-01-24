using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string FrontEnd { get; set; } = null!;

    public string BackEnd { get; set; } = null!;

    public string DataBaseLanguage { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public int CourseFees { get; set; }

    public string CourseFormat { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
