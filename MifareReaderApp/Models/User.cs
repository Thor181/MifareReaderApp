using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff.Extenstions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace MifareReaderApp.Models;

public partial class User : IEditableModel, INotifyPropertyChanged
{
    private Guid id;
    private string? name = null!;
    private string? name2 = null!;
    private string? surname = null!;
    private string card = null!;
    private string? id1 = null!;
    private string? id2 = null!;
    private DateTime? before;
    private int? placeId;
    private bool? staff;
    private DateTime? dt;
    private Place place = null!;
    private string beforeDate = null!;
    private string beforeTime = null!;
    private string placeAsString = null!;

    public Guid Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string? Name { get => name; set { name = value; OnPropertyChanged(); } }
    public string? Name2 { get => name2; set { name2 = value; OnPropertyChanged(); } }
    public string? Surname { get => surname; set { surname = value; OnPropertyChanged(); } }
    public string Card { get => card; set { card = value; OnPropertyChanged(); } }
    public string? Id1 { get => id1; set { id1 = value; OnPropertyChanged(); } }
    public string? Id2 { get => id2; set { id2 = value; OnPropertyChanged(); } }
    public DateTime? Before { get => before; set { before = value; OnPropertyChanged(); } }
    public int? PlaceId { get => placeId; set { placeId = value; OnPropertyChanged(); } }
    public bool? Staff { get => staff; set { staff = value; OnPropertyChanged(); } }
    public DateTime? Dt { get => dt; set { dt = value; OnPropertyChanged(); } }
    public virtual Place Place { get => place; set { place = value; OnPropertyChanged(); } }

    [NotMapped]
    public string BeforeDate { get => beforeDate; set { beforeDate = value; OnPropertyChanged(); } }
    [NotMapped]
    public string BeforeTime { get => beforeTime; set { beforeTime = value; OnPropertyChanged(); } }
    [NotMapped]
    public string PlaceAsString { get => placeAsString; set { placeAsString = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public User Clone()
    {
        return new User()
        {
            Id = Id,
            Name = Name,
            Name2 = Name2,
            Surname = Surname,
            Card = Card,
            Id1 = Id1,
            Id2 = Id2,
            Before = Before,
            PlaceId = PlaceId,
            Staff = Staff,
            Dt = Dt,
            Place = Place,
            BeforeDate = BeforeDate,
            BeforeTime = BeforeTime,
            PlaceAsString = PlaceAsString,
        };
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
