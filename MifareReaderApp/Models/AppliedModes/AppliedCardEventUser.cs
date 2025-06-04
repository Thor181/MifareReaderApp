using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.AppliedModes
{
    public class AppliedCardEventUser : IAppliedModel
    {
        [LocalizedName("Id")]
        public int Id { get; set; }

        [LocalizedName("Номер карты")]
        public string Card { get; set; } = null!;

        [LocalizedName("Место")]
        public string PointName { get; set; } = null!;

        [LocalizedName("Тип")]
        public string TypeName { get; set; } = null!;

        [OverrideVisible(true)]
        [LocalizedName("Метка времени")]
        public DateTime? Dt { get; set; }

        [LocalizedName("ФИО")]
        public string UserFullName { get; set; }

        [LocalizedName("ID1")]
        public string UserId1 { get; set; }

        [LocalizedName("ID2")]
        public string UserId2 { get; set; }

        [LocalizedName("Срок действия")]
        public DateTime? UserTo { get; set; }

        [LocalizedName("Сотрудник")]
        public string? IsEmployee { get; set; }

        public virtual Point? Point { get; set; }
        public virtual EventsType? Type { get; set; }

        public static AppliedCardEventUser CreateWithUser(CardEvent cardEvent, User? user)
        {
            return new AppliedCardEventUser()
            {
                Id = cardEvent.Id,
                Dt = cardEvent.Dt,
                Card = cardEvent.Card,
                PointName = cardEvent.Point.Name,
                TypeName = cardEvent.Type.Name,
                Point = cardEvent.Point,
                Type = cardEvent.Type,
                UserFullName = user?.FullName ?? "Карта не найдена", 
                UserId1 = user?.Id1 ?? string.Empty,
                UserId2 = user?.Id2 ?? string.Empty,
                UserTo = user?.Before ?? null,
                IsEmployee = user == null 
                ? null 
                : user?.Staff == true 
                    ? "Да" 
                    : "Нет"
            };
        }
    }
}
