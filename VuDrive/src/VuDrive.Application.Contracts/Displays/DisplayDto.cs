// Displays/DisplayDto.cs
using System;
using Volo.Abp.Application.Dtos;

namespace VuDrive.Displays;

public class DisplayDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = default!;
    public decimal SizeInInches { get; set; }
    public string AndroidVersion { get; set; } = default!;
    public int Ram { get; set; }
    public int Memory { get; set; }
    public string Cpu { get; set; } = default!;
}

public class CreateUpdateDisplayDto
{
    public string Name { get; set; } = default!;
    public decimal SizeInInches { get; set; }
    public string AndroidVersion { get; set; } = default!;
    public int Ram { get; set; }
    public int Memory { get; set; }
    public string Cpu { get; set; } = default!;
}
