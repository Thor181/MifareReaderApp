using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;

namespace MifareReaderApp.Models.AppliedModes
{
    public class AppliedOperatorEvent : IAppliedModel
    {
        [LocalizedName("Id")]
        public int Id { get; set; }

        [LocalizedName("Место")]
        public string PointName { get; set; } = null!;

        [LocalizedName("Тип")]
        public string TypeName { get; set; } = null!;

        [OverrideVisible(true)]
        [LocalizedName("Метка времени")]
        public DateTime? Dt { get; set; }

        public virtual Point? Point { get; set; }
        public virtual EventsType? Type { get; set; }

        public static explicit operator AppliedOperatorEvent(OperatorEvent cardEvent)
        {
            var appliedCardEvent = new AppliedOperatorEvent()
            {
                Id = cardEvent.Id,
                Dt = cardEvent.Dt,
                PointName = cardEvent.Point.Name,
                TypeName = cardEvent.EventType.Name,
                Point = cardEvent.Point,
                Type = cardEvent.EventType
            };

            return appliedCardEvent;
        }
    }
}
