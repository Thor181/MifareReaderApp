using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models
{
    public class OperatorEvent : IEditableModel
    {
        public int Id { get; set; }
        public DateTime Dt { get; set; }
        public int TypeId { get; set; }
        public int PointId { get; set; }

        public virtual EventsType EventType { get; set; } = null!;
        public virtual Point Point { get; set; } = null!;
    }
}
