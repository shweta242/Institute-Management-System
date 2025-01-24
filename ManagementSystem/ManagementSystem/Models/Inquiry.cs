using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Inquiry
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Contact { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Qualification { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Course { get; set; } = null!;

    public int Fees { get; set; }

    public string Remark { get; set; } = null!;

    public string InquiryTakenBy { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? InquiryDate { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? CourseNavigation { get; set; }
}
