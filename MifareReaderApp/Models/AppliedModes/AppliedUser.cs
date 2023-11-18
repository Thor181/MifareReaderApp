using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;
using MifareReaderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.AppliedModes
{
    public class AppliedUser : IAppliedModel
    {
        [LocalizedName("Id")]
        public Guid Id { get; set; }

        [LocalizedName("Имя")]
        public string? Name { get; set; } = null!;

        [LocalizedName("Отчество")]
        public string? Name2 { get; set; } = null!;

        [LocalizedName("Фамилия")]
        public string? Surname { get; set; } = null!;

        [LocalizedName("Номер карты")]
        public string Card { get; set; } = null!;

        [LocalizedName("Id1")]
        public string? Id1 { get; set; } = null!;

        [LocalizedName("Id2")]
        public string? Id2 { get; set; } = null!;

        [LocalizedName("Срок действия")]
        public DateTime? Before { get; set; }

        [LocalizedName("Сотрудник")]
        public bool? Staff { get; set; }

        [LocalizedName("Местонахождение")]
        public string PlaceName { get; set; } = null!;

        [LocalizedName("Метка времени")]
        public DateTime? Dt { get; set; }

        public virtual Place Place { get; set; } = null!;

        public AppliedUser()
        {
        }

        public static explicit operator AppliedUser(User user)
        {
            var appliedUser = new AppliedUser
            {
                Id = user.Id,
                Name = user.Name,
                Name2 = user.Name2,
                Surname = user.Surname,
                Card = user.Card,
                Id1 = user.Id1,
                Id2 = user.Id2,
                Before = user.Before,
                Staff = user.Staff,
                PlaceName = user.Place.Name,
                Place = user.Place,
                Dt = user.Dt
            };

            return appliedUser;
        }
    }
}
