using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Payment
{
    public int Pid { get; set; }

    public string StudentName { get; set; } = null!;

    public int? StudentId { get; set; }

    public string MobileNo { get; set; } = null!;

    public string Gmail { get; set; } = null!;

    public string ParentName { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public DateOnly ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal? FeesStatus { get; set; }

    public int RecievedAmount { get; set; }

    public string RecievedMode { get; set; } = null!;

    public string TransactionStatus { get; set; } = null!;

    public string? TransactionNumber { get; set; }

    public string Narration { get; set; } = null!;

    public string ReceivedBy { get; set; } = null!;

    public string TermCondition { get; set; } = null!;

    public string ReceiptRemark { get; set; } = null!;

    public DateOnly? TransactionDate { get; set; }

    public decimal? PendingAmount { get; set; }

    public virtual Student? Student { get; set; }
}
