using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public string ContactNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ParentName { get; set; } = null!;

    public string ParentContactNo { get; set; } = null!;

    public string CurrentAddress { get; set; } = null!;

    public string PermanentAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Qualification { get; set; } = null!;

    public string PreviousSchoolClg { get; set; } = null!;

    public string Marks { get; set; } = null!;

    public string Course { get; set; } = null!;

    public DateTime? AdmissionDate { get; set; }

    public string RegistrationFees { get; set; } = null!;

    public string PaymentMode { get; set; } = null!;

    public string? TransactionId { get; set; }

    public int CourseFees { get; set; }

    public int Installment { get; set; }

    public string? FirstDate { get; set; }

    public int? FirstInsta { get; set; }

    public string? SecondDate { get; set; }

    public int? SecondInsta { get; set; }

    public string? ThirdDate { get; set; }

    public int? ThirdInsta { get; set; }

    public string? FourthDate { get; set; }

    public int? FourthInsta { get; set; }

    public string? FifthDate { get; set; }

    public int? FifthInsta { get; set; }

    public string? SixthDate { get; set; }

    public int? SixthInsta { get; set; }

    public string? AddmissionTakenBy { get; set; }

    public int? InqiuryId { get; set; }

    public string? Dob { get; set; }

    public int? Discount { get; set; }

    public decimal? DiscountedCourseFees { get; set; }

    public int? CourseId { get; set; }

    public string? FrontEnd { get; set; }

    public string? BackEnd { get; set; }

    public string? DataBaseLanguage { get; set; }

    public TimeOnly? BatchTime { get; set; }

    public decimal? ReceiveAmount { get; set; }

    public decimal? PendingAmount { get; set; }

    public virtual Course? CourseNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
