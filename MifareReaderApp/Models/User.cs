using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string BeforeDate { get; set; } = null!;
    [NotMapped]
    public string BeforeTime { get; set; } = null!;
}
