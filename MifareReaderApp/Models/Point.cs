using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MifareReaderApp.Models;

public partial class Point : IHelperEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<CardEvent> CardEvents { get; set; } = new HashSet<CardEvent>();
    public virtual ICollection<Qrevent> Qrevents { get; set; } = new HashSet<Qrevent>();
    public virtual ICollection<OperatorEvent> OperatorEvents { get; set; } = new HashSet<OperatorEvent>();
}
