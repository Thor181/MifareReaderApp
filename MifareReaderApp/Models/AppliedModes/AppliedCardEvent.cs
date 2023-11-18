using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.AppliedModes
{
    public class AppliedCardEvent : IAppliedModel
    {
        [LocalizedName("Id")]
        public int Id { get; set; }

        [LocalizedName("Номер карты")]
        public string Card { get; set; } = null!;

        [LocalizedName("Место")]
        public string PointName { get; set; } = null!;

        [LocalizedName("Тип")]
        public string TypeName { get; set; } = null!;

        [LocalizedName("Метка времени")]
        public DateTime? Dt { get; set; }

        public virtual Point? Point { get; set; }
        public virtual EventsType? Type { get; set; }

        public static explicit operator AppliedCardEvent(CardEvent cardEvent)
        {
            var appliedCardEvent = new AppliedCardEvent()
            {
                Id = cardEvent.Id,
                Dt = cardEvent.Dt,
                Card = cardEvent.Card,
                PointName = cardEvent.Point.Name,
                TypeName = cardEvent.Type.Name,
                Point = cardEvent.Point,
                Type = cardEvent.Type
            };

            return appliedCardEvent;
        }

    }
}
