using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace MifareReaderApp.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Name2 { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Card { get; set; } = null!;

    public string Id1 { get; set; } = null!;

    public string Id2 { get; set; } = null!;

    public DateTime Before { get; set; }

    public int PlaceId { get; set; }

    public bool Staff { get; set; }

    public DateTime Dt { get; set; }

    public virtual Place Place { get; set; } = null!;

    [NotMapped]
    private string _beforeDate = string.Empty!;
    [NotMapped]
    public string BeforeDate { get => Before.ToShortDateString(); set => _beforeDate = value; }

    [NotMapped]
    private string _beforeTime = string.Empty!;
    [NotMapped]
    public string BeforeTime { get => Before.ToShortTimeString(); set => Before.AddDays(-1).AddTicks(1).Add(TimeSpan.Parse(value));  }
}
