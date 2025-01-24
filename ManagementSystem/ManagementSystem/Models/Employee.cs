using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string SpecializedSubject { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string DateOfJoing { get; set; } = null!;

    public int Salary { get; set; }

    public string IdProofType { get; set; } = null!;

    public string IdProofNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Experience { get; set; } = null!;
}
