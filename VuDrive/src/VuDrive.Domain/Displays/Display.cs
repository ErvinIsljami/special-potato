using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace VuDrive.Displays;

public class Display : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = default!;
    public decimal SizeInInches { get; set; }
    public string? AndroidVersion { get; set; } // optional
    public int Ram { get; set; }
    public int? Memory { get; set; }
    public string? Cpu { get; set; } // optional

    protected Display() { }

    public Display(Guid id, string name, decimal size, string? android, int ram, int memory, string? cpu)
        : base(id)
    {
        Name = name;
        SizeInInches = size;
        AndroidVersion = android;
        Ram = ram;
        Memory = memory;
        Cpu = cpu;
    }

}
