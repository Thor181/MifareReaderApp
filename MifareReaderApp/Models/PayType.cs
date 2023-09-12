using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MifareReaderApp.Models;

public partial class PayType : IHelperEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Qrevent> Qrevents { get; set; } = new List<Qrevent>();
}
