using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.AppliedModes
{
    public class AppliedQREvent : IAppliedModel
    {
        [LocalizedName("Id")]
        public Guid Id { get; set; }

        [LocalizedName("Сумма")]
        public decimal Sum { get; set; }

        [LocalizedName("FN")]
        public string Fn { get; set; } = null!;

        [LocalizedName("FP")]
        public string Fp { get; set; } = null!;

        [LocalizedName("Оплата")]
        public string PayName { get; set; } = null!;

        [LocalizedName("Место")]
        public string PointName { get; set; } = null!;

        [LocalizedName("Тип")]
        public string TypeName { get; set; } = null!;

        [LocalizedName("Метка времени")]
        public DateTime? Dt { get; set; }

        public virtual Point Point { get; set; } = null!;
        public virtual PayType Pay { get; set; } = null!;
        public virtual EventsType Type { get; set; } = null!;

        public static explicit operator AppliedQREvent(Qrevent qrevent)
        {
            var appliedQREvent = new AppliedQREvent()
            {
                Id = qrevent.Id,
                Sum = qrevent.Sum,
                Fn = qrevent.Fn,
                Fp = qrevent.Fp,
                PointName = qrevent.Point.Name,
                PayName = qrevent.Pay.Name,
                TypeName = qrevent.Type.Name,
                Point = qrevent.Point,
                Pay = qrevent.Pay,
                Type = qrevent.Type,
                Dt = qrevent.Dt
            };

            return appliedQREvent;
        }
    }
}
