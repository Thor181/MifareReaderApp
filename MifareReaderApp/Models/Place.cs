using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MifareReaderApp.Models;

public partial class Place : IHelperEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
