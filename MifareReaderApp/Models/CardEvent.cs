
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;

namespace MifareReaderApp.Models;

public partial class CardEvent : IEditableModel
{
    public int Id { get; set; }

    [LocalizedName("Метка времени")]
    public DateTime Dt { get; set; }

    [OverrideVisible(false)]
    public int TypeId { get; set; }

    [OverrideVisible(false)]
    public int PointId { get; set; }

    [LocalizedName("Номер карты")]
    public string Card { get; set; } = null!;

    [LocalizedName("Место")]
    public virtual Point Point { get; set; } = null!;

    [LocalizedName("Тип")]
    public virtual EventsType Type { get; set; } = null!;
}
