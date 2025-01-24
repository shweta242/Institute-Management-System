using System;
using System.Collections.Generic;

namespace ManagementSystem.Models;

public partial class Note
{
    public int FileId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime UploadDate { get; set; }
}
