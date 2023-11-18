using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff.Extenstions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace MifareReaderApp.Models;

public partial class User : IEditableModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; } = null!;

    public string? Name2 { get; set; } = null!;

    public string? Surname { get; set; } = null!;

    public string Card { get; set; } = null!;

    public string? Id1 { get; set; } = null!;

    public string? Id2 { get; set; } = null!;

    public DateTime? Before { get; set; }

    public int? PlaceId { get; set; }

    public bool? Staff { get; set; }

    public DateTime? Dt { get; set; }

    public virtual Place Place { get; set; } = null!;

    [NotMapped]
    public string BeforeDate { get; set; } = null!;

    [NotMapped]
    public string BeforeTime { get ; set; } = null!;

    [NotMapped]
    public string PlaceAsString { get; set; } = null!;

}
